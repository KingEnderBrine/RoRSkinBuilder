using RoRSkinBuilder.CustomEditors;
using System;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class ProjectileGhostReplacement
    {
        [Tooltip("Path to the projectile GameObject")]
        public string projectilePath;
        [Tooltip("Load projectile ghost that exists in the game files")]
        public bool useResourcesPath;
        [Tooltip("New projectile ghost")]
        [ShowWhen(nameof(useResourcesPath), false, false)]
        public GameObject projectileGhost;
        [Tooltip("Path to the projectile ghost")]
        [ShowWhen(nameof(useResourcesPath), false, true)]
        public string projectileGhostPath;

        [HideInInspector]
        public bool isExpanded;
    }
}
