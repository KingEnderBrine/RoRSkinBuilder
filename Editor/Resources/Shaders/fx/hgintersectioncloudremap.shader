Shader "Fake RoR/Hopoo Games/FX/Cloud Intersection Remap" {
	Properties {
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendFloat ("Source Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlendFloat ("Destination Blend", Float) = 1
		[HDR] _TintColor ("Tint", Vector) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "grey" {}
		_Cloud1Tex ("Cloud 1 (RGB) Trans (A)", 2D) = "grey" {}
		_Cloud2Tex ("Cloud 2 (RGB) Trans (A)", 2D) = "grey" {}
		_RemapTex ("Color Remap Ramp (RGB)", 2D) = "grey" {}
		_CutoffScroll ("Cutoff Scroll Speed", Vector) = (0,0,0,0)
		_InvFade ("Soft Factor", Range(0, 30)) = 1
		_SoftPower ("Soft Power", Range(0.1, 20)) = 1
		_Boost ("Brightness Boost", Range(0, 5)) = 1
		_RimPower ("Rim Power", Range(0.1, 20)) = 1
		_RimStrength ("Rim Strength", Range(0, 5)) = 1
		_AlphaBoost ("Alpha Boost", Range(0, 20)) = 1
		_IntersectionStrength ("Intersection Strength", Range(0, 20)) = 0
		[MaterialEnum(Off,0,Front,1,Back,2)] _Cull ("Cull", Float) = 0
		[PerRendererData] _ExternalAlpha ("External Alpha", Range(0, 1)) = 1
		[Toggle(IGNORE_VERTEX_COLORS)] _VertexColorsOn ("Ignore Vertex Colors", Float) = 0
		[Toggle(TRIPLANAR)] _TriplanarOn ("Enable Triplanar Projections for Clouds", Float) = 0
	}
	Fallback "Diffuse"
}