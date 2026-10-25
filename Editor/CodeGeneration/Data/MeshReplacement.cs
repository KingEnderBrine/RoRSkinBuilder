using System;
using RoRSkinBuilder.CustomEditors;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class MeshReplacement : IConfigEnabled
    {
        [Tooltip("Load mesh using Addressables")]
        public bool useAddressablesKey;
        [Tooltip("A new model")]
        [ShowWhen(nameof(useAddressablesKey), false, false)]
        public Mesh mesh;
        [Tooltip("Addressables key of the mesh")]
        [ShowWhen(nameof(useAddressablesKey), false, true)]
        public string meshKey;
        [Tooltip("The old model that needs to be replaced")]
        public ComponentReference rendererReference;
        public string enableFromConfig;

        public string EnableFromConfig => enableFromConfig;
    }
}
