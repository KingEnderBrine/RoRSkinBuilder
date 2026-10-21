using System;
using System.Collections.Generic;
using RoRSkinBuilder.CustomEditors;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class GameObjectActivation : IConfigEnabled
    {
        public string enableFromConfig;
        public GameObjectActivationAccessType accessType = GameObjectActivationAccessType.ByPath;
        [ShowWhen(nameof(accessType), false, GameObjectActivationAccessType.ByRendererName)]
        public string rendererName;
        [ShowWhen(nameof(accessType), false, GameObjectActivationAccessType.ByPath)]
        [PathReference]
        public string path;
        public bool spawnPrefabOnModelObject;
        [ShowWhen(nameof(spawnPrefabOnModelObject), false, false)]
        public bool shouldActivate;
        [ShowWhen(nameof(spawnPrefabOnModelObject), false, true)]
        public Vector3 localPosition;
        [ShowWhen(nameof(spawnPrefabOnModelObject), false, true)]
        public Vector3 localRotation;
        [ShowWhen(nameof(spawnPrefabOnModelObject), false, true)]
        public Vector3 localScale = Vector3.one;

        [Tooltip("Child object prefab")]
        [ShowWhen(nameof(spawnPrefabOnModelObject), false, true)]
        public bool useAddressablesKey;
        [Tooltip("Child object prefab")]
        [ShowWhen(nameof(spawnPrefabOnModelObject), false, true)]
        [ShowWhen(nameof(useAddressablesKey), false, false)]
        [OnChanged(typeof(GameObjectActivation), nameof(OnPrefabChanged))]
        public GameObject prefab;
        [Tooltip("Addressables key of the prefab")]
        [ShowWhen(nameof(useAddressablesKey), false, true)]
        [ShowWhen(nameof(spawnPrefabOnModelObject), false, true)]
        public string prefabKey;
        [Tooltip("All game objects in the prefab will be added as bones to renderers that have parent object as a bone")]
        [ShowWhen(nameof(spawnPrefabOnModelObject), false, true)]
        public bool addBonesToRenderers;
        [ShowWhen(nameof(spawnPrefabOnModelObject), false, true)]
        public PathListWrapper dynamicBonesColliderPaths;

        public string EnableFromConfig => enableFromConfig;

        private static void OnPrefabChanged(SerializedProperty property)
        {
            var prefab = property.objectReferenceValue as GameObject;
            if (prefab)
            {
                var self = Helpers.GetParent(property);
                self.FindPropertyRelative(nameof(localPosition)).vector3Value = prefab.transform.localPosition;
                self.FindPropertyRelative(nameof(localRotation)).vector3Value = prefab.transform.localRotation.eulerAngles;
                self.FindPropertyRelative(nameof(localScale)).vector3Value = prefab.transform.localScale;
            }
        }
    }
}
