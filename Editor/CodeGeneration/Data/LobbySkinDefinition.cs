using System;
using System.Collections.Generic;
using System.Text;
using RoRSkinBuilder.CustomEditors;
using RoRSkinBuilder.Extensions;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [CreateAssetMenu(menuName = "RoR Skins/" + nameof(LobbySkinDefinition))]
    public class LobbySkinDefinition : ScriptableObject, ISkinParams
    {
        [OnChanged(typeof(LobbySkinDefinition), nameof(CopyFromSkin))]
        public SkinDefinition copyFromSkin;

        [Space(36)]
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

        public static void CopyFromSkin(SerializedProperty property)
        {
            var skin = property.objectReferenceValue as SkinDefinition;
            if (skin)
            {
                var self = property.serializedObject.targetObject as LobbySkinDefinition;
                Undo.RecordObject(self, nameof(CopyFromSkin));
                self.copyFromSkin = null;
                EditorUtility.CopySerializedManagedFieldsOnly(skin, self);
                var serializedObject = property.serializedObject;
                serializedObject.UpdateIfRequiredOrScript();
            }
        }
    }
}
