using System;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public struct Dependency
    {
        public string value;
        [Tooltip("SoftDependency - mod will be loaded even if dependency is not loaded\nHardDependency - mod will not be loaded if dependency is not loaded")]
        public DependencyType type;
    }
}
