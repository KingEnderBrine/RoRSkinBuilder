using RoRSkinBuilder.CustomEditors;
using System;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class ComponentReference
    {
        public ComponentAccessType accessType;
        [ShowWhen(nameof(accessType), false, ComponentAccessType.ByName)]
        public string name;
        [ShowWhen(nameof(accessType), false, ComponentAccessType.ByPath)]
        public string path;

        [HideInInspector]
        public bool isExpanded;
    }
}
