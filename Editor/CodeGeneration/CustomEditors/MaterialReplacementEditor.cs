using RoRSkinBuilder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.CustomEditors
{
    [CustomEditor(typeof(MaterialReplacement))]
    public class MaterialReplacementEditor : Editor
    {
        private string[] shaderNames;
        private int selectedIndex;

        private SerializedProperty shaderProperty;
        private SerializedProperty useAddressablesMaterialProperty;
        private SerializedProperty rendererReferenceProperty;
        private SerializedProperty addressableKeysProperty;
        private SerializedProperty fieldsProperty;

        private string[] shaderProperties = Array.Empty<string>();

        private void Initialize()
        {
            if (shaderProperty == null)
            {
                shaderProperty = serializedObject.FindProperty("shader");
            }
            if (useAddressablesMaterialProperty == null)
            {
                useAddressablesMaterialProperty = serializedObject.FindProperty("useAddressablesMaterial");
            }
            if (rendererReferenceProperty == null)
            {
                rendererReferenceProperty = serializedObject.FindProperty("rendererReference");
            }
            if (addressableKeysProperty == null)
            {
                addressableKeysProperty = serializedObject.FindProperty("addressablesKey");
            }
            if (fieldsProperty == null)
            {
                fieldsProperty = serializedObject.FindProperty("fields");
            }
            if (shaderNames == null)
            {
                shaderNames = ShaderUtil.GetAllShaderInfo()
                    .Where(shaderInfo => !shaderInfo.name.StartsWith("Deprecated") && !shaderInfo.name.StartsWith("Hidden"))
                    .Select(shaderInfo => shaderInfo.name)
                    .ToArray();
                selectedIndex = shaderNames.ToList().IndexOf((shaderProperty.objectReferenceValue as Shader)?.name);
                
                if (selectedIndex >= 0)
                {
                    SelectShader(selectedIndex);
                }
            }
        }

        protected override void OnHeaderGUI()
        {
            Initialize();

            base.OnHeaderGUI();
 
            var rect = GUILayoutUtility.GetLastRect();
            var shaderFieldRect = new Rect(rect.x + 44F, rect.yMax - 21F, rect.width - 50F, 17F);
            var labelWidth = 50F;

            var shaderLabelRect = new Rect(shaderFieldRect);
            shaderLabelRect.width = labelWidth;

            var shaderDropdownRect = new Rect(shaderFieldRect);
            shaderDropdownRect.x += labelWidth;
            shaderDropdownRect.width -= labelWidth;

            EditorGUI.LabelField(shaderLabelRect, "Shader");
            EditorGUI.BeginChangeCheck();
            var index = EditorGUI.Popup(shaderDropdownRect, selectedIndex, shaderNames);
            if (EditorGUI.EndChangeCheck() && index >= 0)
            {
                SelectShader(index);
                
                fieldsProperty.ClearArray();
                for (var i = 0; i < shaderProperties.Length; i++)
                {
                    AddProperty(i);
                }
            }
        }

        protected override bool ShouldHideOpenButton() => true;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Initialize();

            EditorGUILayout.PropertyField(useAddressablesMaterialProperty);
            if (useAddressablesMaterialProperty.boolValue)
            {
                EditorGUILayout.PropertyField(addressableKeysProperty);
            }
            else
            {
                EditorGUILayout.PropertyField(rendererReferenceProperty);
            }

            var index = EditorGUILayout.Popup("Add field", - 1, shaderProperties);
            if (index >= 0)
            {
                AddProperty(index);
            }

            EditorGUILayout.Space();
            if (EditorGUILayout.PropertyField(fieldsProperty, false))
            {
                EditorGUI.indentLevel += 1;
                for (int i = 0; i < fieldsProperty.arraySize; i++)
                {
                    EditorGUILayout.PropertyField(fieldsProperty.GetArrayElementAtIndex(i));
                }
                EditorGUI.indentLevel -= 1;
            }    
            serializedObject.ApplyModifiedProperties();
        }

        private void AddProperty(int index)
        {
            fieldsProperty.InsertArrayElementAtIndex(fieldsProperty.arraySize);

            var shader = Shader.Find(shaderNames[selectedIndex]);
            
            var property = fieldsProperty.GetArrayElementAtIndex(fieldsProperty.arraySize - 1);
            
            property.FindPropertyRelative("propertyDescription").stringValue = ShaderUtil.GetPropertyDescription(shader, index);

            var propertyName = ShaderUtil.GetPropertyName(shader, index);
            property.FindPropertyRelative("propertyName").stringValue = propertyName;

            var shaderPropertyType = ShaderUtil.GetPropertyType(shader, index);
            ShaderField.PropertyType propertyType = ShaderField.PropertyType.Float;


            switch (shaderPropertyType)
            {
                case ShaderUtil.ShaderPropertyType.Color:
                    propertyType = ShaderField.PropertyType.Color;
                    break;
                case ShaderUtil.ShaderPropertyType.Vector:
                    propertyType = ShaderField.PropertyType.Vector;
                    break;
                case ShaderUtil.ShaderPropertyType.Float:
                    propertyType = GetPropertyTypeFromFloat(shader, propertyName, property, index);
                    break;
                case ShaderUtil.ShaderPropertyType.Range:
                    propertyType = ShaderField.PropertyType.Range;
                    break;
                case ShaderUtil.ShaderPropertyType.TexEnv:
                    propertyType = ShaderField.PropertyType.Texture;
                    break;
            }

            property.FindPropertyRelative("textureValue").objectReferenceValue = default;
            property.FindPropertyRelative("offset").vector2Value = default;
            property.FindPropertyRelative("tiling").vector2Value = new Vector2(1, 1);
            property.FindPropertyRelative("vectorValue").vector4Value = 
                propertyType == ShaderField.PropertyType.Vector ? shader.GetPropertyDefaultVectorValue(index) : default;
            property.FindPropertyRelative("colorValue").colorValue = default;
            property.FindPropertyRelative("keyword").stringValue = default;

            if (shaderPropertyType == ShaderUtil.ShaderPropertyType.Float || shaderPropertyType == ShaderUtil.ShaderPropertyType.Range)
            {
                property.FindPropertyRelative("floatValue").floatValue = shader.GetPropertyDefaultFloatValue(index);
            }
            else
            {
                property.FindPropertyRelative("floatValue").floatValue = default;
            }

            property.FindPropertyRelative("propertyType").enumValueIndex = (int)propertyType;
        }

        private ShaderField.PropertyType GetPropertyTypeFromFloat(Shader shader, string name, SerializedProperty property, int index)
        {
            var propertyAttributes = shader.GetPropertyAttributes(index);
            if (propertyAttributes == null || propertyAttributes.Length == 0)
            {
                return ShaderField.PropertyType.Float;
            }

            foreach (var attribute in propertyAttributes)
            {
                var attrName = attribute;
                var argsText = string.Empty;
                Match match = Regex.Match(attribute, "(\\w+)\\s*\\((.*)\\)");
                if (match.Success)
                {
                    attrName = match.Groups[1].Value;
                    argsText = match.Groups[2].Value.Trim();
                }
                
                if (attrName == "Toggle")
                {
                    property.FindPropertyRelative("keyword").stringValue = argsText;
                    return ShaderField.PropertyType.Toggle;
                }

                if (attrName == "MaterialEnum")
                {
                    return ShaderField.PropertyType.Enum;
                }
            }

            return ShaderField.PropertyType.Float;
        }

        private void SelectShader(int index)
        {
            var shader = Shader.Find(shaderNames[index]);

            shaderProperties = new string[ShaderUtil.GetPropertyCount(shader)];

            for (var i = 0; i < shaderProperties.Length; i++)
            {
                shaderProperties[i] = ShaderUtil.GetPropertyDescription(shader, i);
            }

            shaderProperty.objectReferenceValue = shader;
            selectedIndex = index;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
