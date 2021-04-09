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
        private const int propertyHeigh = 16;
        private static readonly Dictionary<Type, DrawerInfo> drawerInfos = new Dictionary<Type, DrawerInfo>();

        protected bool canBeHidden = true;
        protected bool indent = true;
        protected bool showLabel = true;

        private readonly DrawerInfo drawerInfo;
        public HideablePropertyDrawer()
        {
            if (drawerInfos.TryGetValue(typeof(T), out drawerInfo))
            {
                return;
            }
            drawerInfo = new DrawerInfo();
            
            foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if ((field.IsPrivate && field.GetCustomAttribute<SerializeField>() != null) || (field.IsPublic && field.GetCustomAttribute<HideInInspector>() == null))
                {
                    drawerInfo.childProperties[field.Name] = field.GetCustomAttribute<ShowWhenAttribute>();
                }
            }

            drawerInfos[typeof(T)] = drawerInfo;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var isExpandedProperty = property.FindPropertyRelative("isExpanded");

            return isExpandedProperty.boolValue ? drawerInfo.GetVisiblePropertiesHeight(property) - (showLabel ? 0 : propertyHeigh) : propertyHeigh;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var isExpandedProperty = property.FindPropertyRelative("isExpanded");
        
            EditorGUI.BeginProperty(position, label, property);
            property.isExpanded = false;
            if (canBeHidden)
            {
                isExpandedProperty.boolValue = !showLabel || EditorGUI.Foldout(new Rect(position.x, position.y, position.width, propertyHeigh), isExpandedProperty.boolValue, label, true);
            }
            else if (showLabel)
            {
                EditorGUI.LabelField(new Rect(position.x, position.y, position.width, propertyHeigh), label);
                isExpandedProperty.boolValue = true;
            }

            if (isExpandedProperty.boolValue)
            {
                var y = position.y;
                if (showLabel)
                {
                    y += propertyHeigh;
                }
                if (indent)
                {
                    EditorGUI.indentLevel++;
                }
                foreach (var childProperty in drawerInfo.GetVisibleProperties(property))
                {
                    var height = EditorGUI.GetPropertyHeight(childProperty);
                    EditorGUI.PropertyField(new Rect(position.x, y, position.width, height), childProperty, true);
                    y += height;
                }
                if (indent)
                {
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUI.EndProperty();
        }

        private class DrawerInfo
        {
            public readonly Dictionary<string, ShowWhenAttribute> childProperties = new Dictionary<string, ShowWhenAttribute>();

            public float GetVisiblePropertiesHeight(SerializedProperty rootProperty)
            {
                return childProperties.Where(el => el.Value?.IsVisible(rootProperty) ?? true).Sum(el => EditorGUI.GetPropertyHeight(rootProperty.FindPropertyRelative(el.Key), true)) + propertyHeigh;
            }

            public IEnumerable<SerializedProperty> GetVisibleProperties(SerializedProperty rootProperty)
            {
                return childProperties.Where(el => el.Value?.IsVisible(rootProperty) ?? true).Select(el => rootProperty.FindPropertyRelative(el.Key));
            }
        }
    }
}
