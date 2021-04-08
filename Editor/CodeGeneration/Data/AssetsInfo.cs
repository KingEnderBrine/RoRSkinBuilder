using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    public class AssetsInfo
    {
        private readonly SkinModInfo skinModInfo;
        public readonly string uccModName;
        public readonly string assetBundleName;
        public readonly string assetBundlePath;
        public readonly string modFolder;
        public readonly Dictionary<Sprite, string> iconPaths = new Dictionary<Sprite, string>();
        public readonly Dictionary<IconColors, string> iconFromColorPaths = new Dictionary<IconColors, string>();
        public readonly Dictionary<Material, string> materialPaths = new Dictionary<Material, string>();
        public readonly Dictionary<Mesh, string> meshPaths = new Dictionary<Mesh, string>();
        public readonly Dictionary<GameObject, string> gameObjectPaths = new Dictionary<GameObject, string>();
        public readonly Dictionary<Material, string> materialsWithRoRShader = new Dictionary<Material, string>();

        public AssetsInfo(SkinModInfo skinModInfo)
        {
            this.skinModInfo = skinModInfo;
            uccModName = skinModInfo.modName.ToUpperCamelCase();
            assetBundleName = (skinModInfo.author.ToUpperCamelCase() + uccModName).ToLower();
            assetBundlePath = Path.Combine("SkinModAssets", uccModName);
            modFolder = Path.Combine("Assets", "SkinMods", uccModName);
        }

        public void CreateNecessaryAssetsAndFillPaths(bool regenerateAssemblyDefinition)
        {
            var fullModFolderPath = Path.Combine(Environment.CurrentDirectory, modFolder);
            Directory.CreateDirectory(fullModFolderPath);

            var assemblyDefinitionPath = Path.Combine(fullModFolderPath, uccModName + ".asmdef");
            if (!File.Exists(assemblyDefinitionPath) || regenerateAssemblyDefinition)
            {
                File.WriteAllText(assemblyDefinitionPath, defaultAssemblyDefinition.Replace("ASSEMBLY_DEFINITION_NAME", uccModName));
            }

            AssetDatabase.Refresh();

            var tmpIconFolder = Path.Combine(modFolder, "Icons");
            if (Directory.Exists(tmpIconFolder))
            {
                Directory.Delete(tmpIconFolder, true);
            }
            Directory.CreateDirectory(tmpIconFolder);

            var meshSet = new HashSet<Mesh>();
            foreach (var skin in skinModInfo.skins)
            {
                if (skin == null)
                {
                    continue;
                }
                if (skin.icon.createFromColors)
                {
                    var path = Path.Combine(tmpIconFolder, skin.name + "Icon.png");
                    var texture = IconColors.CreateSkinIcon(skin.icon.colors);
                    File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, path), texture.EncodeToPNG());

                    AssetDatabase.Refresh();
                    var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.spriteImportMode = SpriteImportMode.Single;
                    textureImporter.spritePivot = new Vector2(0.5F, 0.5F);
                    textureImporter.spriteBorder = new Vector4(0, 0, 128F, 128F);

                    textureImporter.SaveAndReimport();

                    iconFromColorPaths[skin.icon.colors] = path;
                }
                else if (skin.icon.sprite)
                {
                    iconPaths[skin.icon.sprite] = AssetDatabase.GetAssetPath(skin.icon.sprite);
                }
                foreach (var rendererInfo in skin.rendererInfos)
                {
                    if (!rendererInfo.defaultMaterial)
                    {
                        continue;
                    }
                    materialPaths[rendererInfo.defaultMaterial] = AssetDatabase.GetAssetPath(rendererInfo.defaultMaterial);
                    if (rendererInfo.defaultMaterial.shader.name.StartsWith("Fake RoR/"))
                    {
                        materialsWithRoRShader[rendererInfo.defaultMaterial] = rendererInfo.defaultMaterial.shader.name.Replace("Fake RoR/", "");
                    }
                }

                foreach (var meshReplacement in skin.meshReplacements)
                {
                    if (meshReplacement.mesh)
                    {
                        meshSet.Add(meshReplacement.mesh);
                    }
                }

                foreach (var projectileGhostReplacement in skin.projectileGhostReplacements)
                {
                    gameObjectPaths[projectileGhostReplacement.projectileGhost] = AssetDatabase.GetAssetPath(projectileGhostReplacement.projectileGhost);
                }
            }

            var tmpMeshesFolder = Path.Combine(modFolder, "Meshes");
            if (Directory.Exists(tmpMeshesFolder))
            {
                Directory.Delete(tmpMeshesFolder, true);
            }
            Directory.CreateDirectory(tmpMeshesFolder);

            foreach (var mesh in meshSet)
            {
                var path = AssetDatabase.GetAssetPath(mesh);
                if (AssetDatabase.IsSubAsset(mesh))
                {
                    path = Path.Combine(tmpMeshesFolder, mesh.name + ".mesh");
                    AssetDatabase.CreateAsset(GameObject.Instantiate(mesh), path);
                }
                meshPaths[mesh] = path;
            }
            AssetDatabase.Refresh();
        }

        public bool BuildAssetBundle()
        {
            var assetNames = new List<string>();
            assetNames.AddRange(materialPaths.Values);
            assetNames.AddRange(iconPaths.Values);
            assetNames.AddRange(meshPaths.Values);
            assetNames.AddRange(iconFromColorPaths.Values);

            foreach (var resource in skinModInfo.additionalResources)
            {
                assetNames.Add(AssetDatabase.GetAssetPath(resource));
            }

            Directory.CreateDirectory(assetBundlePath);

            var manifest = BuildPipeline.BuildAssetBundles(
                assetBundlePath,
                new[]
                {
                    new AssetBundleBuild
                    {
                        assetBundleName = assetBundleName,
                        assetNames = assetNames.ToArray()
                    }
                },
                BuildAssetBundleOptions.None,
                BuildTarget.StandaloneWindows);

            return manifest != null;
        }

        public void AddCsc()
        {
            var fullModFolderPath = Path.Combine(Environment.CurrentDirectory, modFolder);
            File.WriteAllText(Path.Combine(fullModFolderPath, "csc.rsp"), $"-res:\"{Path.Combine(assetBundlePath, assetBundleName)}\",\"{uccModName}.{assetBundleName}\",public");
        }

        private const string defaultAssemblyDefinition =
@"
{
    ""name"": ""ASSEMBLY_DEFINITION_NAME"",
    ""references"": [],
    ""optionalUnityReferences"": [],
    ""includePlatforms"": [],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": false,
    ""overrideReferences"": true,
    ""precompiledReferences"": [
        ""Assembly-CSharp.refstub.dll"",
        ""BepInEx.dll"",
        ""MonoMod.RuntimeDetour.dll""
    ],
    ""autoReferenced"": false,
    ""defineConstraints"": []
    }";
    }
}
