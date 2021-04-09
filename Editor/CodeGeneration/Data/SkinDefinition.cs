using RoRSkinBuilder.CustomEditors;
using System.Collections.Generic;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [CreateAssetMenu(menuName = "RoR Skins/"+ nameof(SkinDefinition))]
    public class SkinDefinition : ScriptableObject
    {
        [Tooltip("Fill this if skin depends on another mod")]
        public Dependency modDependency;
        public ConfigInfo config;
        [Tooltip("The name of body prefab e.g. 'CommandoBody', 'MageBody'")]
        public string bodyName;
        [Tooltip("The name of your skin in different languages")]
        public List<TokenLocalization> nameTokenLocalizations;
        [Tooltip("Skin icon in lobby")]
        public IconInfo icon;
        [Tooltip("Name of achievemnt after which skin will be available")]
        public string unlockableName;
        [Tooltip("A list of skins that the game will apply before this one")]
        public List<Reference> baseSkins;
        [Space(30)]
        [Tooltip(@"Which way to populate renderers collection.

AllRendererComponents - get all renderers on a prefab (including particles and disabled ones)

BaseRendererInfos - get renderers from `CharacterModel.baseRendererInfos` field")]
        public RenderersSource renderersSource;
        [Space]
        [Tooltip("Replacements for in-game models")]
        public List<MeshReplacement> meshReplacements;
        [Tooltip("Disable/enable models when skin is used")]
        public List<GameObjectActivation> gameObjectActivations;
        [Tooltip("Which material to use on which model")]
        public List<RendererInfo> rendererInfos;
        [Tooltip("Skins for minions e.g Engi turrets")]
        public List<MinionSkinReplacement> minionSkinReplacements;
        [Tooltip("Replacement for projectiles")]
        public List<ProjectileGhostReplacement> projectileGhostReplacements;

        public string CreateNameToken(string author)
        {
            return author.Trim().ToUpper().Replace(" ", "_") + "_SKIN_" + name.Trim().ToUpper().Replace(" ", "_") + "_NAME";
        }
    }
}
