using System.Collections.Generic;
using UnityEngine;

namespace RoRSkinBuilder
{
    public class BoneInfo
    {
        public int indexInParent;
        public bool inRenderer;
        public List<BoneInfo> children = new List<BoneInfo>();
    }
}