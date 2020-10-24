using System.Collections.Generic;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [CreateAssetMenu(menuName = "RoR Skins/" + nameof(SkinModInfo))]
    public class SkinModInfo : ScriptableObject
    {
        public string modName;
        public string author;
        public string version;
        public List<SkinDefinition> skins;
        public List<UnityEngine.Object> additionalResources;
    }
}
