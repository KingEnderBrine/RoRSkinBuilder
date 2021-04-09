using RoRSkinBuilder.CustomEditors;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class ConfigInfo
    {
        [Tooltip("Should config entry that will allow enable/disable skin be generated")]
        public bool generateEnableConfig;
        [Tooltip("Default value for config entry")]
        [ShowWhen(nameof(generateEnableConfig), false, true)]
        public bool enableConfigDefaultValue = true;

        [HideInInspector]
        public bool isExpanded;
    }
}
