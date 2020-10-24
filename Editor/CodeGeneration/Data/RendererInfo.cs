using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class RendererInfo
    {
        [Tooltip("Material to use")]
        public Material defaultMaterial;
        public ShadowCastingMode defaultShadowCastingMode;
        [Tooltip("Is model will be ignoring overlays e.g. shield outline from 'Personal Shield Generator'")]
        public bool ignoreOverlays;
        [Tooltip("A model for which these settings will be applied")]
        public Reference rendererReference;
    }
}
