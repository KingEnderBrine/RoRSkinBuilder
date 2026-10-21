using System;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.CustomEditors
{
    public class ShowWhenAttribute : CompositePropertyAttribute
    {
        public readonly string propertyName;
        public readonly bool inverse;
        private readonly ValueUnion[] values;

        public ShowWhenAttribute(string propertyName, bool inverse, params object[] values)
        {
            this.propertyName = propertyName;
            this.inverse = inverse;
            this.values = values.Select(el => new ValueUnion(el)).ToArray();
        }

        public ShowWhenAttribute(bool show)
        {
            inverse = !show;
        }

        public override bool GetPropertyHeight(SerializedProperty property, GUIContent label, ref float height)
        {
            var parent = Helpers.GetParent(property);
            if (IsVisible(parent))
            {
                return false;
            }

            //default spacing
            height = -2;
            return true;
        }

        public override bool OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var parent = Helpers.GetParent(property);
            return !IsVisible(parent);
        }

        public bool IsVisible(SerializedProperty rootProperty)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return !inverse;
            }

            SerializedProperty conditionProperty;
            if (rootProperty.depth == -1)
            {
                conditionProperty = rootProperty.serializedObject.FindProperty(propertyName);
            }
            else
            {
                conditionProperty = rootProperty.FindPropertyRelative(propertyName);
            }
            
            return inverse ^ values.Any(el => el.Equals(conditionProperty));
        }

        public struct ValueUnion
        {
            public SerializedPropertyType type;
            public int intValue;
            public string stringValue;
            public bool boolValue;

            public ValueUnion(object obj)
            {
                type = SerializedPropertyType.Generic;
                stringValue = null;
                boolValue = false;
                intValue = 0;

                if (obj is string strObj)
                {
                    type = SerializedPropertyType.String;
                    stringValue = strObj;
                }
                else if (obj is bool boolObj)
                {
                    type = SerializedPropertyType.Boolean;
                    boolValue = boolObj;
                }
                else if (obj is int intObj)
                {
                    type = SerializedPropertyType.Integer;
                    intValue = intObj;
                }
                else if (obj is Enum)
                {
                    type = SerializedPropertyType.Enum;
                    intValue = (int)obj;
                }
            }

            public readonly bool Equals(SerializedProperty property)
            {
                if (type != property.propertyType)
                {
                    return false;
                }

                switch (property.propertyType)
                {
                    case SerializedPropertyType.Integer:
                        return property.intValue == intValue;
                    case SerializedPropertyType.String:
                        return property.stringValue == stringValue;
                    case SerializedPropertyType.Enum:
                        return property.intValue == intValue;
                    case SerializedPropertyType.Boolean:
                        return property.boolValue == boolValue;
                }

                return false;
            }
        }
    }
}
