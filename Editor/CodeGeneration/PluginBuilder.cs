using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoRSkinBuilder.Data;
using UnityEngine;
using UnityEngine.Rendering;

namespace RoRSkinBuilder.CodeGeneration
{
    public class PluginBuilder
    {
        private readonly CodeBuilder code = new CodeBuilder();
        private SkinModInfo SkinModInfo { get; }
        private AssetsInfo AssetsInfo { get; }
        private List<SkinDefinition> ReorderedSkins { get; }
        private Dictionary<string, DependencyType> DistinctDependencies { get; }

        public PluginBuilder(AssetsInfo assetsInfo)
        {
            SkinModInfo = assetsInfo.skinModInfo;
            AssetsInfo = assetsInfo;
            ReorderedSkins = ReorderSkins(SkinModInfo.skins);
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

        public string Build()
        {
            code.A("using BepInEx;")
                .L("using BepInEx.Logging;")
                .L("using RoR2;")
                .L("using System;")
                .L("using System.Collections.Generic;")
                .L("using System.Reflection;")
                .L("using System.Linq;")
                .L("using UnityEngine;")
                .L("using System.Security.Permissions;")
                .L("using MonoMod.RuntimeDetour.HookGen;")
                .L("using RoR2.ContentManagement;")
                .L("using UnityEngine.AddressableAssets;")
                .L("using RoR2.Projectile;")
                .L("using BepInEx.Configuration;")
                .L("using static ").A(AssetsInfo.uccModName).A(".Helpers;")
                .L()
                .L("#pragma warning disable CS0618 // Type or member is obsolete")
                .L("[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]")
                .L("#pragma warning restore CS0618 // Type or member is obsolete")
                .L("namespace ").A(AssetsInfo.uccModName)
                .O();

            foreach (var dependency in DistinctDependencies)
            {
                code.L("[BepInDependency(\"").A(dependency.Key).A("\", BepInDependency.DependencyFlags.").A(Enum.GetName(typeof(DependencyType), dependency.Value)).A(")]");
            }

            code.L("[BepInPlugin(\"com.").A(SkinModInfo.author.StripSpaces()).A(".").A(AssetsInfo.uccModName).A("\", ").Literal(SkinModInfo.modName).A(", ").Literal(SkinModInfo.version).A(")]")
                .L("public partial class ").A(AssetsInfo.uccModName).A("Plugin : BaseUnityPlugin")
                .O()
                .L("internal static ").A(AssetsInfo.uccModName).A("Plugin Instance { get; private set; }")
                .L("internal static ManualLogSource InstanceLogger => Instance?.Logger;")
                .L()
                .L("private static AssetBundle assetBundle;");

            if (AssetsInfo.materialsWithRoRShader.Count != 0)
            {
                code.L("private static readonly List<Material> materialsWithRoRShader = new List<Material>();");
            }

            foreach (var skin in ReorderedSkins)
            {
                if (skin.config.generateEnableConfig)
                {
                    code.L("public static ConfigEntry<bool> ").A(skin.name.ToUpperCamelCase()).A("EnabledConfig;");
                }
                foreach (var config in skin.config.configEntries)
                {
                    code.L("public static ConfigEntry<bool> ").A(skin.name.ToUpperCamelCase()).A(config.name.ToUpperCamelCase()).A("Config;");
                }
            }

            code.L()
                .L("private void Start()")
                .O()
                .L("Instance = this;")
                .L()
                .L("BeforeStart();")
                .L();

            foreach (var skin in ReorderedSkins)
            {
                if (skin.config.generateEnableConfig)
                {
                    code.L(skin.name.ToUpperCamelCase()).A("EnabledConfig = Instance.Config.Bind(").Literal(skin.name.ToUpperCamelCase()).A(", \"Enabled\", ").Literal(skin.config.enableConfigDefaultValue).A(");");
                }
                foreach (var config in skin.config.configEntries)
                {
                    code.L(skin.name.ToUpperCamelCase()).A(config.name.ToUpperCamelCase()).A("Config = Instance.Config.Bind(").Literal(skin.name.ToUpperCamelCase()).A(", ").Literal(config.name).A(", ").Literal(config.enabledByDefault);
                    if (!string.IsNullOrEmpty(config.description))
                    {
                        code.A(", ").Literal(config.description);
                    }
                    code.A(");");
                }
            }

            code.L()
                .L("using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(\"").A(AssetsInfo.uccModName).A(".").A(AssetsInfo.assetBundleName).A("\"))")
                .O()
                .L("assetBundle = AssetBundle.LoadFromStream(assetStream);")
                .C()
                .L()
                .L("BodyCatalog.availability.CallWhenAvailable(BodyCatalogInit);")
                .L("SurvivorCatalog.availability.CallWhenAvailable(SurvivorCatalogInit);")
                .L("HookEndpointManager.Add(typeof(Language).GetMethod(nameof(Language.LoadStrings)), (Action<Action<Language>, Language>)LanguageLoadStrings);")
                .L();

            if (AssetsInfo.materialsWithRoRShader.Count != 0)
            {
                code.L("ReplaceShaders();");
            }

            code.L("AfterStart();")
                .C()
                .L()
                .L("partial void BeforeStart();")
                .L("partial void AfterStart();")
                .L("static partial void BeforeBodyCatalogInit();")
                .L("static partial void AfterBodyCatalogInit();")
                .L("static partial void BeforeSurvivorCatalogInit();")
                .L("static partial void AfterSurvivorCatalogInit();");

            BuildReplaceShaders();
            BuildLanguageLoadStrings();

            code.L()
                .L("private static void BodyCatalogInit()")
                .O()
                .L("BeforeBodyCatalogInit();")
                .L();
            foreach (var skin in ReorderedSkins)
            {
                code.L("Add").A(skin.NameForMethods).A("Skin();");
            }
            code.L()
                .L("AfterBodyCatalogInit();")
                .C();

            code.L()
                .L("private static void SurvivorCatalogInit()")
                .O()
                .L("BeforeSurvivorCatalogInit();")
                .L();
            foreach (var skin in ReorderedSkins)
            {
                if (skin.createLobbySkin)
                {
                    code.L("Add").A(skin.NameForMethods).A("LobbySkin();");
                }
            }
            code.L()
                .L("AfterSurvivorCatalogInit();")
                .C();

            foreach (var skin in ReorderedSkins)
            {
                BuildAddSkin(skin);
                BuildAddLobbySkin(skin);
            }

            code.C()
                .C();

            return code.Build();
        }

        private void BuildAddLobbySkin(SkinDefinition skin)
        {
            if (!skin.createLobbySkin)
            {
                return;
            }

            code.L()
                .L("static partial void ").A(skin.NameForMethods).A("LobbySkinAdded(SkinDef skinDef, GameObject displayPrefab);");

            if (skin.lobbySkinOverride)
            {
                code.L();
                BuildCreateSkin(skin.lobbySkinOverride, true, skin.bodyName, skin.name, skin.CreateNameToken(SkinModInfo.author), skin);
            }

            code.L()
                .L("private static void Add").A(skin.NameForMethods).A("LobbySkin()")
                .O();
            if (!string.IsNullOrWhiteSpace(skin.modDependency.value) && skin.modDependency.type == DependencyType.SoftDependency)
            {
                code.L("if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(").Literal(skin.modDependency.value).A("))")
                    .O()
                    .L("return;")
                    .C();
            }
            if (skin.config.generateEnableConfig)
            {
                code.L("if (!").A(skin.name.ToUpperCamelCase()).A("EnabledConfig.Value)")
                    .O()
                    .L("return;")
                    .C();
            }

            code.L("var bodyName = ").Literal(skin.bodyName).A(";")
                .L("var skinName = ").Literal(skin.name).A(";")
                .L("try")
                .O()
                .L("var bodyIndex = BodyCatalog.FindBodyIndex(bodyName);")
                .L("if (bodyIndex == BodyIndex.None)")
                .O()
                .L("InstanceLogger.LogWarning($\"Failed to add \\\"{skinName}\\\" lobby skin because \\\"{bodyName}\\\" doesn't exist\");")
                .L("return;")
                .C()
                .L()
                .L("var survivorIndex = SurvivorCatalog.GetSurvivorIndexFromBodyIndex(bodyIndex);")
                .L("if (survivorIndex == SurvivorIndex.None)")
                .O()
                .L("InstanceLogger.LogWarning($\"Failed to add \\\"{skinName}\\\" lobby skin to \\\"{bodyName}\\\" because SurvivorDef doesn't exist\");")
                .L("return;")
                .C()
                .L()
                .L("var survivorDef = SurvivorCatalog.GetSurvivorDef(survivorIndex);")
                .L("var displayPrefab = survivorDef.displayPrefab;")
                .L("var skinController = displayPrefab.GetComponentInChildren<ModelSkinController>();")
                .L("if (!skinController)")
                .O()
                .L("InstanceLogger.LogWarning($\"Failed to add \\\"{skinName}\\\" lobby skin to \\\"{bodyName}\\\" because it doesn't have \\\"ModelSkinController\\\" component\");")
                .L("return;")
                .C()
                .L()
                .L("var mdl = skinController.gameObject;");

            if (skin.lobbySkinOverride)
            {
                code.L("var skin = Create").A(skin.NameForMethods).A("LobbySkin(mdl, skinController);");
            }
            else
            {
                code.L("var skin = Create").A(skin.NameForMethods).A("Skin(mdl, skinController);");
            }

            code.L()
                .L("Array.Resize(ref skinController.skins, skinController.skins.Length + 1);")
                .L("skinController.skins[skinController.skins.Length - 1] = skin;")
                .L()
                .L(skin.NameForMethods).A("LobbySkinAdded(skin, displayPrefab);")
                .C()
                .L("catch (FieldException e)")
                .O()
                .L("if (e.InnerException is ElementException ie)")
                .O()
                .L("InstanceLogger.LogWarning($\"Failed to add \\\"{skinName}\\\" lobby skin to \\\"{bodyName}\\\"\");")
                .L("InstanceLogger.LogWarning($\"Field causing issue: {e.Message}, element: {ie.Index}\");")
                .L("InstanceLogger.LogWarning(ie.Message);")
                .L("InstanceLogger.LogError(e.InnerException);")
                .C()
                .L("else")
                .O()
                .L("InstanceLogger.LogWarning($\"Failed to add \\\"{skinName}\\\" lobby skin to \\\"{bodyName}\\\"\");")
                .L("InstanceLogger.LogWarning($\"Field causing issue: {e.Message}\");")
                .L("InstanceLogger.LogError(e.InnerException);")
                .C()
                .C()
                .L("catch (Exception e)")
                .O()
                .L("InstanceLogger.LogWarning($\"Failed to add \\\"{skinName}\\\" lobby skin to \\\"{bodyName}\\\"\");")
                .L("InstanceLogger.LogError(e);")
                .C()
                .C();
        }

        private void BuildAddSkin(SkinDefinition skin)
        {
            code.L()
                .L("static partial void ").A(skin.NameForMethods).A("SkinAdded(SkinDef skinDef, GameObject bodyPrefab);")
                .L();

            BuildCreateSkin(skin, false, skin.bodyName, skin.name, skin.CreateNameToken(SkinModInfo.author), skin);

            code.L()
                .L("private static void Add").A(skin.NameForMethods).A("Skin()")
                .O();
            if (!string.IsNullOrWhiteSpace(skin.modDependency.value) && skin.modDependency.type == DependencyType.SoftDependency)
            {
                code.L("if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(").Literal(skin.modDependency.value).A("))")
                    .O()
                    .L("return;")
                    .C();
            }
            if (skin.config.generateEnableConfig)
            {
                code.L("if (!").A(skin.name.ToUpperCamelCase()).A("EnabledConfig.Value)")
                    .O()
                    .L("return;")
                    .C();
            }
            code.L("var bodyName = ").Literal(skin.bodyName).A(";")
                .L("var skinName = ").Literal(skin.name).A(";")
                .L("try")
                .O()
                .L("var bodyPrefab = BodyCatalog.FindBodyPrefab(bodyName);")
                .L("if (!bodyPrefab)")
                .O()
                .L("InstanceLogger.LogWarning($\"Failed to add \\\"{skinName}\\\" skin because \\\"{bodyName}\\\" doesn't exist\");")
                .L("return;")
                .C()
                .L()
                .L("var modelLocator = bodyPrefab.GetComponent<ModelLocator>();")
                .L("if (!modelLocator)")
                .O()
                .L("InstanceLogger.LogWarning($\"Failed to add \\\"{skinName}\\\" skin to \\\"{bodyName}\\\" because it doesn't have \\\"ModelLocator\\\" component\");")
                .L("return;")
                .C()
                .L()
                .L("var mdl = modelLocator.modelTransform.gameObject;")
                .L("var skinController = mdl ? mdl.GetComponent<ModelSkinController>() : null;")
                .L("if (!skinController)")
                .O()
                .L("InstanceLogger.LogWarning($\"Failed to add \\\"{skinName}\\\" skin to \\\"{bodyName}\\\" because it doesn't have \\\"ModelSkinController\\\" component\");")
                .L("return;")
                .C()
                .L()
                .L("var skin = Create").A(skin.NameForMethods).A("Skin(mdl, skinController);")
                .L()
                .L("Array.Resize(ref skinController.skins, skinController.skins.Length + 1);")
                .L("skinController.skins[skinController.skins.Length - 1] = skin;")
                .L()
                .L(skin.NameForMethods).A("SkinAdded(skin, bodyPrefab);")
                .C()
                .L("catch (FieldException e)")
                .O()
                .L("if (e.InnerException is ElementException ie)")
                .O()
                .L("InstanceLogger.LogWarning($\"Failed to add \\\"{skinName}\\\" skin to \\\"{bodyName}\\\"\");")
                .L("InstanceLogger.LogWarning($\"Field causing issue: {e.Message}, element: {ie.Index}\");")
                .L("InstanceLogger.LogWarning(ie.Message);")
                .L("InstanceLogger.LogError(e.InnerException);")
                .C()
                .L("else")
                .O()
                .L("InstanceLogger.LogWarning($\"Failed to add \\\"{skinName}\\\" skin to \\\"{bodyName}\\\"\");")
                .L("InstanceLogger.LogWarning($\"Field causing issue: {e.Message}\");")
                .L("InstanceLogger.LogError(e.InnerException);")
                .C()
                .C()
                .L("catch (Exception e)")
                .O()
                .L("InstanceLogger.LogWarning($\"Failed to add \\\"{skinName}\\\" skin to \\\"{bodyName}\\\"\");")
                .L("InstanceLogger.LogError(e);")
                .C()
                .C();
        }

        private void BuildCreateSkin(ISkinParams skin, bool isLobby, string bodyName, string name, string nameToken, SkinDefinition parentSkin)
        {
            code.L("private static SkinDef Create").A(bodyName.ToUpperCamelCase()).A(name.ToUpperCamelCase());
            if (isLobby)
            {
                code.A("Lobby");
            }

            code.A("Skin(GameObject mdl, ModelSkinController skinController)")
                .O()
                .L("var renderers = mdl.GetComponentsInChildren<Renderer>(true);")
                .L("var lights = mdl.GetComponentsInChildren<Light>(true);");

            if (skin.GameObjectActivations.Any(a => a.spawnPrefabOnModelObject && a.addBonesToRenderers))
            {
                code
                    .L("var skinnedRendererPaths = new List<string>();")
                    .L("foreach (var renderer in renderers)")
                    .O()
                        .L("if (renderer is SkinnedMeshRenderer)")
                        .O()
                            .L("skinnedRendererPaths.Add(Util.BuildPrefabTransformPath(mdl.transform, renderer.transform, false, false));")
                        .C()
                    .C();
            }

            code.L()
                .L("var skin = ScriptableObject.CreateInstance<SkinDef>();")
                .L("var skinParams = ScriptableObject.CreateInstance<SkinDefParams>();")
                .L("skin.skinDefParams = skinParams;")
                .L("skin.name = ").Literal(name).A(";")
                .L("skin.nameToken = ").Literal(nameToken).A(";")
                .L("skin.rootObject = mdl;");

            BuildSkinIcon(parentSkin);
            BuildSkinUnlockable(parentSkin);
            BuildSkinBaseSkins(skin);
            BuildSkinGameObjectActivations(skin);
            BuildSkinRendererInfos(skin);
            BuildSkinMeshReplacements(skin);
            BuildSkinLightInfos(skin);
            BuildSkinMinionSkinReplacements(skin);
            BuildSkinProjectileGhostReplacements(skin);

            code.L("return skin;")
                .C();
        }

        private void BuildSkinProjectileGhostReplacements(ISkinParams skin)
        {
            BuildTryCatchThrowArray(
                skin,
                "Projectile Ghost Replacements",
                "skinParams.projectileGhostReplacements",
                "SkinDefParams.ProjectileGhostReplacement",
                skin.ProjectileGhostReplacements,
                (replacement, i) =>
                {
                    code.A("new SkinDefParams.ProjectileGhostReplacement")
                        .O()
                        .L("projectilePrefab = Addressables.LoadAssetAsync<GameObject>(@\"")
                        .A(replacement.projectilePath)
                        .A("\").WaitForCompletion(),");
                    if (replacement.useAddressablesPath)
                    {
                        code.L("projectileGhostReplacementPrefab = Addressables.LoadAssetAsync<GameObject>(@\"").A(replacement.projectileGhostPath).A("\").WaitForCompletion(),");
                    }
                    else
                    {
                        code.L("projectileGhostReplacementPrefab = assetBundle.LoadAsset<GameObject>(@\"").A(AssetsInfo.uniqueProjectileGhosts[replacement.projectileGhost]).A("\")")
                            .O(".WithTryComponent<ProjectileGhostController>(),").C("");
                    }
                    code.C();
                });
        }

        private void BuildSkinMinionSkinReplacements(ISkinParams skin)
        {
            BuildTryCatchThrowArray(
                skin,
                "Minion Skin Replacements",
                "skinParams.minionSkinReplacements",
                "SkinDefParams.MinionSkinReplacement",
                skin.MinionSkinReplacements,
                (replacement, i) =>
                {
                    code.A("new SkinDefParams.MinionSkinReplacement")
                        .O()
                        .L("minionBodyPrefab = BodyCatalog.FindBodyPrefab(@\"").A(replacement.bodyName).A("\"),");
                    if (replacement.findSkinByReference)
                    {
                        if (replacement.reference.accessType == AccessType.ByIndex)
                        {
                            code.L("minionSkin = ThrowIfOutOfBounds(").A(i).A(", \"Index ").A(replacement.reference.index).A(" is out of bounds of skins array for body \\\"").A(replacement.bodyName).A("\\\"\", SkinCatalog.FindSkinsForBody(ThrowIfEquals(").A(i).A(", \"Body \\\"").A(replacement.bodyName).A("\\\" doesn't exist\", BodyCatalog.FindBodyIndex(").Literal(replacement.bodyName).A("), BodyIndex.None)), ").A(replacement.reference.index).A("),");
                        }
                        else
                        {
                            code.L("minionSkin = ThrowIfNull(").A(i).A(", \"There is no skin with the name \\\"").A(replacement.reference.name).A("\\\" for body \\\"").A(replacement.bodyName).A("\\\"\", SkinCatalog.FindSkinsForBody(ThrowIfEquals(").A(i).A(", \"Body \\\"").A(replacement.bodyName).A("\\\" doesn't exist\", BodyCatalog.FindBodyIndex(").Literal(replacement.bodyName).A("), BodyIndex.None)).FirstOrDefault(s => s.name == ").Literal(replacement.reference.name).A(")),");
                        }
                    }
                    else
                    {
                        code.L("minionSkin = ThrowIfNull(").A(i).A(", \"There is no skin with the name \\\"").A(replacement.skin.name).A("\\\" for body \\\"").A(replacement.bodyName).A("\\\"\", SkinCatalog.FindSkinsForBody(ThrowIfEquals(").A(i).A(", \"Body \\\"").A(replacement.bodyName).A("\\\" doesn't exist\", BodyCatalog.FindBodyIndex(").Literal(replacement.bodyName).A("), BodyIndex.None)).FirstOrDefault(s => s.name == ").Literal(replacement.skin.name).A(")),");
                    }
                    code.C();
                });
        }

        private void BuildSkinLightInfos(ISkinParams skin)
        {
            BuildTryCatchThrowArray(
                skin,
                "Light Infos",
                "skinParams.lightReplacements",
                "CharacterModel.LightInfo",
                skin.LightReplacements,
                (replacement, i) =>
                {
                    code.A("new CharacterModel.LightInfo")
                        .O()
                        .L("defaultColor = ").A(replacement.defaultColor.ToNewString()).A(",");
                    if (replacement.lightReference.accessType == ComponentAccessType.ByName)
                    {
                        code.L("light = ThrowIfNull(").A(i).A(", \"There is no light with the name \\\"").A(replacement.lightReference.name).A("\\\"\", lights.FirstOrDefault(l => l.name == ").Literal(replacement.lightReference.name).A(")),");
                    }
                    else if (replacement.lightReference.accessType == ComponentAccessType.ByPath)
                    {
                        code.L("light = ThrowIfNull(").A(i).A(", \"There is no light at path \\\"").A(replacement.lightReference.path).A("\\\"\", mdl.transform.Find(").Literal(replacement.lightReference.path).A(")).GetComponent<Light>(),");
                    }
                    code.C();
                });
        }

        private void BuildSkinMeshReplacements(ISkinParams skin)
        {
            BuildTryCatchThrowArray(
                skin,
                "MeshReplacements",
                "skinParams.meshReplacements",
                "SkinDefParams.MeshReplacement",
                skin.MeshReplacements,
                (replacement, i) =>
                {
                    code.A("new SkinDefParams.MeshReplacement")
                        .O();
                    if (replacement.mesh == null)
                    {
                        code.L("mesh = null,");
                    }
                    else
                    {
                        code.L("mesh = assetBundle.LoadAsset<Mesh>(@\"").A(AssetsInfo.meshPaths[replacement.mesh]).A("\"),");
                    }
                    if (replacement.rendererReference.accessType == ComponentAccessType.ByName)
                    {
                        code.L("renderer = ThrowIfNull(").A(i).A(", \"There is no renderer with the name \\\"").A(replacement.rendererReference.name).A("\\\"\", renderers.FirstOrDefault(r => r.name == ").Literal(replacement.rendererReference.name).A(")),");
                    }
                    else if (replacement.rendererReference.accessType == ComponentAccessType.ByPath)
                    {
                        code.L("renderer = ThrowIfNull(").A(i).A(", \"There is no renderer at path \\\"").A(replacement.rendererReference.path).A("\\\"\", mdl.transform.Find(").Literal(replacement.rendererReference.path).A(")).GetComponent<Renderer>(),");
                    }
                    code.C();
                });
        }

        private void BuildSkinRendererInfos(ISkinParams skin)
        {
            BuildTryCatchThrowArray(
                skin,
                "Renderer Infos",
                "skinParams.rendererInfos",
                "CharacterModel.RendererInfo",
                skin.RendererInfos,
                (rendererInfo, i) =>
                {
                    code.A("new CharacterModel.RendererInfo")
                        .O();
                    if (rendererInfo.defaultMaterial == null)
                    {
                        code.L("defaultMaterial = null,");
                    }
                    else
                    {
                        code.L("defaultMaterial = assetBundle.LoadAsset<Material>(@\"").A(AssetsInfo.materialPaths[rendererInfo.defaultMaterial]).A("\"),");
                    }
                    code.L("defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.").A(Enum.GetName(typeof(ShadowCastingMode), rendererInfo.defaultShadowCastingMode)).A(",")
                        .L("ignoreOverlays = ").Literal(rendererInfo.ignoreOverlays).A(",");
                    if (rendererInfo.rendererReference.accessType == ComponentAccessType.ByName)
                    {
                        code.L("renderer = ThrowIfNull(").A(i).A(", \"There is no renderer with the name \\\"").A(rendererInfo.rendererReference.name).A("\\\"\", renderers.FirstOrDefault(r => r.name == ").Literal(rendererInfo.rendererReference.name).A(")),");
                    }
                    else if (rendererInfo.rendererReference.accessType == ComponentAccessType.ByPath)
                    {
                        code.L("renderer = ThrowIfNull(").A(i).A(", \"There is no renderer at path \\\"").A(rendererInfo.rendererReference.path).A("\\\"\", mdl.transform.Find(").Literal(rendererInfo.rendererReference.path).A(")).GetComponent<Renderer>(),");
                    }
                    code.L("hideOnDeath = ").Literal(rendererInfo.hideOnDeath).A(",")
                        .L("ignoresMaterialOverrides = ").Literal(rendererInfo.ignoresMaterialOverrides).A(",")
                        .C();
                });
        }

        private void BuildSkinGameObjectActivations(ISkinParams skin)
        {
            BuildTryCatchThrowArray(
                skin,
                "Game Object Activations",
                "skinParams.gameObjectActivations",
                "SkinDefParams.GameObjectActivation",
                skin.GameObjectActivations,
                (activation, i) =>
                {
                    code.A("new SkinDefParams.GameObjectActivation")
                        .O();
                    if (activation.accessType == GameObjectActivationAccessType.ByRendererName)
                    {
                        code.L("gameObject = ThrowIfNull(").A(i).A(", \"There is no renderer with the name \\\"").A(activation.rendererName).A("\\\"\", renderers.FirstOrDefault(r => r.name == ").Literal(activation.rendererName).A(")).gameObject,");
                    }
                    else if (activation.accessType == GameObjectActivationAccessType.ByPath)
                    {
                        code.L("gameObject = ThrowIfNull(").A(i).A(", \"There is no renderer at path \\\"").A(activation.path).A("\\\"\", mdl.transform.Find(").Literal(activation.path).A(")).gameObject,");
                    }
                    if (activation.spawnPrefabOnModelObject)
                    {
                        code.L("shouldActivate = false,")
                            .L("spawnPrefabOnModelObject = true,")
                            .L("localPosition = ").A(activation.localPosition.ToNewString()).A(",")
                            .L("localRotation = ").A(activation.localRotation.ToNewString()).A(",")
                            .L("localScale = ").A(activation.localScale.ToNewString()).A(",");
                        if (activation.useAddressablesKey)
                        {
                            code.L("prefab = ClonePrefab(Addressables.LoadAssetAsync<GameObject>(@\"").A(activation.prefabKey).A("\").WaitForCompletion())");
                        }
                        else
                        {
                            code.L("prefab = ClonePrefab(assetBundle.LoadAsset<GameObject>(@\"").A(AssetsInfo.uniqueGameObjectActivations[activation.prefab]).A("\"))")
                                .O("");
                            if (activation.addBonesToRenderers)
                            {
                                code.L(".WithComponent<InsertBones>(c => ")
                                    .O()
                                    .L("c.rendererPaths.AddRange(skinnedRendererPaths);")
                                    .L("c.afterPath = @\"").A(FindAfterPath(skin, activation)).A("\";")
                                    .L("var t = c.transform;");
                                foreach (var path in GetPrefabBonePaths(skin, activation))
                                {
                                    code.L("c.newBones.Add(").A(path).A(");");
                                }

                                code.C().A(")");
                            }
                            if (activation.dynamicBonesColliderPaths.Count > 0)
                            {
                                code.L(".WithComponent<AddDynamicColliders>(c => ")
                                    .O();
                                foreach (var path in activation.dynamicBonesColliderPaths)
                                {
                                    code.L("c.paths.Add(@\"").A(path).A("\");");
                                }
                                code.C().A(")");
                            }

                            code.C("");
                        }
                    }
                    else
                    {
                        code.L("shouldActivate = ").Literal(activation.shouldActivate).A(",")
                            .L("spawnPrefabOnModelObject = false,");
                    }
                    code.C();
                });
        }

        private IEnumerable<string> GetPrefabBonePaths(ISkinParams skin, GameObjectActivation activation)
        {
            var parentTransform = skin.ReferenceModel.transform.Find(activation.path);
            var transform = parentTransform.Find(activation.prefab.name);
            var renderers = skin.ReferenceModel.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var renderer in renderers)
            {
                var index = Array.IndexOf(renderer.bones, transform);
                if (index != -1)
                {
                    yield return "t";
                    index++;
                    var path = Helpers.BuildGetPath(renderer.bones[index], transform);
                    while (!string.IsNullOrEmpty(path))
                    {
                        yield return path;
                        index++;
                        if (index >= renderer.bones.Length)
                        {
                            break;
                        }
                        path = Helpers.BuildGetPath(renderer.bones[index], transform);
                    }
                    yield break;
                }
            }

            throw new KeyNotFoundException();
        }

