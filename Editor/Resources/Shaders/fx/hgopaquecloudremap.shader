Shader "Fake RoR/Hopoo Games/FX/Opaque Cloud Remap" {
	Properties {
		[HDR] _TintColor ("Tint", Vector) = (1,1,1,1)
		[HDR] _EmissionColor ("Emission", Vector) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "grey" {}
		_NormalStrength ("Normal Strength", Range(0, 5)) = 1
		[NoScaleOffset] _NormalTex ("Normal Map", 2D) = "bump" {}
		_Cloud1Tex ("Cloud 1 (RGB) Trans (A)", 2D) = "grey" {}
		_Cloud2Tex ("Cloud 2 (RGB) Trans (A)", 2D) = "grey" {}
		_RemapTex ("Color Remap Ramp (RGB)", 2D) = "grey" {}
		_CutoffScroll ("Cutoff Scroll Speed", Vector) = (0,0,0,0)
		_InvFade ("Soft Factor", Range(0, 30)) = 1
		_AlphaBoost ("Alpha Boost", Range(0, 20)) = 1
		_Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
		_SpecularStrength ("Specular Strength", Range(0, 1)) = 0
		_SpecularExponent ("Specular Exponent", Range(0.1, 20)) = 1
		_ExtrusionStrength ("Extrusion Strength", Range(0, 10)) = 1
		[MaterialEnum(Two Tone,0,Smoothed Two Tone,1,Unlitish,3,Subsurface,4,Grass,5)] _RampInfo ("Ramp Choice", Float) = 0
		[Toggle (EMISSIONFROMALBEDO)] _EmissionFromAlbedo ("Emission From Albedo", Float) = 0
		[Toggle (CLOUDNORMAL)] _CloudNormalMap ("Use NormalMap as a Cloud", Float) = 0
		[Toggle(VERTEXALPHA)] _VertexAlphaOn ("Luminance for Vertex Alpha", Float) = 0
		[MaterialEnum(Off,0,Front,1,Back,2)] _Cull ("Cull", Float) = 2
		[PerRendererData] _ExternalAlpha ("External Alpha", Range(0, 1)) = 1
	}
	Fallback "Transparent/Cutout/Diffuse"
}