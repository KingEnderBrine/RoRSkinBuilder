using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoRSkinBuilder.Data;

namespace RoRSkinBuilder.Extensions
{
    public interface ILobbySkinExtension
    {
        bool Validate(LobbySkinDefinition skinDefinition, SkinDefinition parentSkin);
        void OnBuild(AssetsInfo assetsInfo, LobbySkinDefinition skinDefinition, SkinDefinition parentSkin);
    }
}
