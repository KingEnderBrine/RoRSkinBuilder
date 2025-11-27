using System;
using RoRSkinBuilder.CustomEditors;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class GameObjectActivation
    {
        public GameObjectActivationAccessType accessType = GameObjectActivationAccessType.ByPath;
        [ShowWhen(nameof(accessType), false, GameObjectActivationAccessType.ByRendererName)]
        public string rendererName;
        [ShowWhen(nameof(accessType), false, GameObjectActivationAccessType.ByPath)]
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
        public GameObject prefab;
        [Tooltip("Addressables key of the prefab")]
        [ShowWhen(nameof(useAddressablesKey), false, true)]
        [ShowWhen(nameof(spawnPrefabOnModelObject), false, true)]
        public string prefabKey;

        [HideInInspector]
        public bool isExpanded;
    }
}
