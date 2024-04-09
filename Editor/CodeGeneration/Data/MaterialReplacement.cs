using System;
using System.Collections.Generic;
using System.Text;
using RoRSkinBuilder.CustomEditors;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [CreateAssetMenu(menuName = "RoR Skins/"+ nameof(MaterialReplacement))]
    public class MaterialReplacement : ScriptableObject
    {
        public Shader shader;
        public bool useAddressablesMaterial;
        public Reference rendererReference;
        public string addressablesKey;
        [Space]
        public List<ShaderField> fields;
    }
}