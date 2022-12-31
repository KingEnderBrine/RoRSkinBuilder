using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace RoR2
{
    public class CharacterModel
    {
        [Serializable]
        public struct RendererInfo
        {
            public Renderer renderer;
            public Material defaultMaterial;
            public ShadowCastingMode defaultShadowCastingMode;
            public bool ignoreOverlays;
            public bool hideOnDeath;
        }
    }
}