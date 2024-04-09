using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.CustomEditors
{
    public abstract class HideablePropertyDrawer<T> : PropertyDrawer
    {
        private const int propertyHeight = 18;

        protected virtual bool CanBeHidden => true;
        protected virtual bool Indent => true;
        protected virtual bool ShowLabel => true;
        protected virtual bool InlineIfOnlyOneProperty => false;

        private readonly List<string> fieldNames = new List<string>();
        private readonly List<ShowWhenAttribute> showWhenAttributes = new List<ShowWhenAttribute>();

        public HideablePropertyDrawer()
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                if ((field.IsPrivate && field.GetCustomAttribute<SerializeField>() != null) || (field.IsPublic && field.GetCustomAttribute<HideInInspector>() == null))
                {
                    fieldNames.Add(field.Name);
                    showWhenAttributes.Add(field.GetCustomAttribute<ShowWhenAttribute>());
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var isExpandedProperty = property.FindPropertyRelative("isExpanded");
            if (isExpandedProperty == null)
            {

            }
            if (isExpandedProperty.boolValue)
            {
                var visibilityEvaluation = EvaluateVisibleProperties(property, out var moreThanOne);
                var sum = 0f;
                foreach (var prop in VisibleProperties(property, visibilityEvaluation))
                {
                    sum += EditorGUI.GetPropertyHeight(prop, true);
                }

                if (!InlineIfOnlyOneProperty || moreThanOne)
                {
                    sum += propertyHeight;
                }

                return sum - (ShowLabel || CanBeHidden ? 0 : propertyHeight);
            }

            return propertyHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var visibilityEvaluation = EvaluateVisibleProperties(property, out var moreThanOne);
            label = new GUIContent(label.text);
            var isExpandedProperty = property.FindPropertyRelative("isExpanded");
            var shouldBeInlined = InlineIfOnlyOneProperty && !moreThanOne && CanBeInlined(property, visibilityEvaluation);
            var labelHeight = 0;
            EditorGUI.BeginProperty(position, label, property);
            property.isExpanded = false;
            if (CanBeHidden)
            {
                isExpandedProperty.boolValue = !ShowLabel || EditorGUI.Foldout(new Rect(position.x, position.y, position.width, propertyHeight), isExpandedProperty.boolValue, label, true);
            }
            else if (ShowLabel && !shouldBeInlined)
            {
                EditorGUI.LabelField(new Rect(position.x, position.y, position.width, propertyHeight), label);
                labelHeight = propertyHeight;
                isExpandedProperty.boolValue = true;
            }
            else
            {
                isExpandedProperty.boolValue = true;
            }

            if (isExpandedProperty.boolValue)
            {
                var y = position.y;
                if (ShowLabel && !shouldBeInlined)
                {
                    y += propertyHeight;
                }
                if (Indent && !shouldBeInlined)
                {
                    EditorGUI.indentLevel++;
                }

                DrawChildProperties(new Rect(position.x, y, position.width, position.height - labelHeight), property, label, shouldBeInlined, visibilityEvaluation);

                if (Indent && !shouldBeInlined)
                {
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUI.EndProperty();
        }

        protected virtual void DrawChildProperties(Rect position, SerializedProperty property, GUIContent label, bool shouldBeInlined, bool[] visibilityEvaluation)
        {
            var y = position.y;
            foreach (var prop in VisibleProperties(property, visibilityEvaluation))
            {
                if (shouldBeInlined)
                {
                    EditorGUI.PropertyField(position, prop, label, true);
                    return;
                }

                var height = EditorGUI.GetPropertyHeight(prop);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, height), prop, true);
                y += height;
            }
        }

        private bool[] EvaluateVisibleProperties(SerializedProperty property, out bool moreThanOne)
        {
            var prop = property.Copy();
            if (!prop.Next(true))
            {
                moreThanOne = false;
                return Array.Empty<bool>();
            }

            var countVisible = 0;
            var visibilityEvaluation = new bool[fieldNames.Count];
            for (var i = 0; i < visibilityEvaluation.Length; i++)
            {
                visibilityEvaluation[i] = true;
            }

            do
            {
                for (var i = 0; i < fieldNames.Count; i++)
                {
                    var showWhenAttribute = showWhenAttributes[i];
                    if (showWhenAttribute != null && showWhenAttribute.propertyName == prop.name)
                    {
                        var isVisible = showWhenAttribute.IsVisible(prop);
                        if (isVisible)
                        {
                            countVisible++;
                        }
                        visibilityEvaluation[i] = isVisible;
                    }
                }
            }
            while (prop.Next(false));

            moreThanOne = countVisible > 1;
            return visibilityEvaluation;
        }
        
        private IEnumerable<SerializedProperty> VisibleProperties(SerializedProperty property, bool[] visibilityEvaluation)
        {
            var prop = property.Copy();
            if (!prop.Next(true))
            {
                yield break;
            }

            do
            {
                for (var i = 0; i < fieldNames.Count; i++)
                {
                    if (fieldNames[i] == prop.name)
                    {
                        if (visibilityEvaluation[i])
                        {
                            yield return prop;
                        }
                        break;
                    }
                }
            }
            while (prop.Next(false));
        }

        private bool CanBeInlined(SerializedProperty rootProperty, bool[] visibilityEvaluation)
        {
            var first = VisibleProperties(rootProperty, visibilityEvaluation).FirstOrDefault();
            if (first is null)
            {
                return false;
            }

            return EditorGUI.GetPropertyHeight(first, true) <= propertyHeight;
        }
    }
}
