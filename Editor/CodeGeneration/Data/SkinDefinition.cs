using RoRSkinBuilder.CodeGeneration.CustomEditors;
using RoRSkinBuilder.CodeGeneration.Extensions;
using RoRSkinBuilder.CustomEditors;
using RoRSkinBuilder.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [CreateAssetMenu(menuName = "RoR Skins/"+ nameof(SkinDefinition))]
    public class SkinDefinition : ScriptableObject, ISkinParams
    {
        [Tooltip("Fill this if skin depends on another mod")]
        public Dependency modDependency;
        [NoFoldout]
        public ConfigInfo config;
        [Tooltip("The name of body prefab e.g. 'CommandoBody', 'MageBody'")]
        public string bodyName;
        public bool createLobbySkin = true;
        [ShowWhen(nameof(createLobbySkin), false, true)]
        public LobbySkinDefinition lobbySkinOverride;
        [Tooltip("The name of your skin in different languages")]
        public List<TokenLocalization> nameTokenLocalizations;
        [Tooltip("Skin icon in lobby")]
        public IconInfo icon;
        [Tooltip("Name of achievement after which skin will be available")]
        public string unlockableName;
        [Space]
        [Tooltip("Used for building DynamicSkins related code")]
        public GameObject referenceModel;
        [Tooltip("A list of skins that the game will apply before this one")]
        public List<Reference> baseSkins;
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
        [Tooltip("Colors for lights")]
        public List<LightInfo> lightReplacements;
        public List<ISkinExtension> extensions = new List<ISkinExtension>();

        public string NameForMethods => $"{bodyName.ToUpperCamelCase()}{name.ToUpperCamelCase()}";

        string ISkinParams.Name => name;
        GameObject ISkinParams.ReferenceModel => referenceModel;
        List<Reference> ISkinParams.BaseSkins => baseSkins;
        List<MeshReplacement> ISkinParams.MeshReplacements => meshReplacements;
        List<GameObjectActivation> ISkinParams.GameObjectActivations => gameObjectActivations;
        List<RendererInfo> ISkinParams.RendererInfos => rendererInfos;
        List<MinionSkinReplacement> ISkinParams.MinionSkinReplacements => minionSkinReplacements;
        List<ProjectileGhostReplacement> ISkinParams.ProjectileGhostReplacements => projectileGhostReplacements;
        List<LightInfo> ISkinParams.LightReplacements => lightReplacements;
        List<ISkinExtension> ISkinParams.Extensions => extensions;

        public string CreateNameToken(string author)
        {
            return author.Trim().ToUpper().Replace(" ", "_") + "_SKIN_" + name.Trim().ToUpper().Replace(" ", "_") + "_NAME";
        }

        public void OnValidate()
        {
            if (gameObjectActivations != null)
            {
                foreach (var activation in gameObjectActivations)
                {
                    if (activation.accessType == 0)
                    {
                        activation.accessType = GameObjectActivationAccessType.ByRendererName;
                    }
                }
            }
        }
    }
}
