using RoRSkinBuilder.CodeGeneration;
using RoRSkinBuilder.Data;
using RoRSkinBuilder.SkinAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
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

            assetInfo.CreateNecessaryAssetsAndFillPaths(skinModInfo.regenerateAssemblyDefinition);

            var path = Path.Combine(assetInfo.modFolder, assetInfo.uccModName + "Plugin.cs");
            if (skinModInfo.regenerateCode)
            {
                var pluginCode = new PluginCodeTemplate(skinModInfo, assetInfo);
                File.WriteAllText(path, pluginCode.TransformText());
            }

            if (!assetInfo.BuildAssetBundle())
            {
                return;
            }

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            CompilationPipeline.assemblyCompilationFinished += WaitForCompilation;
            
            void WaitForCompilation(string assemblyPath, CompilerMessage[] messages)
            {
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

                var buildFolder = Path.Combine(Environment.CurrentDirectory, "Builds", assetInfo.uccModName);
                if (Directory.Exists(buildFolder))
                {
                    Directory.Delete(buildFolder, true);
                }
                Directory.CreateDirectory(buildFolder);
                File.Copy(Path.Combine(Environment.CurrentDirectory, "Library", "ScriptAssemblies", assetInfo.uccModName + ".dll"), Path.Combine(buildFolder, assetInfo.uccModName + ".dll"), true);
                if (skinModInfo.copyDebugFile)
                {
                    File.Copy(Path.Combine(Environment.CurrentDirectory, "Library", "ScriptAssemblies", assetInfo.uccModName + ".pdb"), Path.Combine(buildFolder, assetInfo.uccModName + ".pdb"), true);
                }
                File.Copy(Path.Combine(assetInfo.assetBundlePath, assetInfo.assetBundleName), Path.Combine(buildFolder, assetInfo.assetBundleName), true);
                Directory.CreateDirectory(Path.Combine(buildFolder, "Language"));

                var tokensByLanguage = new Dictionary<string, Dictionary<string, string>>();
                foreach (var skin in skinModInfo.skins)
                {
                    if (skin.nameTokenLocalizations.Count == 0)
                    {
                        continue;
                    }
                    var nameToken = skin.CreateNameToken(skinModInfo.author);

                    foreach (var localization in skin.nameTokenLocalizations)
                    {
                        var language = localization.language.ToLower();
                        if (!tokensByLanguage.TryGetValue(language, out var tokens))
                        {
                            tokensByLanguage[language] = tokens = new Dictionary<string, string>();
                        }
                        tokens[nameToken] = localization.value;
                    }
                }

                foreach (var row in tokensByLanguage)
                {
                    var languageFolder = Path.Combine(buildFolder, "Language", row.Key);
                    Directory.CreateDirectory(languageFolder);
                    var tokensPath = Path.Combine(languageFolder, "tokens.json");
                    File.WriteAllText(tokensPath, LanguageToJson(row.Value));
                }

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

            var replacementNames = new HashSet<string>();
            var uniqueReplacements = new HashSet<MaterialReplacement>();
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
                foreach (var renderer in skin.rendererInfos)
                {
                    if (renderer.useMaterialReplacement
                        && renderer.materialReplacement
                        && uniqueReplacements.Add(renderer.materialReplacement)
                        && !replacementNames.Add(renderer.materialReplacement.name.ToUpperCamelCase()))
                    {
                        EditorUtility.DisplayDialog("Error", $"Duplicate \"Material Replacement\" name \"{renderer.materialReplacement.name}\"", "Ok");
                        return false;
                    }
                }
            }

            return true;
        }

        public static string LanguageToJson(Dictionary<string, string> strings)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("{");
            stringBuilder.Append("    \"strings\": {");
            foreach (var item in strings)
            {
                stringBuilder.AppendLine().Append($"        \"{item.Key.EscapeJsonString()}\": \"{item.Value.EscapeJsonString()}\",");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }
    }
}
