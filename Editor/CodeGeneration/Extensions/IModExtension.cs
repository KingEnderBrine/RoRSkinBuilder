using System;
using System.Collections.Generic;
using System.Text;
using RoRSkinBuilder.Data;

namespace RoRSkinBuilder.CodeGeneration.Extensions
{
    public interface IModExtension
    {
        bool Validate(SkinModInfo skinModInfo);
        void OnBuild(AssetsInfo assetsInfo);
    }
}
