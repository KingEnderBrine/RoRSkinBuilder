using RoRSkinBuilder.CustomEditors;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class ProjectileGhostReplacement
    {
        [Tooltip("Path to the projectile GameObject")]
        public string projectilePath;
        [Tooltip("Load projectile ghost that exists in the game files")]
        [FormerlySerializedAs("useResourcesPath")]
        public bool useAddressablesPath;
        [Tooltip("New projectile ghost")]
        [ShowWhen(nameof(useAddressablesPath), false, false)]
        public GameObject projectileGhost;
        [Tooltip("Path to the projectile ghost")]
        [ShowWhen(nameof(useAddressablesPath), false, true)]
        public string projectileGhostPath;

        [HideInInspector]
        public bool isExpanded;
    }
}
