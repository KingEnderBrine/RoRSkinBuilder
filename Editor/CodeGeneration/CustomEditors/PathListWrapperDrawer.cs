using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.CodeGeneration.CustomEditors
{
    [CustomPropertyDrawer(typeof(PathListWrapper))]
    public class PathListWrapperDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var list = property.FindPropertyRelative(nameof(PathListWrapper.list));
            return EditorGUI.GetPropertyHeight(list);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var list = property.FindPropertyRelative(nameof(PathListWrapper.list));
            EditorGUI.PropertyField(position, list, label);
        }
    }
}
