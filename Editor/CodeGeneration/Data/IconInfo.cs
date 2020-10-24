using RoRSkinBuilder.CustomEditors;
using System;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class IconInfo
    {
        public bool createFromColors;
        [ShowWhen(nameof(createFromColors), false, false)]
        public Sprite sprite;
        [ShowWhen(nameof(createFromColors), false, true)]
        public IconColors colors;

        [HideInInspector]
        public bool isExpanded;
    }
}
