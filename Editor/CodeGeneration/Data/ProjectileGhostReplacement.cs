using System;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class ProjectileGhostReplacement
    {
        [Tooltip("Path to the projectile GameObject")]
        public string projectilePath;
        [Tooltip("New projectile ghost")]
        public GameObject projectileGhost;
    }
}
