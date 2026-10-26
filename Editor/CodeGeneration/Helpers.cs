using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder
{
    public static class Helpers
    {
        public static SerializedProperty GetParent(SerializedProperty property)
        {
            var dotIndex = property.propertyPath.LastIndexOf('.');
            SerializedProperty parent;
            if (dotIndex != -1)
            {
                var parentPath = property.propertyPath.Substring(0, dotIndex);
                parent = property.serializedObject.FindProperty(parentPath);
            }
            else
            {
                parent = property.serializedObject.GetIterator();
            }

            return parent;
        }

        public static string BuildPath(GameObject gameObject)
        {
            return BuildPath(gameObject.transform);
        }

        public static string BuildPath(Transform transform)
        {
            var sb = new StringBuilder();
            BuildPath(transform, sb);
            return sb.ToString();
        }

        private static void BuildPath(Transform current, StringBuilder sb)
        {
            var parent = current.parent;
            if (!parent)
            {
                return;
            }

            BuildPath(parent, sb);
            if (sb.Length > 0)
            {
                sb.Append("/");
            }
            sb.Append(current.name);
        }

    }
}
