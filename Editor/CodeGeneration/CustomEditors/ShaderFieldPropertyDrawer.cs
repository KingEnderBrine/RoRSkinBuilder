using RoRSkinBuilder.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.CustomEditors
{
    [CustomPropertyDrawer(typeof(ShaderField))]
    public class ShaderFieldPropertyDrawer : HideablePropertyDrawer<ShaderField>
    {
        protected override bool CanBeHidden => false;
        protected override bool InlineIfOnlyOneProperty => true;

        private GUIContent[] options;
        private float[] values;
        private string keyword;

        protected override void DrawChildProperties(Rect position, SerializedProperty property, GUIContent label, bool shouldBeInlined, bool[] visibilityEvaluation)
        {
            var shader = property.serializedObject.FindProperty("shader").objectReferenceValue as Shader;
            var type = (ShaderField.PropertyType)property.FindPropertyRelative("propertyType").enumValueIndex;

            switch (type)
            {
                case ShaderField.PropertyType.Vector:
                    DrawVectorProperty(shader, position, property, label);
                    break;
                case ShaderField.PropertyType.Toggle:
                    DrawToggleProperty(shader, position, property, label);
                    break;
                case ShaderField.PropertyType.Enum:
                    DrawEnumProperty(shader, position, property, label);
                    break;
                case ShaderField.PropertyType.Range:
                    DrawRangeProperty(shader, position, property, label);
                    break;
                default:
                    base.DrawChildProperties(position, property, label, shouldBeInlined, visibilityEvaluation);
                    break;
            }
        }

        private void DrawVectorProperty(Shader shader, Rect position, SerializedProperty property, GUIContent label)
        {
            var valueProperty = property.FindPropertyRelative("vectorValue");

            EditorGUI.BeginChangeCheck();
            var value = EditorGUI.Vector4Field(new Rect(position), label, valueProperty.vector4Value);
            if (!EditorGUI.EndChangeCheck())
            {
                return;
            }
            valueProperty.vector4Value = value;
        }

        private void DrawToggleProperty(Shader shader, Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyName = property.FindPropertyRelative("propertyName").stringValue;
            var floatValueProperty = property.FindPropertyRelative("floatValue");
            if (keyword == null)
            {
                keyword = FindAttributeArgs(shader, propertyName, "Toggle");
                return;
            }

            EditorGUI.BeginChangeCheck();
            var on = EditorGUI.Toggle(new Rect(position), label, floatValueProperty.floatValue != 0);
            if (!EditorGUI.EndChangeCheck())
            {
                return;
            }

            floatValueProperty.floatValue = on ? 1 : 0;
            property.FindPropertyRelative("keyword").stringValue = keyword;
        }

        private void DrawEnumProperty(Shader shader, Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyName = property.FindPropertyRelative("propertyName").stringValue;
            var valueProperty = property.FindPropertyRelative("floatValue");
            if (options == null)
            {
                var attr = FindAttributeArgs(shader, propertyName, "MaterialEnum");
                var split = attr.Split(',');
                var length = split.Length / 2;
                options = new GUIContent[length];
                values = new float[length];

                for (var i = 0; i < length; i++)
                {
                    options[i] = new GUIContent(split[i * 2]);
                    values[i] = float.Parse(split[i * 2 + 1], NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
                }
            }

            var index = Array.IndexOf(values, valueProperty.floatValue);
            EditorGUI.BeginChangeCheck();
            index = EditorGUI.Popup(new Rect(position), label, index, options);
            if (!EditorGUI.EndChangeCheck())
            {
                return;
            }
            valueProperty.floatValue = values[index];
        }

        private void DrawRangeProperty(Shader shader, Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyName = property.FindPropertyRelative("propertyName").stringValue;
            var valueProperty = property.FindPropertyRelative("floatValue");
            var index = shader.FindPropertyIndex(propertyName);
            var range = shader.GetPropertyRangeLimits(index);

            EditorGUI.BeginChangeCheck();
            var value = EditorGUI.Slider(new Rect(position), label, valueProperty.floatValue, range.x, range.y);
            if (!EditorGUI.EndChangeCheck())
            {
                return;
            }
            valueProperty.floatValue = value;
        }

        private string FindAttributeArgs(Shader shader, string name, string attributeName)
        {
            var index = shader.FindPropertyIndex(name);
            if (index == -1)
            {
                return null;
            }

            var propertyAttributes = shader.GetPropertyAttributes(index);
            if (propertyAttributes == null || propertyAttributes.Length == 0)
            {
                return null;
            }

            foreach (var attribute in propertyAttributes)
            {
                var attrName = attribute;
                var argsText = string.Empty;
                Match match = Regex.Match(attribute, "(\\w+)\\s*\\((.*)\\)");
                if (match.Success)
                {
                    attrName = match.Groups[1].Value;
                    argsText = match.Groups[2].Value.Trim();
                }

                if (attrName == attributeName)
                {
                    return argsText;
                }
            }

            return null;
        }
    }
}
