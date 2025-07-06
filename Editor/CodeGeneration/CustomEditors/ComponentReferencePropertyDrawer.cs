using RoRSkinBuilder.Data;
using UnityEditor;

namespace RoRSkinBuilder.CustomEditors
{
    [CustomPropertyDrawer(typeof(ComponentReference))]
    public class ComponentReferencePropertyDrawer : HideablePropertyDrawer<ComponentReference>
    {
    }
}
