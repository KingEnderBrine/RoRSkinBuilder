using RoRSkinBuilder.Data;
using UnityEditor;

namespace RoRSkinBuilder.CustomEditors
{
    [CustomPropertyDrawer(typeof(Reference))]
    public class ReferencePropertyDrawer : HideablePropertyDrawer<Reference>
    {
    }
}
