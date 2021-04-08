using System.Collections.Generic;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [CreateAssetMenu(menuName = "RoR Skins/" + nameof(SkinModInfo))]
    public class SkinModInfo : ScriptableObject
    {
        [Tooltip("Mod name")]
        public string modName;
        [Tooltip("Author of the mod")]
        public string author;
        [Tooltip("Version number of the mod, following the semantic version format 'Major.Minor.Patch' e.g. '1.0.0'")]
        public string version;
        [Tooltip("Collection of skins to add into mod")]
        public List<SkinDefinition> skins;
        [Tooltip("Additional resources which need to be added to the asset bundle")]
        public List<UnityEngine.Object> additionalResources;
        [Header("Build")]
        [Tooltip("Should code be regenerated on build")]
        public bool regenerateCode = true;
        [Tooltip("Should AssemblyDefinition be regenerated on build")]
        public bool regenerateAssemblyDefinition = true;
    }
}
