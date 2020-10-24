using RoRSkinBuilder.Data;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.CustomEditors
{
    [CustomPropertyDrawer(typeof(IconColors))]
    public class IconColorsPropertyDrawer : PropertyDrawer
    {
        private const float defaultPropertyHeigh = 16;
        private const float iconSide = 64;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = defaultPropertyHeigh;
            property.Next(true);
            for (var i = 0; i < 4; i++)
            {
                height += EditorGUI.GetPropertyHeight(property);
                property.Next(false);
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var leftProperty = property.FindPropertyRelative("left");
            var rightProperty = property.FindPropertyRelative("right");
            var topProperty = property.FindPropertyRelative("top");
            var bottomProperty = property.FindPropertyRelative("bottom");
            var iconPreviewProperty = property.FindPropertyRelative("iconPreview");

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PrefixLabel(new Rect(position.x, position.y, position.width, defaultPropertyHeigh), label);

            EditorGUI.indentLevel++;
            var y = position.y + defaultPropertyHeigh;
            property.Next(true);
            for (var i = 0; i < 4; i++)
            {
                var height = EditorGUI.GetPropertyHeight(property);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width - iconSide, height), property);
                y += height;
                property.Next(false);
            }

            var style = new GUIStyle();
            style.normal.background = IconColors.CreateSkinIcon(topProperty.colorValue, rightProperty.colorValue, bottomProperty.colorValue, leftProperty.colorValue, IconSize.s32);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.LabelField(new Rect(position.x + position.width - iconSide, position.y + defaultPropertyHeigh, iconSide, iconSide), GUIContent.none, style);
            EditorGUI.indentLevel = indent;

            EditorGUI.indentLevel--;


            EditorGUI.EndProperty();
        }
    }
}
