using System;
using RoRSkinBuilder.CustomEditors;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class GameObjectActivation
    {
        public GameObjectActivationAccessType accessType = GameObjectActivationAccessType.ByRendererName;
        [ShowWhen(nameof(accessType), false, GameObjectActivationAccessType.ByRendererName)]
        public string rendererName;
        [ShowWhen(nameof(accessType), false, GameObjectActivationAccessType.ByPath)]
        public string path;
        public bool shouldActivate;

        [HideInInspector]
        public bool isExpanded;
    }
}
