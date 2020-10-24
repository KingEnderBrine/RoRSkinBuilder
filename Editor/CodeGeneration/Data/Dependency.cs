using System;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public struct Dependency
    {
        public string value;
        public DependencyType type;
    }
}
