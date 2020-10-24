using System;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class MeshReplacement
    {
        [Tooltip("A new model")]
        public Mesh mesh;
        [Tooltip("The old model that needs to be replaced")]
        public Reference rendererReference;
    }
}
