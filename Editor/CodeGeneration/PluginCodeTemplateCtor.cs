using RoRSkinBuilder.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoRSkinBuilder
{
    public partial class PluginCodeTemplate
    {
        private SkinModInfo SkinModInfo { get; }
        private AssetsInfo AssetsInfo { get; }
        private List<SkinDefinition> ReorderedSkins { get; }
        private Dictionary<string, DependencyType> DistinctDependencies { get; }

        public PluginCodeTemplate(SkinModInfo skinModInfo, AssetsInfo assetsInfo)
        {
            SkinModInfo = skinModInfo;
            AssetsInfo = assetsInfo;
            ReorderedSkins = ReorderSkins(skinModInfo.skins);
            DistinctDependencies = GetDistinctDependencies(ReorderedSkins);
        }

        private Dictionary<string, DependencyType> GetDistinctDependencies(List<SkinDefinition> skins)
        {
            var dependencies = new Dictionary<string, DependencyType>();
            foreach (var skin in skins)
            {
                if (skin == null)
                {
                    continue;
                }
                if (string.IsNullOrWhiteSpace(skin.modDependency.value))
                {
                    continue;
                }
                if (dependencies.TryGetValue(skin.modDependency.value, out _))
                {
                    if (skin.modDependency.type == DependencyType.HardDependency)
                    {
                        dependencies[skin.modDependency.value] = DependencyType.HardDependency;
                    }
                }
                else
                {
                    dependencies[skin.modDependency.value] = skin.modDependency.type;
                }
            }

            return dependencies;
        }

        private List<SkinDefinition> ReorderSkins(List<SkinDefinition> sourceSkins)
        {
            var requiredSkins = new Dictionary<SkinDefinition, List<SkinDefinition>>();
            var skins = new List<SkinDefinition>();
            foreach (var skin in sourceSkins)
            {
                if (skin == null)
                {
                    continue;
                }
                if (skin.minionSkinReplacements.Count == 0)
                {
                    skins.Add(skin);
                    continue;
                }
                requiredSkins[skin] = skin.minionSkinReplacements.Select(el => el.skin).Where(el => el != null).ToList();
            }
            var previousSkinCount = requiredSkins.Count;
            while (requiredSkins.Count > 0)
            {
                foreach (var row in requiredSkins)
                {
                    foreach (var requiredSkin in row.Value.ToList())
                    {
                        if (skins.Contains(requiredSkin))
                        {
                            row.Value.Remove(requiredSkin);
                        }
                    }
                }
                foreach (var row in requiredSkins.ToList())
                {
                    if (row.Value.Count == 0)
                    {
                        skins.Add(row.Key);
                        requiredSkins.Remove(row.Key);
                        continue;
                    }
                }

                if (previousSkinCount == requiredSkins.Count)
                {
                    throw new ArgumentException("Can't solve minion skin dependencies");
                }
                previousSkinCount = requiredSkins.Count;
            }

            return skins;
        }
    }
}
