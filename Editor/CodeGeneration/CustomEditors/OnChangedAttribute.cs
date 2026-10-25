using System;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.CustomEditors
{
    public class OnChangedAttribute : CompositePropertyAttribute
    {
        private readonly Action<SerializedProperty> action;
        public OnChangedAttribute(Type type, string methodName)
        {
            action = (Action<SerializedProperty>)Delegate.CreateDelegate(typeof(Action<SerializedProperty>), type, methodName);
        }

        public override bool OnPreGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            return false;
        }

        public override bool OnPostGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (EditorGUI.EndChangeCheck())
            {
                action(property);
            }
            return false;
        }
    }
}