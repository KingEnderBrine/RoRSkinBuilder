using RoRSkinBuilder.CustomEditors;
using System;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class MinionSkinReplacement
    {
        public string bodyName;
        public bool findSkinByReference;
        [ShowWhen(nameof(findSkinByReference), false, true)]
        public Reference reference;
        [ShowWhen(nameof(findSkinByReference), false, false)]
        public SkinDefinition skin;

        [HideInInspector]
        public bool isExpanded;
    }
}
