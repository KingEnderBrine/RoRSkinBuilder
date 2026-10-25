Shader "StubbedRoR2/Base/Shaders/HGStandardTransparent" {
	Properties {
		[Toggle(USE_DITHER)] _useDither ("Screen Space Dither", Float) = 0
		[Toggle(USE_BLUE)] _useBlue ("Blue Vertex Channel Dither Masking", Float) = 0
		_DitherMask ("Dither Mask", 2D) = "grey" {}
		_DitherMaskScale ("Dither Mask Scale", Range(0, 1000)) = 10
		_Cutoff ("Mask Clip Value", Range(0, 1)) = 0.5
		_AlphaContrast ("Input Alpha Contrast", Range(0, 2)) = 1
		_AlphaMidPoint ("Input Alpha Mid Value", Range(0, 1)) = 0.5
		[Header(Default)] _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB) Specular Scale (A)", 2D) = "white" {}
		_NormalStrength ("Normal Strength", Range(0, 5)) = 1
		_NormalTex ("Normal Map", 2D) = "bump" {}
		_EmColor ("Emission Color", Color) = (0,0,0,1)
		[NoScaleOffset] _EmTex ("Emission Tex (RGB)", 2D) = "white" {}
		_EmPower ("Emission Power", Range(0, 10)) = 1
		_Smoothness ("Smoothness", Range(0, 1)) = 0
		_SecondarySmoothness ("Smoothness for Vertex Color Override", Range(0, 1)) = 0
		[Toggle(FORCE_SPEC)] _ForceSpecOn ("Ignore Diffuse Alpha for Speculars", Float) = 0
		[MaterialEnum(Two Tone,0,Smoothed Two Tone,1,Unlitish,3,Subsurface,4,Grass,5)] _RampInfo ("Ramp Choice", Float) = 0
		[MaterialEnum(Default,0,Environment,1,Character,2,Misc,3)] _DecalLayer ("Decal Layer", Float) = 0
		_SpecularStrength ("Specular Strength", Range(0, 1)) = 0
		_SpecularExponent ("Specular Exponent", Range(0.1, 20)) = 1
		[MaterialEnum(Off,0,Front,1,Back,2)] _Cull ("Cull", Float) = 2
		[Header(Fresnel Emission)] [Toggle(FRESNEL_EMISSION)] _FEON ("Enable Fresnel Emission", Float) = 0
		[NoScaleOffset] _FresnelRamp ("Fresnel Ramp", 2D) = "white" {}
		_FresnelPower ("Fresnel Power", Range(0.1, 20)) = 1
		[NoScaleOffset] _FresnelMask ("Fresnel Mask", 2D) = "white" {}
		_FresnelBoost ("Fresnel Boost", Range(0, 20)) = 1
		[Header(Splatmapping)] [Toggle(SPLATMAP)] _SplatmapOn ("Enable Splatmap", Float) = 0
		_Depth ("Blend Depth", Range(0, 1)) = 0.2
		_SplatmapTex ("Splatmap Tex (RGB)", 2D) = "white" {}
		_SplatmapTileScale ("Splatmap Tile Scale", Range(0, 20)) = 1
		[NoScaleOffset] _GreenChannelTex ("Green Channel Albedo (RGB) Specular Scale (A)", 2D) = "white" {}
		[NoScaleOffset] _GreenChannelNormalTex ("Green Channel Normal Map", 2D) = "bump" {}
		_GreenChannelSmoothness ("Green Channel Smoothness", Range(0, 1)) = 0
		_GreenChannelBias ("Green Channel Bias", Range(-2, 5)) = 0
		[NoScaleOffset] _BlueChannelTex ("Blue Channel Albedo  Specular Scale (A)", 2D) = "white" {}
		[NoScaleOffset] _BlueChannelNormalTex ("Blue Channel Normal Map", 2D) = "bump" {}
		_BlueChannelSmoothness ("Blue Channel Smoothness", Range(0, 1)) = 0
		_BlueChannelBias ("Blue Channel Bias", Range(-2, 5)) = 0
		[Header(Flowmap)] [Toggle(FLOWMAP)] _FlowmapOn ("Enable Flowmap", Float) = 0
		[NoScaleOffset] _FlowTex ("Flow Vector (RG), Noise (B)", 2D) = "bump" {}
		_FlowHeightmap ("Flow Heightmap", 2D) = "white" {}
		_FlowHeightRamp ("Flow Ramp", 2D) = "black" {}
		_FlowHeightBias ("Flow Height Bias", Range(-1, 1)) = 0
		_FlowHeightPower ("Flow Height Power", Range(0.1, 20)) = 1
		_FlowEmissionStrength ("Flow Height Strength", Range(0.1, 20)) = 1
		_FlowSpeed ("Flow Speed", Range(0, 15)) = 1
		_FlowMaskStrength ("Mask Flow Strength", Range(0, 5)) = 0
		_FlowNormalStrength ("Normal Flow Strength", Range(0, 5)) = 1
		_FlowTextureScaleFactor ("Flow Texture Scale Factor", Range(0, 10)) = 1
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
	Fallback "Diffuse"
}