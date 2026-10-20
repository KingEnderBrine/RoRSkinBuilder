Shader "StubbedRoR2/DLC4/CrystalShader" {
	Properties {
		[Header(Base Color)] [Space] _MainColor ("Main Color", Color) = (0.19,0.3,0.07,1)
		[Header(Specular and Lighting)] [Space] _SpecularStrength ("Specular Strength", Range(0, 1)) = 0
		_SpecularExponent ("Specular Exponent", Range(0.1, 20)) = 1
		_Smoothness ("Smoothness", Range(0, 1)) = 0.9
		[MaterialEnum(Two Tone,0,Smoothed Two Tone,1,Unlitish,3,Subsurface,4,Grass,5)] _RampInfo ("Ramp Choice", Float) = 0
		[MaterialEnum(Default,0,Environment,1,Character,2,Misc,3)] _DecalLayer ("Decal Layer", Float) = 0
		[Header(Inner Glow)] [Space] _GlowSize ("Glow Size", Float) = 0.5
		_GlowFalloff ("Glow Falloff", Float) = 1
		_GlowIntensity ("Glow Intensity", Float) = 5
		[HDR] _GlowColor ("Glow Color", Color) = (0.93,0.91,0.18,1)
		[Header(MatCap)] [Space] _MatCapTexture ("MatCap Texture", 2D) = "white" {}
		_MatCapFactor ("MatCap Factor", Float) = 0.8
		_MatCapPower ("MatCap Power", Float) = 1
		_MatCapColor ("MatCap Color", Color) = (0.51,0.51,0.33,1)
		[Header(Mist Scroll)] [Space] _PrimaryMistTexture ("Primary Mist Texture", 2D) = "white" {}
		_ScrollSpeedOne ("Scroll Speed 1", Vector) = (0.015,0.015,0,0)
		_SecondaryMistTexture ("Secondary Mist Texture", 2D) = "white" {}
		_ScrollSpeedTwo ("Scroll Speed 2", Vector) = (0,-0.1,0,0)
		[Header(Fresnel)] [Space] [HDR] _FresnelColor ("Fresnel Color", Color) = (1.1,1.94,0.21,1)
		_FresnelPower ("Fresnel Power", Range(0, 5)) = 0.7
		[Toggle(FRESNEL_RAMP)] _FresnelRampOn ("Use Fresnel Ramp Instead of Color", Float) = 0
		[NoScaleOffset] _FresnelRamp ("Fresnel Ramp", 2D) = "white" {}
		[NoScaleOffset] _FresnelMask ("Fresnel Mask", 2D) = "white" {}
		_FresnelBoost ("Fresnel Boost", Range(0, 20)) = 1
		[Header(Limb Removal)] [Space] [Toggle(LIMBREMOVAL)] _LimbRemovalOn ("Enable Limb Removal", Float) = 0
		_LimbPrimeMask ("Limb Prime Mask", Range(1, 10000)) = 1
		[Header(Print Behavior)] [Space] [Toggle(PRINT_CUTOFF)] _PrintOn ("Enable Printing", Float) = 0
		_SliceHeight ("Slice Height", Range(-25, 25)) = 5
		_SliceBandHeight ("Print Band Height", Range(0, 200)) = 1
		_SliceAlphaDepth ("Print Alpha Depth", Range(0, 1)) = 0.1
		_SliceAlphaTex ("Print Alpha Texture", 2D) = "gray" {}
		_PrintBoost ("Print Color Boost", Range(0, 10)) = 1
		_PrintBias ("Print Alpha Bias", Range(0, 4)) = 0
		_PrintEmissionToAlbedoLerp ("Print Emission to Albedo Lerp", Range(0, 1)) = 0
		[MaterialEnum(BottomUp,0,TopDown,1,BackToFront,3)] _PrintDirection ("Print Direction", Float) = 0
		[NoScaleOffset] _PrintRamp ("Print Ramp", 2D) = "gray" {}
		[Header(Elite Remap)] [Space] [Toggle(ELITE_RAMP)] _EliteRampOn ("Enable Elite Ramp", Float) = 0
		[Toggle(ELITE_LINEAR_BRIGHTNESS)] _EliteLinearBrightnessOn ("Use Linear Brightness (No Gamma)", Float) = 0
		[PerRendererData] _EliteIndex ("Elite Index", Float) = 0
		_EliteBrightnessMin ("Elite Brightness, Min", Range(-10, 10)) = 0
		_EliteBrightnessMax ("Elite Brightness, Max", Range(-10, 10)) = 1
		[HideInInspector] [PerRendererData] _Fade ("Fade", Range(0, 1)) = 1
		[HideInInspector] [PerRendererData] _FlashColor ("Flash Color", Color) = (0,0,0,1)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	Fallback "Diffuse"
}