using System.IO;
using System.Text;
using RoRSkinBuilder.Data;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder
{
    public class StaticCodeBuilder
    {
        private enum Parts
        {
            Static = 1 << 0,
            DynamicSkins = 1 << 1,
        }

        private const string staticContentVersion = "1";

        public static void TryUpdate(AssetsInfo assetInfo)
        {
            var path = Path.Combine(assetInfo.modFolder, "StaticCode.cs");
            var parts = Parts.Static;
            foreach (var skin in assetInfo.skinModInfo.skins)
            {
                foreach (var gameObjectActivation in skin.gameObjectActivations)
                {
                    if (gameObjectActivation.spawnPrefabOnModelObject && (gameObjectActivation.addBonesToRenderers || gameObjectActivation.dynamicBonesColliderPaths.Count > 0))
                    {
                        parts |= Parts.DynamicSkins;
                        goto Out;
                    }
                }
            }
        Out:

            var userData = $"{staticContentVersion}|{(int)parts}";
            var importer = AssetImporter.GetAtPath(path);
            if (importer && importer.userData == userData)
            {
                return;
            }

            var sb = new StringBuilder();
            sb.Append(CodePre).Append(assetInfo.uccModName).AppendLine()
                .AppendLine("{");

            if ((parts | Parts.Static) > 0)
            {
                sb.Append(CodeStatic);
            }

            if ((parts | Parts.DynamicSkins) > 0)
            {
                sb.Append(CodeDynamicSkins);
            }

            sb.AppendLine().Append("}");
            File.WriteAllText(path, sb.ToString());
            AssetDatabase.ImportAsset(path);

            importer = AssetImporter.GetAtPath(path);
            importer.userData = userData;
            importer.SaveAndReimport();
        }

        private static readonly string CodePre = @"using System;
using System.Collections.Generic;
using RoR2;
using UnityEngine;

namespace ";

        private static readonly string CodeStatic = @"
    public static class Helpers
    {
        public static Transform GetChild(this Transform transform, int index, out Transform child)
        {
            return child = transform.GetChild(index);
        }

        public static GameObject AddComponent<T>(this GameObject gameObject, Action<T> modifier) where T : MonoBehaviour
        {
            modifier(gameObject.AddComponent<T>());
            return gameObject;
        }

        public static T ThrowIfEquals<T>(int index, string message, T value, T expected) where T : Enum
        {
            if (value.Equals(expected))
            {
                throw new ElementException(index, message);
            }

            return value;
        }

        public static T ThrowIfOutOfBounds<T>(int index, string message, T[] array, int elementIndex) where T : class
        {
            if (array is null || array.Length <= elementIndex)
            {
                throw new ElementException(index, message);
            }

            return array[elementIndex];
        }

        public static T ThrowIfNull<T>(int index, string message, T value) where T : class
        {
            if (value is null)
            {
                throw new ElementException(index, message);
            }

            return value;
        }

        public static void TryCatchThrow(string message, Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                throw new FieldException(message, e);
            }
        }

        public static void TryAddComponent<T>(GameObject obj) where T : Component
        {
            if (obj && !obj.GetComponent<T>())
            {
                obj.AddComponent<T>();
            }
        }
    }

    public class FieldException : Exception
    {
        public FieldException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ElementException : Exception
    {
        public int Index { get; }
        public ElementException(int index, string message) : base(message)
        {
            Index = index;
        }
    }";

        private static readonly string CodeDynamicSkins = @"
    public class InsertBones : MonoBehaviour
    {
        private readonly List<SkinnedMeshRenderer> modifiedRenderers = new List<SkinnedMeshRenderer>();
        public List<string> rendererPaths = new List<string>();
        public List<Transform> newBones = new List<Transform>();
        public string afterPath;

        public void Awake()
        {
            var mdl = GetComponentInParent<ModelSkinController>().transform;
            var afterTransform = mdl.transform.Find(afterPath);
            foreach (var rendererPath in rendererPaths)
            {
                var renderer = mdl.Find(rendererPath)?.GetComponent<SkinnedMeshRenderer>();
                if (renderer)
                {
                    var bones = renderer.bones;
                    var index = Array.IndexOf(bones, afterTransform);
                    if (index != -1)
                    {
                        Array.Resize(ref bones, bones.Length + newBones.Count);
                        newBones.CopyTo(bones, index);
                        renderer.bones = bones;
                        modifiedRenderers.Add(renderer);
                    }
                }
            }
        }

        public void OnDestroy()
        {
            foreach (var renderer in modifiedRenderers)
            {
                if (renderer)
                {
                    var bones = renderer.bones;
                    var index = Array.IndexOf(bones, transform);
                    Array.Copy(bones, index + newBones.Count, bones, index, bones.Length - index - newBones.Count);
                    Array.Resize(ref bones, bones.Length - newBones.Count);
                    renderer.bones = bones;
                }
            }
            modifiedRenderers.Clear();
        }
    }

    public class AddDynamicColliders : MonoBehaviour
    {
        public List<string> paths = new List<string>();

        public void Awake()
        {
            var mdl = GetComponentInParent<ModelSkinController>().transform;
            var allColliders = new List<DynamicBoneCollider>();
            foreach (var path in paths)
            {
                var obj = mdl.Find(path);
                if (obj)
                {
                    allColliders.AddRange(obj.GetComponents<DynamicBoneCollider>());
                }
            }

            foreach (var dynamicSkin in GetComponents<DynamicBone>())
            {
                dynamicSkin.m_Colliders.AddRange(allColliders);
            }
        }
    }";
    }
}