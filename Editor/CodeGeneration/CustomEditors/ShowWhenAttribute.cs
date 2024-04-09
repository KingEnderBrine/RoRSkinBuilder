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
        public readonly object[] values;

        public ShowWhenAttribute(string propertyName, bool inverse, params object[] values)
        {
            this.propertyName = propertyName;
            this.inverse = inverse;
            this.values = values;
        }

        public bool IsVisible(SerializedProperty conditionProperty)
        {
            return inverse ^ values.Any(el => PropertyEquals(conditionProperty, el));
        }

        private bool PropertyEquals(SerializedProperty property, object el)
        {
            if (property == null)
            {
                return false;
            }

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return property.intValue == (int)el;
                case SerializedPropertyType.String:
                    return property.stringValue == (string)el;
                case SerializedPropertyType.Enum:
                    return property.enumValueIndex == (int)el;
                case SerializedPropertyType.Boolean:
                    return property.boolValue == (bool)el;
            }

            return false;
        }
    }
}
