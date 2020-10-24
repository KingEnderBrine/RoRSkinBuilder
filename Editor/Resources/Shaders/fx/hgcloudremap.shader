Shader "Fake RoR/Hopoo Games/FX/Cloud Remap" {
	Properties {
		[HideInInspector] _SrcBlend ("Source Blend", Float) = 1
		[HideInInspector] _DstBlend ("Destination Blend", Float) = 1
		[HideInInspector] _InternalSimpleBlendMode ("Internal Simple Blend Mode", Float) = 0
		[HDR] _TintColor ("Tint", Vector) = (1,1,1,1)
		[Toggle(DISABLEREMAP)] _DisableRemapOn ("Disable Remapping", Float) = 0
		_MainTex ("Base (RGB) Trans (A)", 2D) = "grey" {}
		_RemapTex ("Color Remap Ramp (RGB)", 2D) = "grey" {}
		_InvFade ("Soft Factor", Range(0, 2)) = 0.1
		_Boost ("Brightness Boost", Range(1, 20)) = 1
		_AlphaBoost ("Alpha Boost", Range(0, 20)) = 1
		_AlphaBias ("Alpha Bias", Range(0, 1)) = 0
		[Toggle(USE_UV1)] _UseUV1On ("Use UV1", Float) = 0
		[Toggle(FADECLOSE)] _FadeCloseOn ("Fade when near camera", Float) = 0
		_FadeCloseDistance ("Fade Close Distance", Range(0, 1)) = 0.5
		[MaterialEnum(None,0,Front,1,Back,2)] _Cull ("Culling Mode", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
		_DepthOffset ("_DepthOffset", Range(-10, 10)) = 0
		[Toggle(USE_CLOUDS)] _CloudsOn ("Cloud Remapping", Float) = 1
		[Toggle(CLOUDOFFSET)] _CloudOffsetOn ("Distortion Clouds", Float) = 0
		_DistortionStrength ("Distortion Strength", Range(-2, 2)) = 0.1
		_Cloud1Tex ("Cloud 1 (RGB) Trans (A)", 2D) = "grey" {}
		_Cloud2Tex ("Cloud 2 (RGB) Trans (A)", 2D) = "grey" {}
		_CutoffScroll ("Cutoff Scroll Speed", Vector) = (0,0,0,0)
		[Toggle(VERTEXCOLOR)] _VertexColorOn ("Vertex Colors", Float) = 0
		[Toggle(VERTEXALPHA)] _VertexAlphaOn ("Luminance for Vertex Alpha", Float) = 0
		[Toggle(CALCTEXTUREALPHA)] _CalcTextureAlphaOn ("Luminance for Texture Alpha", Float) = 0
		[Toggle(VERTEXOFFSET)] _VertexOffsetOn ("Vertex Offset", Float) = 0
		[Toggle(FRESNEL)] _FresnelOn ("Fresnel Fade", Float) = 0
		[Toggle(SKYBOX_ONLY)] _SkyboxOnly ("Skybox Only", Float) = 0
		_FresnelPower ("Fresnel Power", Range(-20, 20)) = 0
		_OffsetAmount ("Vertex Offset Amount", Range(0, 3)) = 0
		[PerRendererData] _ExternalAlpha ("External Alpha", Range(0, 1)) = 1
		[PerRendererData] _Fade ("Fade", Range(0, 1)) = 1
	}
	Fallback "Transparent/VertexLit"
}