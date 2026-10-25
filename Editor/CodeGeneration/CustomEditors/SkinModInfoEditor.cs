using RoRSkinBuilder.Data;
using RoRSkinBuilder.SkinAPI;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace RoRSkinBuilder.CustomEditors
{
    [CustomEditor(typeof(SkinModInfo))]
    public class SkinModInfoEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Build"))
            {
                Build(serializedObject.targetObject as SkinModInfo);
            }
            if (GUILayout.Button("Import skins from RyanSkinAPI"))
            {
                ImportSkinApiSkins(serializedObject.targetObject as SkinModInfo);
            }
        }

        private static void Build(SkinModInfo skinModInfo)
        {
            var assetInfo = new AssetsInfo(skinModInfo);

            if (!Validate(skinModInfo))
            {
                return;
            }

            foreach (var extension in skinModInfo.extensions)
            {
                if ((UnityEngine.Object)extension && !extension.Validate(skinModInfo))
                {
                    return;
                }
            }
            foreach (var skin in skinModInfo.skins)
            {
                if (skin)
                {
                    foreach (var extension in skin.extensions)
                    {
                        if ((UnityEngine.Object)extension && !extension.Validate(skin))
                        {
                            return;
                        }
                    }

                    if (skin.createLobbySkin && skin.lobbySkinOverride)
                    {
                        foreach (var extension in skin.lobbySkinOverride.extensions)
                        {
                            if ((UnityEngine.Object)extension && !extension.Validate(skin.lobbySkinOverride, skin))
                            {
                                return;
                            }
                        }
                    }
                }
            }

            assetInfo.CreateNecessaryAssetsAndFillPaths(skinModInfo.regenerateAssemblyDefinition);

            var path = Path.Combine(assetInfo.modFolder, assetInfo.uccModName + "Plugin.cs");
            if (skinModInfo.regenerateCode)
            {
                var pluginCode = new PluginBuilder(assetInfo);
                File.WriteAllText(path, pluginCode.Build());
            }

            foreach (var extension in skinModInfo.extensions)
            {
                if ((UnityEngine.Object)extension)
                {
                    extension.OnBuild(assetInfo);
                }
            }
            foreach (var skin in skinModInfo.skins)
            {
                if (skin)
                {
                    foreach (var extension in skin.extensions)
                    {
                        if ((UnityEngine.Object)extension)
                        {
                            extension.OnBuild(assetInfo, skin);
                        }
                    }

                    if (skin.createLobbySkin && skin.lobbySkinOverride)
                    {
                        foreach (var extension in skin.lobbySkinOverride.extensions)
                        {
                            if ((UnityEngine.Object)extension)
                            {
                                extension.OnBuild(assetInfo, skin.lobbySkinOverride, skin);
                            }
                        }
                    }
                }
            }

            StaticCodeBuilder.TryUpdate(assetInfo);

            if (!assetInfo.BuildAssetBundle())
            {
                return;
            }

            assetInfo.AddCsc();

            var noCompilation = true;
            CompilationPipeline.assemblyCompilationFinished += WaitForCompilation;
            CompilationPipeline.compilationFinished += WaitForCompilationAll;
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

            void WaitForCompilationAll(object obj)
            {
                CompilationPipeline.compilationFinished -= WaitForCompilationAll;
                if (noCompilation)
                {
                    FinishBuild();
                }
            }
            void WaitForCompilation(string assemblyPath, CompilerMessage[] messages)
            {
                noCompilation = false;

                if (!assemblyPath.EndsWith(assetInfo.uccModName + ".dll"))
                {
                    return;
                }

                CompilationPipeline.assemblyCompilationFinished -= WaitForCompilation;
                if (messages.Length != 0)
                {
                    foreach (var message in messages)
                    {
                        if (message.type == CompilerMessageType.Error)
                        {
                            return;
                        }
                    }
                }

                FinishBuild();
            }

            void FinishBuild()
            {
                var buildFolder = Path.Combine(Environment.CurrentDirectory, "Builds", assetInfo.uccModName);
                Directory.CreateDirectory(buildFolder);
                File.Copy(Path.Combine(Environment.CurrentDirectory, "Library", "ScriptAssemblies", assetInfo.uccModName + ".dll"), Path.Combine(buildFolder, assetInfo.uccModName + ".dll"), true);
                Process.Start(buildFolder);
            }
        }

        private static void ImportSkinApiSkins(SkinModInfo skinModInfo)
        {
            var path = EditorUtility.OpenFolderPanel("Select folder that contains skin folders", "", "");
            if (!Directory.Exists(path))
            {
                return;
            }

            new SkinAPIImporter(path, skinModInfo).Import();
        }

        private static bool Validate(SkinModInfo skinModInfo)
        {
            if (string.IsNullOrWhiteSpace(skinModInfo.author))
            {
                EditorUtility.DisplayDialog("Error", "\"Author\" field is empty", "Ok");
                return false;
            }
            if (string.IsNullOrWhiteSpace(skinModInfo.modName))
            {
                EditorUtility.DisplayDialog("Error", "\"Mod Name\" field is empty", "Ok");
                return false;
            }
            if (!Regex.IsMatch(skinModInfo.version, @"\d+?\.\d+?\.\d+?"))
            {
                EditorUtility.DisplayDialog("Error", "\"Version\" field has invalid format. It should be X.X.X where X is a number", "Ok");
                return false;
            }

            foreach (var skin in skinModInfo.skins)
            {
                if (!skin)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(skin.bodyName))
                {
                    EditorUtility.DisplayDialog("Error", $"\"Body Name\" field for a \"{skin.name}\" skin is empty", "Ok");
                    return false;
                }

                if (!ValidateSkinParams(skin))
                {
                    return false;
                }

                if (skin.createLobbySkin && skin.lobbySkinOverride && !ValidateSkinParams(skin.lobbySkinOverride))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateSkinParams(ISkinParams skin)
        {
            for (var i = 0; i < skin.GameObjectActivations.Count; i++)
            {
                var gameObjectActivation = skin.GameObjectActivations[i];
                if (gameObjectActivation.spawnPrefabOnModelObject)
                {
                    if (!gameObjectActivation.useAddressablesKey && !gameObjectActivation.prefab)
                    {
                        EditorUtility.DisplayDialog("Error", $"\"Game Object Activations[{i}].Prefab\" field for a \"{skin.Name}\" skin is empty", "Ok");
                        return false;
                    }

                    if ((gameObjectActivation.addBonesToRenderers || gameObjectActivation.dynamicBonesColliderPaths.Count > 0) && !skin.ReferenceModel)
                    {
                        EditorUtility.DisplayDialog("Error", $"\"Reference Model\" field for a \"{skin.Name}\" skin is empty", "Ok");
                        return false;
                    }
                }

            }

            for (var i = 0; i < skin.MinionSkinReplacements.Count; i++)
            {
                var replacement = skin.MinionSkinReplacements[i];
                if (!replacement.findSkinByReference && !replacement.skin)
                {
                    EditorUtility.DisplayDialog("Error", $"\"Minion Skin Replacements[{i}].Skin\" field for a \"{skin.Name}\" skin is empty", "Ok");
                    return false;
                }
            }

            return true;
        }
    }
}
