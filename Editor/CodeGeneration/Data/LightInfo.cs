using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class LightInfo : IConfigEnabled
    {
        public string enableFromConfig;
        [Tooltip("Light component reference")]
        public ComponentReference lightReference;
        [Tooltip("New color")]
        public Color defaultColor = Color.white;

        public string EnableFromConfig => enableFromConfig;
    }
}
