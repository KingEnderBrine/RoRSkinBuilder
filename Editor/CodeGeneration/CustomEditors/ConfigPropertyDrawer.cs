using RoRSkinBuilder.Data;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;

namespace RoRSkinBuilder.CustomEditors
{
    [CustomPropertyDrawer(typeof(ConfigInfo))]
    public class ConfigPropertyDrawer : HideablePropertyDrawer<ConfigInfo>
    {
        public ConfigPropertyDrawer()
        {
            canBeHidden = false;
            showLabel = false;
            indent = false;
        }
    }
}
