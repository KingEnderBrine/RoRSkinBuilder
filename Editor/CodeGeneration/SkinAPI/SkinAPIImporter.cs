using RoRSkinBuilder.Data;
using RoRSkinBuilder.SkinAPI.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.SkinAPI
{
    public class SkinAPIImporter
    {
        private readonly string path;
        private readonly SkinModInfo skinModInfo;
        private readonly string importPath;
        private readonly string unityImportPath;

        public SkinAPIImporter(string path, SkinModInfo skinModInfo)
        {
            this.path = path;
            this.skinModInfo = skinModInfo;
            unityImportPath = Path.Combine("Assets", "Resources", skinModInfo.modName);
            importPath = Path.Combine(Environment.CurrentDirectory, unityImportPath);
        }

        public void Import()
        {
            var skins = new List<SkinDefinition>();
            var pendingMinionSkins = new Dictionary<SkinConfig, SkinDefinition>();

            Directory.CreateDirectory(importPath);

            var importedSkins = false;

            foreach (var skinDirectory in GatherDirectories(path))
            {
                importedSkins = true;

                var skinConfigPath = Path.Combine(skinDirectory, "Skin.cfg");

                var skinConfig = BepInExConfigReader.ReadConfig<SkinConfig>(skinConfigPath);

                var skinDefinition = ScriptableObject.CreateInstance<SkinDefinition>();
                skinDefinition.name = Path.GetFileName(skinDirectory);
                skinDefinition.bodyName = skinConfig.CharacterBodyName;
                skinDefinition.icon = new IconInfo
                {
                    createFromColors = true,
                    colors = new IconColors
                    {
                        left = skinConfig.IconColorLeft,
                        top = skinConfig.IconColorTop,
                        right = skinConfig.IconColorRight,
                        bottom = skinConfig.IconColorBottom
                    }
                };
                skinDefinition.unlockableName = skinConfig.UnlockableName;
                skinDefinition.nameTokenLocalizations = new List<TokenLocalization>
                {
                    new TokenLocalization
                    {
                        language = "en",
                        value = skinConfig.SkinName
                    }
                };
                skinDefinition.baseSkins = new List<Reference>();
                skinDefinition.meshReplacements = new List<MeshReplacement>();
                skinDefinition.gameObjectActivations = new List<GameObjectActivation>();
                skinDefinition.minionSkinReplacements = new List<MinionSkinReplacement>();
                skinDefinition.rendererInfos = new List<RendererInfo>();
                skinDefinition.projectileGhostReplacements = new List<ProjectileGhostReplacement>();
                
                if (string.IsNullOrWhiteSpace(skinConfig.SkinMeshToUse))
                {
                    if (skinConfig.MeshReplacementIndex >= 0)
                    {
                        skinDefinition.baseSkins.Add(new Reference { accessType = AccessType.ByIndex, index = 0 });
                    }
                }
                else if (skinConfig.SkinMeshToUse.ToLower().Contains("default"))
                {
                    skinDefinition.baseSkins.Add(new Reference { accessType = AccessType.ByIndex, index = 0 });
                }

                var skinImportPath = Path.Combine(importPath, skinDefinition.name);
                Directory.CreateDirectory(skinImportPath);
                var unitySkinImportPath = Path.Combine(unityImportPath, skinDefinition.name);
                foreach (var materialDirectory in Directory.GetDirectories(skinDirectory))
                {
                    var materialConfigPath = Path.Combine(materialDirectory, "Material.cfg");
                    if (!File.Exists(materialConfigPath))
                    {
                        continue;
                    }
                    var materialConfig = BepInExConfigReader.ReadConfigDictionary(materialConfigPath);
                    var material = new Material(Shader.Find("Fake RoR/Hopoo Games/Deferred/Standard"));
                    material.name = "mat" + Path.GetFileName(materialDirectory);
                    foreach (var row in materialConfig)
                    {
                        if (row.Value is float floatValue)
                        {
                            material.SetFloat(row.Key, floatValue);
                        }
                        else if (row.Value is Color colorValue)
                        {
                            material.SetColor(row.Key, colorValue);
                        }
                        else if (row.Value is bool boolValue)
                        {
                            if (boolValue)
                            {
                                material.EnableKeyword(row.Key);
                            }
                            else
                            {
                                material.DisableKeyword(row.Key);
                            }
                        }
                    }

                    var textureImportPath = Path.Combine(skinImportPath, Path.GetFileName(materialDirectory));
                    Directory.CreateDirectory(textureImportPath);
                    var textureNames = new List<string>();
                    foreach (var textureFilePath in Directory.GetFiles(materialDirectory, "*.png", SearchOption.TopDirectoryOnly))
                    {
                        var textureFileName = Path.GetFileNameWithoutExtension(textureFilePath);
                        textureNames.Add(textureFileName);
                        File.Copy(textureFilePath, Path.Combine(textureImportPath, textureFileName + ".png"));
                    }
                    AssetDatabase.Refresh();

                    var unityTextureDirectory = Path.Combine(unitySkinImportPath, Path.GetFileName(materialDirectory));
                    foreach (var textureName in textureNames)
                    {
                        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(unityTextureDirectory, textureName + ".png"));
                        material.SetTexture(textureName, texture);
                    }
                    AssetDatabase.CreateAsset(material, Path.Combine(unityTextureDirectory, material.name + ".mat"));

                    skinDefinition.rendererInfos.Add(new RendererInfo
                    {
                        defaultMaterial = material,
                        defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                        ignoreOverlays = false,
                        rendererReference = new Reference
                        {
                            accessType = AccessType.ByName,
                            name = Path.GetFileName(materialDirectory)
                        }
                    });
                }

                if (!string.IsNullOrWhiteSpace(skinConfig.MinionReplacementForBody) && 
                    skinConfig.MinionReplacementForBody != "NONE" &&
                    !string.IsNullOrWhiteSpace(skinConfig.MinionReplacementForSkin) &&
                    skinConfig.MinionReplacementForSkin != "NONE")
                {
                    pendingMinionSkins.Add(skinConfig, skinDefinition);
                }

                skins.Add(skinDefinition);
            }

            if (!importedSkins)
            {
                EditorUtility.DisplayDialog("RyanSkinAPI import", "Could not find any skins", "Ok");
                return;
            }

            foreach (var pendingSkin in pendingMinionSkins)
            {
                var ownerSkin = skins.FirstOrDefault(el => el.bodyName == pendingSkin.Key.MinionReplacementForBody && el.nameTokenLocalizations[0].value == pendingSkin.Key.MinionReplacementForSkin);
                if (ownerSkin == null)
                {
                    continue;
                }

                ownerSkin.minionSkinReplacements.Add(new MinionSkinReplacement
                {
                    bodyName = pendingSkin.Value.bodyName,
                    findSkinByReference = false,
                    skin = pendingSkin.Value
                });
            }

            Directory.CreateDirectory(Path.Combine(importPath, "Skins"));
            var unitySkinsDirectory = Path.Combine(unityImportPath, "Skins");
            foreach (var skin in skins)
            {
                AssetDatabase.CreateAsset(skin, Path.Combine(unitySkinsDirectory, skin.name + ".asset"));
                skinModInfo.skins.Add(skin);
            }
            AssetDatabase.SaveAssets();
        }

        private IEnumerable<string> GatherDirectories(string path)
        {
            foreach (var directory in Directory.GetDirectories(path))
            {
                if (File.Exists(Path.Combine(directory, "Skin.cfg")))
                {
                    yield return directory;
                    continue;
                }
                foreach (var gatheredDirectory in GatherDirectories(directory))
                {
                    yield return gatheredDirectory;
                }
            }
        }
    }
}
