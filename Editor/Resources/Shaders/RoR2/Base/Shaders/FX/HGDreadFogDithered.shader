Shader "StubbedRoR2/Base/Shaders/HGDreadFogDithered" {
	Properties {
		[Header(_)] [Header(FOG)] [Header(_)] _useFog ("Use Fog 0/1", Float) = 1
		_fogColor ("Fog Tint Color", Color) = (1,1,1,1)
		_FogDistance ("Fog Distance", Range(0, 1000)) = 1.2
		_fogIntensity ("Fog Intensity", Range(0, 1)) = 0
		[Header(_)] [Header(ShaderLightning)] [Header(_)] _AlbedoIntensity ("Albedo Intensity", Range(0, 1)) = 0
		_EmissiveIntensity ("Emissive Intensity", Range(0, 1)) = 1
		[Space(25)] [Header(_)] [Header(REMAP)] [Header(_)] [Toggle(DISABLEREMAP)] _DisableRemapOn ("REMAP using ramps", Float) = 0
		_RemapTex ("Top Clouds Remap Ramp (RGB)", 2D) = "grey" {}
		_RemapTexB ("Bottom Clouds Remap Ramp (RGB)", 2D) = "grey" {}
		[Space(25)] [Header(_)] [Header(MAPS)] [Header(_)] _sceneTintColor ("Tint Color", Color) = (1,1,1,1)
		[HDR] _BottomTintColor ("Top Cloud Color", Color) = (1,1,1,1)
		[HDR] _TopTintColor ("Bottom Cloud Color", Color) = (1,1,1,1)
		_bottomTint ("Ground Color - vertex Blue Channel", Color) = (1,1,1,1)
		_MainTex ("Cloud Top", 2D) = "grey" {}
		_topBoost ("Top Brightness Boost", Range(0, 2)) = 0
		_topCloudScrollSpeed ("Top Cloud Scroll Speed", Vector) = (0,0,0,0)
		_SecTex ("Cloud Bottom", 2D) = "grey" {}
		_bottomBoost ("Bottom Brightness Boost", Range(0, 2)) = 0
		_bottomCloudScrollSpeed ("Bottom Cloud Scroll Speed", Vector) = (0,0,0,0)
		_MaskTex ("Fog Mask Texture", 2D) = "grey" {}
		_maskScrollSpeed ("Mask Scroll Speed", Vector) = (0,0,0,0)
		_maskContrast ("Mask Contrast", Range(-100, 100)) = 0
		_maskMid ("Mask Midpoint", Range(-1, 1)) = 0
		[Space(25)] [Header(_)] [Header(Dithering)] [Header(_)] _DitherMask ("Dither Mask", 2D) = "grey" {}
		_DitherMaskScale ("Dither Mask Scale", Range(0, 100)) = 10
		_Cutoff ("Mask Clip Value", Range(0, 1)) = 0.5
		_InvFade ("Soft Factor", Range(0, 2)) = 0.1
		_DevAlphaValue ("Base Alpha Value", Range(0, 2)) = 1
		[Toggle(USE_CONTRAST)] _ContrastOn ("Use contrast Alpha", Float) = 0
		_ContrastAlpha ("Alpha Contrast", Range(-100, 100)) = 0
		_ContrastMid ("Alpha Contrast Midpoint", Range(0, 1)) = 0.5
		[Toggle(FADECLOSE)] _FadeCloseOn ("Fade when near camera", Float) = 0
		_FadeCloseDistance ("Fade Close Distance", Range(0, 1)) = 0.5
		[MaterialEnum(None,0,Front,1,Back,2)] _Cull ("Culling Mode", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
		_DepthOffset ("_DepthOffset", Range(-10, 10)) = 0
		[Toggle(FRESNEL)] _FresnelOn ("Fresnel Fade", Float) = 0
		[Toggle(SKYBOX_ONLY)] _SkyboxOnly ("Skybox Only", Float) = 0
		_FresnelPower ("Fresnel Power", Range(-20, 20)) = 0
		[Space(25)] [Header(_)] [Header(Emissive)] [Header(_)] [Toggle(USE_EMISSIVE)] _useEmissive ("Use Emissive map", Float) = 0
		[HDR] _EmTint ("Emissive Tint", Color) = (1,1,1,1)
		_EmTex ("Emmisive Texture", 2D) = "grey" {}
		_emBoost ("Emissive Intensity", Range(0, 50)) = 0
		_emScrollSpeed ("Emissive Scroll Speed", Vector) = (0,0,0,0)
		_EmMask ("Emmisive Mask", 2D) = "grey" {}
		[Space(25)] [Header(_)] [Header(Secondary Emissive)] [Header(_)] [Toggle(USE_EMISSIVE2)] _useEmissive2 ("Use Secondary Emissive FLOW map", Float) = 0
		[HDR] _EmTint2 ("Emissive Tint", Color) = (1,1,1,1)
		_EmTex2 ("Emmisive Texture", 2D) = "grey" {}
		_emBoost2 ("Emissive Intensity", Range(0, 200)) = 0
		_emScrollSpeed2 ("Emissive Scroll Speed", Vector) = (0,0,0,0)
		_EmMask2 ("Emmisive Mask", 2D) = "grey" {}
		[Space(10)] _NormalTex ("Normal Map", 2D) = "bump" {}
		[NoScaleOffset] _FlowTex ("Flow Vector (RG), Noise (B)", 2D) = "bump" {}
		[HDR] _FlowTint ("Flow Tint", Color) = (1,1,1,1)
		_FlowHeightmap ("Flow Heightmap", 2D) = "white" {}
		_FlowHeightRamp ("Flow Ramp", 2D) = "black" {}
		_flowOffsetPower ("Flow Offset Power", Range(0, 1)) = 1
		_FlowHeightBias ("Flow Height Bias", Range(-1, 1)) = 0
		_FlowHeightPower ("Flow Height Power", Range(0.1, 20)) = 1
		_FlowEmissionStrength ("Flow Height Strength", Range(0.1, 20)) = 1
		_FlowSpeed ("Flow Speed", Range(0, 15)) = 1
		_FlowMaskStrength ("Mask Flow Strength", Range(0, 5)) = 0
		_FlowNormalStrength ("Normal Flow Strength", Range(0, 5)) = 1
		_FlowTextureScaleFactor ("Flow Texture Scale Factor", Range(0, 10)) = 1
		[Space(25)] [Header(_)] [Header(Top Down Gradient)] [Header(_)] [Toggle(USE_GRADIENT)] _useGradient ("Use Top Down Gradient map - uv2", Float) = 0
		_gradientTex ("Gradient Remap - uv2", 2D) = "grey" {}
		_gradientIntensity ("Gradient Intensity", Range(0, 100)) = 0
		[Space(25)] [Header(_)] [Header(Vertex Displacement)] [Header(_)] [Toggle(USE_VERTEX)] _useVertex ("Use Vertex Displacement", Float) = 0
		_VertexOffset1Speed ("Vertex Offset 1 Speed", Vector) = (0.2,0.5,0,0)
		_VertexOffset1Tiling ("Vertex Offset 1 Tiling", Vector) = (-6,1,0,0)
		_VertexOffset2Speed ("Vertex Offset 2 Speed", Vector) = (0.5,0,0,0)
		_VertexOffset2Tiling ("Vertex Offset 2 Tiling", Vector) = (5,0.5,0,0)
		_VertexOffsetAmount ("Push Pull Offset", Float) = 0
		_VertexOffsetPower ("Vertex Offset Power", Float) = 0.12
		[PerRendererData] _ExternalAlpha ("External Alpha", Range(0, 1)) = 1
		_Fade ("Fade", Range(0, 1)) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Transparent/VertexLit"
}