        private string FindAfterPath(ISkinParams skin, GameObjectActivation activation)
        {
            var parentTransform = skin.ReferenceModel.transform.Find(activation.path);
            var transform = parentTransform.Find(activation.prefab.name);
            var renderers = skin.ReferenceModel.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var renderer in renderers)
            {
                var index = Array.IndexOf(renderer.bones, transform);
                if (index != -1)
                {
                    var previousTransform = renderer.bones[index - 1];
                    return Helpers.BuildPath(previousTransform);
                }
            }

            throw new KeyNotFoundException();
        }

        private void BuildSkinUnlockable(SkinDefinition skin)
        {
            code.L("TryCatchThrow(\"Unlockable Name\", () =>")
                .O();
            if (string.IsNullOrWhiteSpace(skin.unlockableName))
            {
                code.L("skin.unlockableDef = null;");
            }
            else
            {
                code.L("skin.unlockableDef = ContentManager.unlockableDefs.FirstOrDefault(def => def.cachedName == ").Literal(skin.unlockableName).A(");");
            }
            code.C().A(");");
        }

        private void BuildSkinBaseSkins(ISkinParams skin)
        {
            code.L("TryCatchThrow(\"Base Skins\", () =>")
                .O();
            if (skin.BaseSkins.Count == 0)
            {
                code.L("skin.baseSkins = Array.Empty<SkinDef>();");
            }
            else
            {
                code.L("skin.baseSkins = new SkinDef[] ")
                    .O();
                for (var i = 0; i < skin.BaseSkins.Count; i++)
                {
                    var reference = skin.BaseSkins[i];
                    if (reference.accessType == AccessType.ByIndex)
                    {
                        code.L("ThrowIfOutOfBounds(").A(i).A(", \"Index ").A(reference.index).A(" is out of bounds of skins array\", skinController.skins, ").A(reference.index).A("),");
                    }
                    else
                    {
                        code.L("ThrowIfNull(").A(i).A(", \"There is no skin with the name \\\"").A(reference.name).A("\\\"\", skinController.skins.FirstOrDefault(s => s.name == ").Literal(reference.name).A(")),");
                    }
                }
                code.C().A(";");
            }
            code.C().A(");");
        }

