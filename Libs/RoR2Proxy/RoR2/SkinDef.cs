using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoR2
{
    public class SkinDef : ScriptableObject
    {
        [Serializable]
        public struct GameObjectActivation
        {
            public GameObject gameObject;
            public bool shouldActivate;
        }

        [Serializable]
        public struct MeshReplacement
        {
            public Renderer renderer;
            public Mesh mesh;
        }

        [Serializable]
        public struct ProjectileGhostReplacement
        {
            public GameObject projectilePrefab;
            public GameObject projectileGhostReplacementPrefab;
        }

        [Serializable]
        public struct MinionSkinReplacement
        {
            public GameObject minionBodyPrefab;
            public SkinDef minionSkin;
        }

        public SkinDef[] baseSkins;
        public Sprite icon;
        public string nameToken;
        //public string unlockableName;
        //public UnlockableDef unlockableDef;
        //public GameObject rootObject;
        public CharacterModel.RendererInfo[] rendererInfos;
        public GameObjectActivation[] gameObjectActivations;
        public MeshReplacement[] meshReplacements;
        public ProjectileGhostReplacement[] projectileGhostReplacements;
        public MinionSkinReplacement[] minionSkinReplacements;
    }
}