using System;
using RoRSkinBuilder.CustomEditors;
using UnityEngine;
using UnityEngine.Rendering;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class RendererInfo : IConfigEnabled
    {
        [Tooltip("Load material using Addressables")]
        public bool useAddressablesKey;
        [Tooltip("Material to use")]
        [ShowWhen(nameof(useAddressablesKey), false, false)]
        public Material defaultMaterial;
        [Tooltip("Addressables key of the material")]
        [ShowWhen(nameof(useAddressablesKey), false, true)]
        public string materialKey;
        public ShadowCastingMode defaultShadowCastingMode;
        [Tooltip("Is model will be ignoring overlays e.g. shield outline from 'Personal Shield Generator'")]
        public bool ignoreOverlays;
        [Tooltip("A model for which these settings will be applied")]
        public ComponentReference rendererReference;
        public bool hideOnDeath;
        public bool ignoresMaterialOverrides;
        public string enableFromConfig;

        public string EnableFromConfig => enableFromConfig;
    }
}
