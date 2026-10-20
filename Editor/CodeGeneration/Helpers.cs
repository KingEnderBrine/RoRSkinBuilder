using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RoRSkinBuilder
{
    public static class Helpers
    {
        public static string BuildGetPath(Transform childTransform, Transform parentTransform)
        {
            var path = new List<int>();
            if (!BuildGetPath(childTransform, parentTransform, path))
            {
                return null;
            }

            var sb = new StringBuilder();
            sb.Append("t");
            for (var i = path.Count - 1; i > 0; i--)
            {
                sb.Append("_").Append(path[i]);
            }

            sb.Append(".GetChild(").Append(path[0]).Append(", out var t");
            for (var i = path.Count - 1; i >= 0; i--)
            {
                sb.Append("_").Append(path[i]);
            }
            sb.Append(")");

            return sb.ToString();
        }

        private static bool BuildGetPath(Transform childTransform, Transform parentTransform, List<int> path)
        {
            if (!childTransform)
            {
                return false;
            }

            if (childTransform == parentTransform)
            {
                return true;
            }

            if (!BuildGetPath(childTransform.parent, parentTransform, path))
            {
                return false;
            }

            path.Add(childTransform.GetSiblingIndex());
            return true;
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
