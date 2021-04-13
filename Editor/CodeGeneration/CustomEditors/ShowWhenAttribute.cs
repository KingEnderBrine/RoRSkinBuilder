using System;
using System.Linq;
using UnityEditor;

namespace RoRSkinBuilder.CustomEditors
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ShowWhenAttribute : Attribute
    {
        public readonly string propertyName;
        public readonly bool inverse;
        public readonly string[] values;

        public ShowWhenAttribute(string propertyName, bool inverse, params object[] values)
        {
            this.propertyName = propertyName;
            this.inverse = inverse;
            this.values = values.Select(el => el.ToString()).ToArray();
        }

        public ShowWhenAttribute(bool show)
        {
            inverse = !show;
        }

        public bool IsVisible(SerializedProperty rootProperty)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return !inverse;
            }
            var conditionProperty = rootProperty.FindPropertyRelative(propertyName);
            
            return inverse ^ values.Any(el => el == PropertyAsString(conditionProperty));
        }

        private string PropertyAsString(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return property.intValue.ToString();
                case SerializedPropertyType.String:
                    return property.stringValue;
                case SerializedPropertyType.Enum:
                    return property.enumNames[property.enumValueIndex];
                case SerializedPropertyType.Boolean:
                    return property.boolValue.ToString();
            }

            return null;
        }
    }
}
