using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.CustomEditors
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public abstract class CompositePropertyAttribute : PropertyAttribute
    {
        public List<CompositePropertyAttribute> attributes;

        public void Collect(FieldInfo fieldInfo)
        {
            attributes ??= new List<CompositePropertyAttribute>();
            if (attributes.Count == 0)
            {
                attributes.AddRange(fieldInfo.GetCustomAttributes(true).OfType<CompositePropertyAttribute>());
            }
        }

        public virtual bool ModifyLabel(ref GUIContent label)
        {
            return false;
        }

        public virtual bool OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            return false;
        }

        public virtual bool GetPropertyHeight(SerializedProperty property, GUIContent label, ref float height)
        {
            return false;
        }

        public virtual bool OnPreGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            return false;
        }

        public virtual bool OnPostGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            return false;
        }
    }
}
