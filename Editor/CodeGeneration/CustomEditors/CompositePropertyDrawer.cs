using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoRSkinBuilder.CustomEditors
{
    [CustomPropertyDrawer(typeof(CompositePropertyAttribute), true)]
    public class CompositePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var composite = attribute as CompositePropertyAttribute;
            composite.Collect(fieldInfo);

            var height = EditorGUI.GetPropertyHeight(property, label);
            foreach (var attr in composite.attributes)
            {
                if (attr.GetPropertyHeight(property, label, ref height))
                {
                    break;
                }
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var composite = attribute as CompositePropertyAttribute;
            composite.Collect(fieldInfo);

            foreach (var attr in composite.attributes)
            {
                if (attr.ModifyLabel(ref label))
                {
                    break;
                }
            }
            
            foreach (var attr in composite.attributes)
            {
                if (attr.OnPreGUI(position, property, label))
                {
                    break;
                }
            }

            var guiHandled = false;
            foreach (var attr in composite.attributes)
            {
                if (attr.OnGUI(position, property, label))
                {
                    guiHandled = true;
                    break;
                }
            }

            if (!guiHandled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            foreach (var attr in composite.attributes)
            {
                if (attr.OnPostGUI(position, property, label))
                {
                    break;
                }
            }
        }
    }
}
