using System;
using RoRSkinBuilder.CustomEditors;
using UnityEngine;
using UnityEngine.Rendering;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class RendererInfo
    {
        public bool useMaterialReplacement;
        [ShowWhen(nameof(useMaterialReplacement), false, false)]
        [Tooltip("Material to use")]
        public Material defaultMaterial;
        [ShowWhen(nameof(useMaterialReplacement), false, true)]
        public MaterialReplacement materialReplacement;
        public ShadowCastingMode defaultShadowCastingMode;
        [Tooltip("Is model will be ignoring overlays e.g. shield outline from 'Personal Shield Generator'")]
        public bool ignoreOverlays;
        [Tooltip("A model for which these settings will be applied")]
        public Reference rendererReference;

        [HideInInspector]
        public bool isExpanded;
    }
}
