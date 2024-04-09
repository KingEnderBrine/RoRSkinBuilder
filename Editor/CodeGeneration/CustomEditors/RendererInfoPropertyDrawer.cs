using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoRSkinBuilder.CustomEditors;
using RoRSkinBuilder.Data;
using UnityEditor;

namespace Packages.RoRSkinBuilder.Editor.CodeGeneration.CustomEditors
{
    [CustomPropertyDrawer(typeof(RendererInfo))]
    public class RendererInfoPropertyDrawer : HideablePropertyDrawer<RendererInfo>
    {
    }
}
