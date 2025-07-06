namespace RoRSkinBuilder.Data
{
    public enum AccessType { ByIndex, ByName }
    public enum ComponentAccessType { ByName, ByPath }
    public enum DependencyType { HardDependency, SoftDependency }
    public enum IconSize { s32 = 32, s128 = 128 }
    public enum RenderersSource { AllRendererComponents, BaseRendererInfos }
    public enum GameObjectActivationAccessType { ByRendererName = 1, ByPath }
}
