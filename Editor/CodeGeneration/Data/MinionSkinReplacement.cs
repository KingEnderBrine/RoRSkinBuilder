using RoRSkinBuilder.CustomEditors;
using System;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class MinionSkinReplacement
    {
        [Tooltip("Minion body name")]
        public string bodyName;
        [Tooltip("Use skin that is added by the game/other mods")]
        public bool findSkinByReference;
        [ShowWhen(nameof(findSkinByReference), false, true)]
        public Reference reference;
        [ShowWhen(nameof(findSkinByReference), false, false)]
        public SkinDefinition skin;

        [HideInInspector]
        public bool isExpanded;
    }
}