        private void BuildTryCatchThrowArray<T>(
            ISkinParams skin,
            string errorField,
            string field,
            string type,
            IList<T> collection,
            Action<T, int> itemAction,
            Action prefixAction = null) where T : IConfigEnabled
        {
            code.L("TryCatchThrow(").Literal(errorField).A(", () =>")
                .O();

            prefixAction?.Invoke();

            if (collection.Count == 0)
            {
                code.L(field).A(" = Array.Empty<").A(type).A(">();");
            }
            else if (collection.Any(i => !string.IsNullOrWhiteSpace(i.EnableFromConfig)))
            {
                code.L("var list = new List<").A(type).A(">();");
                for (var i = 0; i < collection.Count; i++)
                {
                    var item = collection[i];
                    if (!string.IsNullOrWhiteSpace(item.EnableFromConfig))
                    {
                        code.L("if (").A(skin.Name.ToUpperCamelCase()).A(item.EnableFromConfig.ToUpperCamelCase()).A("Config.Value)")
                            .O();
                    }

                    code.L("list.Add(");
                    itemAction(item, i);
                    code.A(");");

                    if (!string.IsNullOrWhiteSpace(item.EnableFromConfig))
                    {
                        code.C();
                    }
                }
                code.L(field).A(" = list.ToArray();");
            }
            else
            {
                code.L(field).A(" = new ").A(type).A("[] ")
                    .O();
                for (var i = 0; i < collection.Count; i++)
                {
                    code.L();
                    itemAction(collection[i], i);
                    code.A(",");
                }
                code.C().A(";");
            }
            code.C().A(");");
        }

