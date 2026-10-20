Shader "StubbedRoR2/DLC4/ReaperShader" {
	Properties {
		[Header(Base Colors)] [Space] _PrimaryColor ("Primary Color", Color) = (0.1294118,0.9490197,0.1294118,1)
		_SecondaryColor ("Secondary Color", Color) = (0,0,0,0)
		[Header(Diffuse Map)] [Space] [Toggle(DIFFUSE_MAP)] _DiffuseMapOn ("Enable Diffuse Map", Float) = 0
		_MainTex ("Diffuse Map", 2D) = "white" {}
		_MainColor ("Diffuse Tint", Color) = (1,1,1,1)
		[MaterialEnum(Textured Emission,0,Lit Albedo,1)] _DiffuseMapMode ("Diffuse Map Mode", Float) = 0
		[Header(Specular and Lighting)] [Space] _SpecularStrength ("Specular Strength", Range(0, 1)) = 0
		_SpecularExponent ("Specular Exponent", Range(0, 20)) = 1
		_Smoothness ("Smoothness", Range(0, 1)) = 0.9
		[MaterialEnum(Two Tone,0,Smoothed Two Tone,1,Unlitish,3,Subsurface,4,Grass,5)] _RampInfo ("Ramp Choice", Float) = 0
		[MaterialEnum(Default,0,Environment,1,Character,2,Misc,3)] _DecalLayer ("Decal Layer", Float) = 2
		[Header(Fake Lighting)] [Space] _FakeLightingMask ("Fake Lighting Mask", 2D) = "white" {}
		_LightPower ("Light Power", Range(0, 10)) = 1.89
		_LightIntensity ("Light Intensity", Range(0, 1)) = 1
		[Header(Vertical Gradient)] [Space] _VerticalGradientMask ("Vertical Gradient Mask", 2D) = "white" {}
		_GradientMaskPower ("Gradient Mask Power", Range(0, 10)) = 4.18
		[Header(Caustics)] [Space] _CausticsColor ("Caustics Color", Color) = (0,0,0,0)
		_CausticsPower ("Caustics Power", Range(0, 10)) = 0.32
		_CausticsIntensity ("Caustics Intensity", Range(0, 10)) = 8.78
		[Header(Caustics Layer A)] [Space] _CausticsTextureA ("Caustics Texture A", 2D) = "white" {}
		_CausticsScrollSpeedA ("Caustics Scroll Speed A", Vector) = (0.1,0.25,0.7,0)
		_CausticsPowerA ("Caustics Power A", Range(0, 10)) = 4.9
		_CausticsIntensityA ("Caustics Intensity A", Range(0, 10)) = 1
		[Header(Caustics Layer B)] [Space] _CausticsTextureB ("Caustics Texture B", 2D) = "white" {}
		_CausticsScrollSpeedB ("Caustics Scroll Speed B", Vector) = (-0.4,-0.1,0.1,0)
		_CausticsPowerB ("Caustics Power B", Range(0, 10)) = 3.18
		_CausticsIntensityB ("Caustics Intensity B", Range(0, 10)) = 1
		[Header(Fresnel Emission)] [Space] [Toggle(FRESNEL_EMISSION)] _FresnelEmissionOn ("Enable Fresnel Emission", Float) = 0
		[HDR] _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
		[Toggle(FRESNEL_RAMP)] _FresnelRampOn ("Use Fresnel Ramp Instead of Color", Float) = 0
		[NoScaleOffset] _FresnelRamp ("Fresnel Ramp", 2D) = "white" {}
		[NoScaleOffset] _FresnelMask ("Fresnel Mask", 2D) = "white" {}
		_FresnelPower ("Fresnel Power", Range(0.1, 20)) = 1
		_FresnelBoost ("Fresnel Boost", Range(0, 20)) = 1
		_InnerGlowIntensity ("Inner Glow Intensity", Range(-20, 20)) = 0
		[HDR] _InnerGlowColor ("Inner Glow Color", Color) = (1,1,1,1)
		_InnerGlowPower ("Inner Glow Power", Range(0.1, 20)) = 1
		[Toggle(INNER_GLOW_USE_MASK)] _InnerGlowUseMaskOn ("Inner Glow Uses Fresnel Mask", Float) = 0
		[Header(Screenspace Dithering)] [Space] [HideInInspector] [PerRendererData] _Fade ("Fade", Range(0, 1)) = 1
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
		[Toggle(ELITE_LINEAR_BRIGHTNESS)] _EliteLinearBrightnessOn ("Use Linear Brightness", Float) = 0
		[PerRendererData] _EliteIndex ("Elite Index", Float) = 0
		_EliteBrightnessMin ("Elite Brightness Min", Range(-10, 10)) = 0
		_EliteBrightnessMax ("Elite Brightness Max", Range(-10, 10)) = 1
		[HideInInspector] [PerRendererData] _FlashColor ("Flash Color", Color) = (0,0,0,1)
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
	Fallback "Diffuse"
}