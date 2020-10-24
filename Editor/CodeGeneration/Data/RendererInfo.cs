using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class RendererInfo
    {
        public Material defaultMaterial;
        public ShadowCastingMode defaultShadowCastingMode;
        public bool ignoreOverlays;
        public Reference rendererReference;
    }
}
