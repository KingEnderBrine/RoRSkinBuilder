Shader "StubbedRoR2/DLC3/TransparentTextureScrolling" {
	Properties {
		[Enum(Opaque,0,Transparent,1)] _BlendMode ("Blend Mode", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Source Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Destination Blend", Float) = 1
		_TintColor ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Inner Effect Tex (RGB)", 2D) = "grey" {}
		_FadeCloseDistance ("Fade Close Distance", Range(0, 1)) = 0.5
		[MaterialEnum(None,0,Front,1,Back,2)] _Cull ("Culling Mode", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
		[Toggle(ZWRITE)] _ZWriteMode ("ZWrite Mode", Float) = 0
		_RemapTex ("Color Remap Ramp (RGB)", 2D) = "grey" {}
		_Color ("Inner Effect Color", Color) = (0.5,0.5,0.5,1)
		[HDR] _EmColor ("Emission Color", Color) = (0,0,0,1)
		[NoScaleOffset] _EmTex ("Emission Tex (RGB)", 2D) = "white" {}
		_EmPower ("Emission Power", Range(0, 10)) = 1
		_EmBoost ("Emission Saturation", Range(0, 100)) = 1
		_ScrollTex ("Inline Tex Effect (RGB)", 2D) = "white" {}
		_TexScrollSpeed ("Inline Tex Effect Scroll Speed (XY)", Vector) = (0.2,0.2,0,0)
		_BaseTex_TilingOffset ("Inner Texture Tiling (X,Y) and Offset (Z,W)", Vector) = (1,1,0,0)
		[NoScaleOffset] _BaseTexScrollSpeed ("Inner Texture Scroll Speed (XY)", Vector) = (0,0,0,0)
		_WireframeColor ("Wireframe Color", Color) = (1,1,1,1)
		_WireframeThickness ("Wireframe Thickness", Range(0, 0.1)) = 0.01
		_WireframeSmoothing ("Wireframe Smoothing", Range(0.5, 3)) = 1
		[Toggle(WIREFRAME_ONLY)] _WireframeOnly ("Wireframe Only", Float) = 0
		[Toggle(FLIPVERTICAL)] _flipVerticalMode ("Flip Gradient Direction", Float) = 0
		_ModelHeight ("Model Height", Float) = 1
		_VertColor ("Vertical Effect Color", Color) = (0.5,0.5,0.5,1)
		_GradientCenter ("Gradient Center Position", Range(-4, 4)) = 0
		_GradientWidth ("Transition Zone Width", Range(0.1, 5)) = 1
		_GradientSmoothness ("Gradient Smoothness", Range(0.1, 3)) = 1
		[Toggle(LIMBREMOVAL)] _LimbRemovalOn ("Enable Limb Removal", Float) = 0
		_LimbPrimeMask ("Limb Prime Mask", Range(1, 10000)) = 1
		_Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
		[PerRendererData] _ExternalAlpha ("External Alpha", Range(0, 1)) = 1
		[PerRendererData] _Fade ("Fade", Range(0, 1)) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Transparent/VertexLit"
	//CustomEditor "RoR2.TransparentTextureScrollingGUI"
}