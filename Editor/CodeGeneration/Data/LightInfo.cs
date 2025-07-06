using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class LightInfo
    {
        [Tooltip("Light component reference")]
        public ComponentReference lightReference;
        [Tooltip("New color")]
        public Color defaultColor = Color.white;
    }
}
