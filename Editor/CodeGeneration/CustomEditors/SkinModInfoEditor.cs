using RoRSkinBuilder.Data;
using RoRSkinBuilder.SkinAPI;
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
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
            assetInfo.CreateNecessaryAssetsAndFillPaths();
            
            var pluginCode = new PluginCodeTemplate(skinModInfo, assetInfo);
            File.WriteAllText(Path.Combine(assetInfo.modFolder, assetInfo.uccModName + "Plugin.cs"), pluginCode.TransformText());
            
            if (!assetInfo.BuildAssetBundle())
            {
                return;
            }
            assetInfo.AddCsc();

            AssetDatabase.Refresh();
           
            var buildFolder = Path.Combine(Environment.CurrentDirectory, "Builds", assetInfo.uccModName);
            Directory.CreateDirectory(buildFolder);
            File.Copy(Path.Combine(Environment.CurrentDirectory, "Library", "ScriptAssemblies", assetInfo.uccModName + ".dll"), Path.Combine(buildFolder, assetInfo.uccModName + ".dll"), true);
            Process.Start(buildFolder);
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
    }
}
