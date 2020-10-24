using System;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class IconColors
    {
        private static Color32[] template128;
        private static Color32[] Template128 { get => template128 ?? (template128 = SearchTemplate(IconSize.s128)); }
        private static Color32[] template32;
        private static Color32[] Template32 { get => template32 ?? (template32 = SearchTemplate(IconSize.s32)); }

        [ColorUsage(false)]
        public Color32 left;
        [ColorUsage(false)]
        public Color32 top;
        [ColorUsage(false)]
        public Color32 right;
        [ColorUsage(false)]
        public Color32 bottom;

        public static Texture2D CreateSkinIcon(IconColors colors, IconSize size = IconSize.s128) => CreateSkinIcon(colors.top, colors.right, colors.bottom, colors.left, size);

        public static Texture2D CreateSkinIcon(Color32 top, Color32 right, Color32 bottom, Color32 left, IconSize size = IconSize.s128)
        {
            var side = (int)size;
            Color32[] template;
            switch (size)
            {
                case IconSize.s32:
                    template = Template32;
                    break;
                case IconSize.s128:
                    template = Template128;
                    break;
                default:
                    throw new NotSupportedException();
            }
            if (template == null)
            {
                Debug.LogError("Template texture was not found");
                return null;
            }
            Texture2D tex = new Texture2D(side, side, TextureFormat.RGBA32, false);
            tex.SetPixels32(template);
            new IconTexJob
            {
                LineThickness = Math.Max(2, side / 128 * 4),
                Side = side,
                Top = top,
                Bottom = bottom,
                Right = right,
                Left = left,
                TexOutput = tex.GetRawTextureData<Color32>()
            }.Schedule(side * side, 32).Complete();
            tex.wrapMode = TextureWrapMode.Clamp;

            tex.Apply();
            return tex;
        }
        
        private static Color32[] SearchTemplate(IconSize size)
        {
            var assets = AssetDatabase.FindAssets($"l:SkinBuilderT{(int)size}");
            if (assets.Length == 0)
            {
                return null;
            }
            var path = AssetDatabase.GUIDToAssetPath(assets.First());
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            
            return tex.GetPixels32();
        }

        private struct IconTexJob : IJobParallelFor
        {
            [ReadOnly]
            public int Side;
            [ReadOnly]
            public int LineThickness;
            [ReadOnly]
            public Color32 Top;
            [ReadOnly]
            public Color32 Right;
            [ReadOnly]
            public Color32 Bottom;
            [ReadOnly]
            public Color32 Left;
            public NativeArray<Color32> TexOutput;
            public void Execute(int index)
            {
                int x = index % Side - Side / 2;
                int yc = index / Side - Side / 2;

                if (yc >= x && yc >= -x)
                {
                    TexOutput[index] = (Color.white - TexOutput[index]) + (Top + Color.black);
                    return;
                }
                if (yc < x && yc < -x)
                {
                    TexOutput[index] = (Color.white - TexOutput[index]) + (Bottom + Color.black);
                    return;
                }
                if (yc >= x && yc <= -x)
                {
                    TexOutput[index] = (Color.white - TexOutput[index]) + (Left + Color.black);
                    return;
                }
                if (yc < x && yc >= -x)
                {
                    TexOutput[index] = (Color.white - TexOutput[index]) + (Right + Color.black);
                    return;
                }
            }
        }
    }
}
