using System;
using System.Collections.Generic;
using System.Text;
using RoRSkinBuilder.Data;

namespace RoRSkinBuilder.Extensions
{
    public interface ISkinExtension
    {
        bool Validate(SkinDefinition skinDefinition);
        void OnBuild(AssetsInfo assetsInfo, SkinDefinition skinDefinition);
    }
}
