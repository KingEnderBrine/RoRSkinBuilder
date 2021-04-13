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
        protected override bool CanBeHidden => false;
        protected override bool ShowLabel => false;
        protected override bool Indent => false;
    }
}
