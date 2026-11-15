using System;
using System.Linq;
using System.Text;
using RoRSkinBuilder.Data;
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

        public static InsertBonesInfo GetInsertBonesInfo(ISkinParams skin, GameObjectActivation activation)
        {
            var info = new InsertBonesInfo();
            var prefabTransform = activation.prefab.transform;
            var parentTransform = skin.ReferenceModel.transform.Find(activation.path);
            if (!parentTransform)
            {
                info.error = $"Reference Model doesn't contain an object at path \"{activation.path}\"";
                return info;
            }

            var referenceTransform = parentTransform.Find(activation.prefab.name);
            if (!referenceTransform)
            {
                info.error = $"Reference Model doesn't contain an object at path \"{activation.path}/{activation.prefab.name}\"";
                return info;
            }

            var renderers = skin.ReferenceModel.GetComponentsInChildren<SkinnedMeshRenderer>();
            var rootBone = new BoneInfo();

            GatherBonesInfo(rootBone, referenceTransform, prefabTransform, info, renderers);
            if (PruneBonesInfo(rootBone))
            {
                return null;
            }

            info.bone = rootBone.children[0];
            return info;
        }

        private static bool PruneBonesInfo(BoneInfo bone)
        {
            for (var i = bone.children.Count - 1; i >= 0; i--)
            {
                if (PruneBonesInfo(bone.children[i]))
                {
                    bone.children.RemoveAt(i);
                }
            }

            return !bone.inRenderer && bone.children.Count == 0;
        }

        private static void GatherBonesInfo(BoneInfo parent, Transform referenceTransform, Transform prefabTransform, InsertBonesInfo info, SkinnedMeshRenderer[] renderers)
        {
            if (!info.renderer)
            {
                foreach (var renderer in renderers)
                {
                    var index = Array.IndexOf(renderer.bones, referenceTransform);
                    if (index != -1)
                    {
                        info.renderer = renderer;
                        info.afterPath = Helpers.BuildPath(renderer.bones[index - 1]);
                        info.startIndex = index;

                        var endIndex = index;
                        for (; endIndex < renderer.bones.Length; endIndex++)
                        {
                            var current = renderer.bones[endIndex];
                            while (current)
                            {
                                if (current == referenceTransform)
                                {
                                    break;
                                }
                                current = current.parent;
                            }

                            if (!current)
                            {
                                break;
                            }
                        }

                        info.count = endIndex - index;
                        break;
                    }
                }
            }

            var bone = new BoneInfo
            {
                indexInParent = prefabTransform.GetSiblingIndex(),
                inRenderer = info.renderer ? Array.IndexOf(info.renderer.bones, referenceTransform, info.startIndex, info.count) != -1 : false,
            };

            parent.children.Add(bone);

            var count = referenceTransform.childCount;
            var indexOrder = Enumerable.Range(0, count).OrderBy(x => Array.IndexOf(info.renderer.bones, referenceTransform.GetChild(x)));

            foreach (var index in indexOrder)
            {
                var nextBone = referenceTransform.GetChild(index);
                GatherBonesInfo(bone, nextBone, prefabTransform.Find(nextBone.name), info, renderers);
            }
        }
    }
}
