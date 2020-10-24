using RoRSkinBuilder.CustomEditors;
using System;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class Reference
    {
        public AccessType accessType;
        [ShowWhen(nameof(accessType), false, AccessType.ByName)]
        public string name;
        [ShowWhen(nameof(accessType), false, AccessType.ByIndex)]
        public int index;

        [HideInInspector]
        public bool isExpanded;
    }
}
