using System.Collections.Generic;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [CreateAssetMenu(menuName = "RoR Skins/"+ nameof(SkinDefinition))]
    public class SkinDefinition : ScriptableObject
    {
        public Dependency modDependency;
        public string bodyName;
        public List<TokenLocalization> nameTokenLocalizations;
        public IconInfo icon;
        public string unlockableName;
        public List<Reference> baseSkins;
        public List<MeshReplacement> meshReplacements;
        public List<GameObjectActivation> gameObjectActivations;
        public List<RendererInfo> rendererInfos;
        public List<MinionSkinReplacement> minionSkinReplacements;
        public List<ProjectileGhostReplacement> projectileGhostReplacements;

        public string CreateNameToken(string author)
        {
            return author.Trim().ToUpper().Replace(" ", "_") + "_SKIN_" + name.Trim().ToUpper().Replace(" ", "_") + "_NAME";
        }
    }
}
