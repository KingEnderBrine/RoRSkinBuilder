﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace RoRSkinBuilder
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using RoRSkinBuilder.Data;
    using UnityEngine.Rendering;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class PluginCodeTemplate : PluginCodeTemplateBase
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write(@"using BepInEx;
using BepInEx.Logging;
using RoR2;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System.Security.Permissions;
using MonoMod.RuntimeDetour.HookGen;
using RoR2.ContentManagement;
using UnityEngine.AddressableAssets;


#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
namespace ");
            this.Write(this.ToStringHelper.ToStringWithCulture(AssetsInfo.uccModName));
            this.Write("\r\n{\r\n");
 foreach (var dependency in DistinctDependencies) { 
            this.Write("    [BepInDependency(\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(dependency.Key));
            this.Write("\", BepInDependency.DependencyFlags.");
            this.Write(this.ToStringHelper.ToStringWithCulture(Enum.GetName(typeof(DependencyType), dependency.Value)));
            this.Write(")]\r\n");
 } 
            this.Write("    \r\n    [BepInPlugin(\"com.");
            this.Write(this.ToStringHelper.ToStringWithCulture(SkinModInfo.author.StripSpaces()));
            this.Write(".");
            this.Write(this.ToStringHelper.ToStringWithCulture(AssetsInfo.uccModName));
            this.Write("\",\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(SkinModInfo.modName));
            this.Write("\",\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(SkinModInfo.version));
            this.Write("\")]\r\n    public partial class ");
            this.Write(this.ToStringHelper.ToStringWithCulture(AssetsInfo.uccModName));
            this.Write("Plugin : BaseUnityPlugin\r\n    {\r\n        internal static ");
            this.Write(this.ToStringHelper.ToStringWithCulture(AssetsInfo.uccModName));
            this.Write("Plugin Instance { get; private set; }\r\n        internal static ManualLogSource In" +
                    "stanceLogger => Instance?.Logger;\r\n        \r\n        private static AssetBundle " +
                    "assetBundle;\r\n");
 if (AssetsInfo.materialsWithRoRShader.Count != 0) { 
            this.Write("        private static readonly List<Material> materialsWithRoRShader = new List<" +
                    "Material>();\r\n");
 } 
            this.Write("        private void Start()\r\n        {\r\n            Instance = this;\r\n\r\n        " +
                    "    BeforeStart();\r\n\r\n            using (var assetStream = Assembly.GetExecuting" +
                    "Assembly().GetManifestResourceStream(\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(AssetsInfo.uccModName));
            this.Write(".");
            this.Write(this.ToStringHelper.ToStringWithCulture(AssetsInfo.assetBundleName));
            this.Write(@"""))
            {
                assetBundle = AssetBundle.LoadFromStream(assetStream);
            }

            BodyCatalog.availability.CallWhenAvailable(BodyCatalogInit);
            HookEndpointManager.Add(typeof(Language).GetMethod(nameof(Language.LoadStrings)), (Action<Action<Language>, Language>)LanguageLoadStrings);

");
 if (AssetsInfo.materialsWithRoRShader.Count != 0) { 
            this.Write("            ReplaceShaders();\r\n\r\n");
 } 
            this.Write("            AfterStart();\r\n        }\r\n\r\n        partial void BeforeStart();\r\n    " +
                    "    partial void AfterStart();\r\n        static partial void BeforeBodyCatalogIni" +
                    "t();\r\n        static partial void AfterBodyCatalogInit();\r\n\r\n");
 if (AssetsInfo.materialsWithRoRShader.Count != 0) { 
            this.Write("        private static void ReplaceShaders()\r\n        {\r\n");
 foreach (var row in AssetsInfo.materialsWithRoRShader) { 
            this.Write("            LoadMaterialsWithReplacedShader(@\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(row.Key));
            this.Write("\"\r\n");
 foreach (var material in row.Value) { 
            this.Write("                ,@\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(AssetsInfo.materialPaths[material]));
            this.Write("\"");
 } 
            this.Write(");\r\n");
 } 
            this.Write(@"        }

        private static void LoadMaterialsWithReplacedShader(string shaderPath, params string[] materialPaths)
        {
            var shader = Addressables.LoadAssetAsync<Shader>(shaderPath).WaitForCompletion();
            foreach (var materialPath in materialPaths)
            {
                var material = assetBundle.LoadAsset<Material>(materialPath);
                material.shader = shader;
                materialsWithRoRShader.Add(material);
            }
        }
");
 } 
            this.Write("\r\n        private static void LanguageLoadStrings(Action<Language> orig, Language" +
                    " self)\r\n        {\r\n            orig(self);\r\n\r\n");
 var tokensByLanguage = new Dictionary<string, Dictionary<string, string>>();
var defaultTokens = new Dictionary<string, string>();

foreach (var skin in ReorderedSkins) {
    if (skin.nameTokenLocalizations.Count == 0) {
        continue;
    }
    var nameToken = skin.CreateNameToken(SkinModInfo.author);
    defaultTokens[nameToken] = (skin.nameTokenLocalizations.FirstOrDefault(el => el.language.ToLower() == "en") ?? skin.nameTokenLocalizations.First()).value; 
    
    foreach (var localization in skin.nameTokenLocalizations) {
        if (defaultTokens[nameToken] == localization.value) {
            continue;
        }
        var language = localization.language.ToLower();
        if (!tokensByLanguage.TryGetValue(language, out var tokens)) {
            tokensByLanguage[language] = tokens = new Dictionary<string, string>();
        }
        tokens[nameToken] = localization.value;
    } 
} 
 foreach (var row in defaultTokens) { 
            this.Write("            self.SetStringByToken(\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(row.Key));
            this.Write("\", \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(row.Value));
            this.Write("\");\r\n");
 } 
 if (tokensByLanguage.Count > 0) { 
            this.Write("\r\n            switch(self.name.ToLower())\r\n            {\r\n");
     foreach (var row in tokensByLanguage) { 
            this.Write("                case \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(row.Key));
            this.Write("\":\r\n");
         foreach (var value in row.Value) { 
            this.Write("                    self.SetStringByToken(\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(value.Key));
            this.Write("\", \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(value.Value));
            this.Write("\");\r\n");
         } 
            this.Write("                    break;\r\n");
     } 
            this.Write("            }\r\n");
 } 
            this.Write(@"        }

        private static void Nothing(Action<SkinDef> orig, SkinDef self)
        {

        }

        private static void BodyCatalogInit()
        {
            BeforeBodyCatalogInit();

            var awake = typeof(SkinDef).GetMethod(nameof(SkinDef.Awake), BindingFlags.NonPublic | BindingFlags.Instance);
            HookEndpointManager.Add(awake, (Action<Action<SkinDef>, SkinDef>)Nothing);

");
 foreach (var skin in ReorderedSkins) { 
            this.Write("            Add");
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.bodyName.ToUpperCamelCase()));
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.name.ToUpperCamelCase()));
            this.Write("Skin();\r\n");
 } 
            this.Write("            \r\n            HookEndpointManager.Remove(awake, (Action<Action<SkinDe" +
                    "f>, SkinDef>)Nothing);\r\n\r\n            AfterBodyCatalogInit();\r\n        }\r\n");
 foreach (var skin in ReorderedSkins) { 
            this.Write("\r\n        static partial void ");
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.bodyName.ToUpperCamelCase()));
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.name.ToUpperCamelCase()));
            this.Write("SkinAdded(SkinDef skinDef, GameObject bodyPrefab);\r\n\r\n        private static void" +
                    " Add");
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.bodyName.ToUpperCamelCase()));
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.name.ToUpperCamelCase()));
            this.Write("Skin()\r\n        {\r\n");
 if (!string.IsNullOrWhiteSpace(skin.modDependency.value) && skin.modDependency.type == DependencyType.SoftDependency) { 
            this.Write("            if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.modDependency.value));
            this.Write("\"))\r\n            {\r\n                return;\r\n            }\r\n");
 } 
 if (skin.config.generateEnableConfig) { 
            this.Write("            if (!Instance.Config.Bind(\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.name.ToUpperCamelCase()));
            this.Write("\", \"Enabled\", ");
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.config.enableConfigDefaultValue.ToLiteral()));
            this.Write(").Value)\r\n            {\r\n                return;\r\n            }\r\n");
 } 
            this.Write("            var bodyName = \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.bodyName));
            this.Write("\";\r\n            var skinName = \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.name));
            this.Write(@""";
            try
            {
                var bodyPrefab = BodyCatalog.FindBodyPrefab(bodyName);
                if (!bodyPrefab)
                {
                    InstanceLogger.LogWarning($""Failed to add \""{skinName}\"" skin because \""{bodyName}\"" doesn't exist"");
                    return;
                }

                var modelLocator = bodyPrefab.GetComponent<ModelLocator>();
                if (!modelLocator)
                {
                    InstanceLogger.LogWarning($""Failed to add \""{skinName}\"" skin to \""{bodyName}\"" because it doesn't have \""ModelLocator\"" component"");
                    return;
                }

                var mdl = modelLocator.modelTransform.gameObject;
                var skinController = mdl ? mdl.GetComponent<ModelSkinController>() : null;
                if (!skinController)
                {
                    InstanceLogger.LogWarning($""Failed to add \""{skinName}\"" skin to \""{bodyName}\"" because it doesn't have \""ModelSkinController\"" component"");
                    return;
                }

");
 switch (skin.renderersSource)
{
    case RenderersSource.AllRendererComponents: 
            this.Write("                var renderers = mdl.GetComponentsInChildren<Renderer>(true);\r\n");
      break;
    case RenderersSource.BaseRendererInfos: 
            this.Write(@"                var characterModel = mdl.GetComponent<CharacterModel>();
                if (!characterModel)
                {
                    InstanceLogger.LogWarning($""Failed to add \""{skinName}\"" skin to \""{bodyName}\"" because it doesn't have \""CharacterModel\"" component"");
                    return;
                }
                var renderers = characterModel.baseRendererInfos.Select(info => info.renderer).ToArray();
");
      break;
} 
            this.Write("\r\n                var skin = ScriptableObject.CreateInstance<SkinDef>();\r\n       " +
                    "         TryCatchThrow(\"Icon\", () =>\r\n                {\r\n");
 if (!skin.icon.createFromColors) {
        if (skin.icon.sprite) { 
            this.Write("                    skin.icon = assetBundle.LoadAsset<Sprite>(@\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(AssetsInfo.iconPaths[skin.icon.sprite]));
            this.Write("\");\r\n");
      } else { 
            this.Write("                    skin.icon = null;\r\n");
      } 
    } else { 
            this.Write("                    skin.icon = assetBundle.LoadAsset<Sprite>(@\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(AssetsInfo.iconFromColorPaths[skin.icon.colors]));
            this.Write("\");\r\n");
 } 
            this.Write("                });\r\n                skin.name = skinName;\r\n                skin." +
                    "nameToken = \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.CreateNameToken(SkinModInfo.author)));
            this.Write("\";\r\n                skin.rootObject = mdl;\r\n                TryCatchThrow(\"Base S" +
                    "kins\", () =>\r\n                {\r\n");
 if (skin.baseSkins.Count == 0) { 
            this.Write("                    skin.baseSkins = Array.Empty<SkinDef>();\r\n");
 } else { 
            this.Write("                    skin.baseSkins = new SkinDef[] \r\n                    { \r\n");
 foreach (var reference in skin.baseSkins) { 
    if (reference.accessType == AccessType.ByIndex) { 
            this.Write("                        skinController.skins[");
            this.Write(this.ToStringHelper.ToStringWithCulture(reference.index));
            this.Write("],\r\n");
 } else { 
            this.Write("                        skinController.skins.First(s => s.Name == \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(reference.name));
            this.Write("\")\r\n");
 } 
} 
            this.Write("                    };\r\n");
 } 
            this.Write("                });\r\n                TryCatchThrow(\"Unlockable Name\", () =>\r\n    " +
                    "            {\r\n");
 if (string.IsNullOrWhiteSpace(skin.unlockableName)) { 
            this.Write("                    skin.unlockableDef = null;\r\n");
 } else { 
            this.Write("                    skin.unlockableDef = ContentManager.unlockableDefs.FirstOrDef" +
                    "ault(def => def.cachedName == \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.unlockableName));
            this.Write("\");\r\n");
 } 
            this.Write("                });\r\n                TryCatchThrow(\"Game Object Activations\", () " +
                    "=>\r\n                {\r\n");
 if (skin.gameObjectActivations.Count == 0) { 
            this.Write("                    skin.gameObjectActivations = Array.Empty<SkinDef.GameObjectAc" +
                    "tivation>();\r\n");
 } else { 
            this.Write("                    skin.gameObjectActivations = new SkinDef.GameObjectActivation" +
                    "[]\r\n                    {\r\n");
 foreach (var activation in skin.gameObjectActivations) { 
            this.Write("                        new SkinDef.GameObjectActivation\r\n                       " +
                    " {\r\n");
 if (activation.accessType == GameObjectActivationAccessType.ByRendererIndex) { 
            this.Write("                            gameObject = renderers[");
            this.Write(this.ToStringHelper.ToStringWithCulture(activation.rendererIndex));
            this.Write("].gameObject,\r\n");
 } else if (activation.accessType == GameObjectActivationAccessType.ByRendererName) { 
            this.Write("                            gameObject = renderers.First(r => r.name == \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(activation.rendererName));
            this.Write("\").gameObject,\r\n");
 } else if (activation.accessType == GameObjectActivationAccessType.ByPath) { 
            this.Write("                            gameObject = mdl.transform.Find(\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(activation.path));
            this.Write("\").gameObject,\r\n");
 } 
            this.Write("                            shouldActivate = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(activation.shouldActivate.ToLiteral()));
            this.Write("\r\n                        },\r\n");
 } 
            this.Write("                    };\r\n");
 } 
            this.Write("                });\r\n                TryCatchThrow(\"Renderer Infos\", () =>\r\n     " +
                    "           {\r\n");
 if (skin.rendererInfos.Count == 0) { 
            this.Write("                    skin.rendererInfos = Array.Empty<CharacterModel.RendererInfo>" +
                    "();\r\n");
 } else { 
            this.Write("                    skin.rendererInfos = new CharacterModel.RendererInfo[]\r\n     " +
                    "               {\r\n");
 foreach (var rendererInfo in skin.rendererInfos) { 
            this.Write("                        new CharacterModel.RendererInfo\r\n                        " +
                    "{\r\n");
 if (rendererInfo.defaultMaterial == null) { 
            this.Write("                            defaultMaterial = null,\r\n");
 } else { 
            this.Write("                            defaultMaterial = assetBundle.LoadAsset<Material>(@\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(AssetsInfo.materialPaths[rendererInfo.defaultMaterial]));
            this.Write("\"),\r\n");
 } 
            this.Write("                            defaultShadowCastingMode = UnityEngine.Rendering.Shad" +
                    "owCastingMode.");
            this.Write(this.ToStringHelper.ToStringWithCulture(Enum.GetName(typeof(ShadowCastingMode), rendererInfo.defaultShadowCastingMode)));
            this.Write(",\r\n                            ignoreOverlays = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(rendererInfo.ignoreOverlays.ToLiteral()));
            this.Write(",\r\n");
 if (rendererInfo.rendererReference.accessType == AccessType.ByIndex) { 
            this.Write("                            renderer = renderers[");
            this.Write(this.ToStringHelper.ToStringWithCulture(rendererInfo.rendererReference.index));
            this.Write("]\r\n");
 } else { 
            this.Write("                            renderer = renderers.First(r => r.name == \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(rendererInfo.rendererReference.name));
            this.Write("\")\r\n");
 } 
            this.Write("                        },\r\n");
 } 
            this.Write("                    };\r\n");
 } 
            this.Write("                });\r\n                TryCatchThrow(\"Mesh Replacements\", () =>\r\n  " +
                    "              {\r\n");
 if (skin.meshReplacements.Count == 0) { 
            this.Write("                    skin.meshReplacements = Array.Empty<SkinDef.MeshReplacement>(" +
                    ");\r\n");
 } else { 
            this.Write("                    skin.meshReplacements = new SkinDef.MeshReplacement[]\r\n      " +
                    "              {\r\n");
 foreach (var replacement in skin.meshReplacements) { 
            this.Write("                        new SkinDef.MeshReplacement\r\n                        {\r\n");
 if (replacement.mesh == null) { 
            this.Write("                            mesh = null,\r\n");
 } else { 
            this.Write("                            mesh = assetBundle.LoadAsset<Mesh>(@\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(AssetsInfo.meshPaths[replacement.mesh]));
            this.Write("\"),\r\n");
 } 
if (replacement.rendererReference.accessType == AccessType.ByIndex) { 
            this.Write("                            renderer = renderers[");
            this.Write(this.ToStringHelper.ToStringWithCulture(replacement.rendererReference.index));
            this.Write("]\r\n");
 } else { 
            this.Write("                            renderer = renderers.First(r => r.name == \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(replacement.rendererReference.name));
            this.Write("\")\r\n");
 } 
            this.Write("                        },\r\n");
 } 
            this.Write("                    };\r\n");
 } 
            this.Write("                });\r\n                TryCatchThrow(\"Minion Skin Replacements\", ()" +
                    " =>\r\n                {\r\n");
 if (skin.minionSkinReplacements.Count == 0) { 
            this.Write("                    skin.minionSkinReplacements = Array.Empty<SkinDef.MinionSkinR" +
                    "eplacement>();\r\n");
 } else { 
            this.Write("                    skin.minionSkinReplacements = new SkinDef.MinionSkinReplaceme" +
                    "nt[]\r\n                    {\r\n");
 foreach (var replacement in skin.minionSkinReplacements) {
    if (!replacement.findSkinByReference && replacement.skin == null) {
        continue;
    } 
            this.Write("                        new SkinDef.MinionSkinReplacement\r\n                      " +
                    "  {\r\n                            minionBodyPrefab = BodyCatalog.FindBodyPrefab(@" +
                    "\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(replacement.bodyName));
            this.Write("\"),\r\n");
 if (replacement.findSkinByReference) { 
    if (replacement.reference.accessType == AccessType.ByIndex) { 
            this.Write("                            minionSkin = BodyCatalog.GetBodySkins(BodyCatalog.Fin" +
                    "dBodyIndex(@\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(replacement.bodyName));
            this.Write("\"))[");
            this.Write(this.ToStringHelper.ToStringWithCulture(replacement.reference.index));
            this.Write("]\r\n");
 } else { 
            this.Write("                            minionSkin = BodyCatalog.GetBodySkins(BodyCatalog.Fin" +
                    "dBodyIndex(@\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(replacement.bodyName));
            this.Write("\")).First(s => s.name == @\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(replacement.reference.name));
            this.Write("\")\r\n    ");
 }
} else { 
            this.Write("                            minionSkin = BodyCatalog.GetBodySkins(BodyCatalog.Fin" +
                    "dBodyIndex(@\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(replacement.bodyName));
            this.Write("\")).First(s => s.name == @\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(replacement.skin.name));
            this.Write("\")\r\n");
 } 
            this.Write("                        },\r\n");
 } 
            this.Write("                    };\r\n");
 } 
            this.Write("                });\r\n                TryCatchThrow(\"Projectile Ghost Replacements" +
                    "\", () =>\r\n                {\r\n");
 if (skin.projectileGhostReplacements.Count == 0) { 
            this.Write("                    skin.projectileGhostReplacements = Array.Empty<SkinDef.Projec" +
                    "tileGhostReplacement>();\r\n");
 } else { 
            this.Write("                    skin.projectileGhostReplacements = new SkinDef.ProjectileGhos" +
                    "tReplacement[]\r\n                    {\r\n");
 foreach (var replacement in skin.projectileGhostReplacements) { 
            this.Write("                        new SkinDef.ProjectileGhostReplacement\r\n                 " +
                    "       {\r\n                            projectilePrefab = Addressables.LoadAssetA" +
                    "sync<GameObject>(@\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(replacement.projectilePath));
            this.Write("\").WaitForCompletion(),\r\n");
 if (replacement.useResourcesPath) { 
            this.Write("                            projectileGhostReplacementPrefab = Addressables.LoadA" +
                    "ssetAsync<GameObject>(@\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(replacement.projectileGhostPath));
            this.Write("\".WaitForCompletion())\r\n");
 } else { 
            this.Write("                            projectileGhostReplacementPrefab = assetBundle.LoadAs" +
                    "set<GameObject>(@\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(AssetsInfo.gameObjectPaths[replacement.projectileGhost]));
            this.Write("\")\r\n");
 } 
            this.Write("                        },\r\n");
 } 
            this.Write("                    };\r\n");
 } 
            this.Write(@"                });

                Array.Resize(ref skinController.skins, skinController.skins.Length + 1);
                skinController.skins[skinController.skins.Length - 1] = skin;

                BodyCatalog.skins[(int)BodyCatalog.FindBodyIndex(bodyPrefab)] = skinController.skins;
                ");
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.bodyName.ToUpperCamelCase()));
            this.Write(this.ToStringHelper.ToStringWithCulture(skin.name.ToUpperCamelCase()));
            this.Write(@"SkinAdded(skin, bodyPrefab);
            }
            catch (FieldException e)
            {
                InstanceLogger.LogWarning($""Failed to add \""{skinName}\"" skin to \""{bodyName}\"""");
                InstanceLogger.LogWarning($""Field causing issue: {e.Message}"");
                InstanceLogger.LogError(e.InnerException);
            }
            catch (Exception e)
            {
                InstanceLogger.LogWarning($""Failed to add \""{skinName}\"" skin to \""{bodyName}\"""");
                InstanceLogger.LogError(e);
            }
        }
");
 } 
            this.Write(@"
        private static void TryCatchThrow(string message, Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                throw new FieldException(message, e);
            }
        }

        private class FieldException : Exception
        {
            public FieldException(string message, Exception innerException) : base(message, innerException) { }
        }
    }
}");
            return this.GenerationEnvironment.ToString();
        }
    }
}
