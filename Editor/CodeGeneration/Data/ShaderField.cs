using RoRSkinBuilder.CustomEditors;
using System;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public sealed class ShaderField
    {
        public enum PropertyType { Color, Vector, Float, Range, Texture, Toggle, Enum }
        
        [HideInInspector]
        public string propertyDescription;
        [HideInInspector]
        public string propertyName;
        [ShowWhen(nameof(isCustom), false, true)]
        public PropertyType propertyType;

        [ShowWhen(nameof(propertyType), false, PropertyType.Texture)]
        public Texture textureValue;
        [ShowWhen(nameof(propertyType), false, PropertyType.Texture)]
        public Vector2 offset;
        [ShowWhen(nameof(propertyType), false, PropertyType.Texture)]
        public Vector2 tiling;
        [ShowWhen(nameof(propertyType), false, PropertyType.Vector)]
        public Vector4 vectorValue;
        [ShowWhen(nameof(propertyType), false, PropertyType.Color)]
        public Color colorValue;
        [ShowWhen(nameof(propertyType), false, PropertyType.Float, PropertyType.Range, PropertyType.Toggle, PropertyType.Enum)]
        public float floatValue;
        [HideInInspector]
        public string keyword;

        [HideInInspector]
        public bool hidden;
        [HideInInspector]
        public bool isCustom;

        [HideInInspector]
        public bool isExpanded;
    }
}
