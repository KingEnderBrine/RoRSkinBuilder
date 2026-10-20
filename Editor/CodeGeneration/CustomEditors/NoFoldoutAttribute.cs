using System;
using System.Collections.Generic;
using System.Text;
using RoRSkinBuilder.CustomEditors;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.CodeGeneration.CustomEditors
{
    public class NoFoldoutAttribute : CompositePropertyAttribute
    {
        public override bool GetPropertyHeight(SerializedProperty property, GUIContent label, ref float height)
        {
            height = 0;
            var copy = property.Copy();
            var depth = copy.depth;
            if (copy.NextVisible(true))
            {
                do
                {
                    height += EditorGUI.GetPropertyHeight(copy);
                }
                while (copy.NextVisible(false) && depth != copy.depth);
            }

            return true;
        }

        public override bool OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var copy = property.Copy();
            var depth = copy.depth;
            if (copy.NextVisible(true))
            {
                do
                {
                    position.height = EditorGUI.GetPropertyHeight(copy);
                    EditorGUI.PropertyField(position, copy, null);
                    position.y += position.height;
                }
                while (copy.NextVisible(false) && depth != copy.depth);
            }

            return true;
        }
    }
}
