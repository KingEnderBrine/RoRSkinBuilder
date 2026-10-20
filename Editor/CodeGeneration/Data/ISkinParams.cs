using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoRSkinBuilder.Extensions;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    public interface ISkinParams
    {
        string Name { get; }
        GameObject ReferenceModel { get; }
        List<Reference> BaseSkins { get; }
        List<MeshReplacement> MeshReplacements { get; }
        List<GameObjectActivation> GameObjectActivations { get; }
        List<RendererInfo> RendererInfos { get; }
        List<MinionSkinReplacement> MinionSkinReplacements { get; }
        List<ProjectileGhostReplacement> ProjectileGhostReplacements { get; }
        List<LightInfo> LightReplacements { get; }
        List<ISkinExtension> Extensions { get; }
    }
}