        private void BuildSkinIcon(SkinDefinition skin)
        {
            code.L("TryCatchThrow(\"Icon\", () =>")
                .O();
            if (!skin.icon.createFromColors)
            {
                if (skin.icon.sprite)
                {
                    code.L("skin.icon = assetBundle.LoadAsset<Sprite>(@\"").A(AssetsInfo.iconPaths[skin.icon.sprite]).A("\");");
                }
                else
                {
                    code.L("skin.icon = null;");
                }
            }
            else
            {
                code.L("skin.icon = assetBundle.LoadAsset<Sprite>(@\"").A(AssetsInfo.iconFromColorPaths[skin.icon.colors]).A("\");");
            }
            code.C().A(");");
        }

        private void BuildLanguageLoadStrings()
        {
            code.L()
                .L("private static void LanguageLoadStrings(Action<Language> orig, Language self)")
                .O()
                .L("orig(self);")
                .L();

            var tokensByLanguage = new Dictionary<string, Dictionary<string, string>>();
            var defaultTokens = new Dictionary<string, string>();
            foreach (var skin in ReorderedSkins)
            {
                if (skin.nameTokenLocalizations.Count == 0)
                {
                    continue;
                }
                var nameToken = skin.CreateNameToken(SkinModInfo.author);
                defaultTokens[nameToken] = (skin.nameTokenLocalizations.FirstOrDefault(el => el.language.ToLower() == "en") ?? skin.nameTokenLocalizations.First()).value;

                foreach (var localization in skin.nameTokenLocalizations)
                {
                    if (defaultTokens[nameToken] == localization.value)
                    {
                        continue;
                    }
                    var language = localization.language.ToLower();
                    if (!tokensByLanguage.TryGetValue(language, out var tokens))
                    {
                        tokensByLanguage[language] = tokens = new Dictionary<string, string>();
                    }
                    tokens[nameToken] = localization.value;
                }
            }
            foreach (var row in defaultTokens)
            {
                code.L("self.SetStringByToken(").Literal(row.Key).A(", ").Literal(row.Value).A(");");
            }

            if (tokensByLanguage.Count > 0)
            {
                code.L()
                    .L("switch(self.name.ToLower())")
                    .O();
                foreach (var row in tokensByLanguage)
                {
                    code.L("case ").Literal(row.Key).A(":")
                        .O();
                    foreach (var value in row.Value)
                    {
                        code.L("self.SetStringByToken(").Literal(value.Key).A(", ").Literal(value.Value).A(");");
                    }
                    code.L("break;")
                        .C();
                }
                code.C();
            }

            code.C();
        }

        private void BuildReplaceShaders()
        {
            if (AssetsInfo.materialsWithRoRShader.Count != 0)
            {
                code.L()
                    .L("private static void ReplaceShaders()")
                    .O();
                foreach (var row in AssetsInfo.materialsWithRoRShader)
                {
                    code.L("LoadMaterialsWithReplacedShader(")
                        .O("")
                        .L("@\"").A(row.Key).A(".shader\"");
                    foreach (var material in row.Value)
                    {
                        code.L(",@\"").A(AssetsInfo.materialPaths[material]).A("\"");
                    }
                    code.A(");").C("");
                }
                code.C()
                    .L()
                    .L("private static void LoadMaterialsWithReplacedShader(string shaderPath, params string[] materialPaths)")
                    .O()
                        .L("var shader = Addressables.LoadAssetAsync<Shader>(shaderPath).WaitForCompletion();")
                        .L("foreach (var materialPath in materialPaths)")
                        .O()
                            .L("var material = assetBundle.LoadAsset<Material>(materialPath);")
                            .L("material.shader = shader;")
                            .L("materialsWithRoRShader.Add(material);")
                        .C()
                    .C();
            }
        }
    }
}
