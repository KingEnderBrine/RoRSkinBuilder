Shader "StubbedRoR2/Base/Shaders/HGStandardTriplanarSplat" {
	Properties {
		[Header(Shader Features)] [Header(Hopoo Shader Type ...................................................................................................)] [MaterialEnum(Two Tone,0,Smoothed Two Tone,1,Unlitish,3,Subsurface,4,Grass,5)] _RampInfo ("Ramp Choice", Float) = 0
		[MaterialEnum(Default,0,Environment,1,Character,2, Misc,3)] _DecalLayer ("Decal Layer", Float) = 0
		[MaterialEnum(Off,0,Front,1,Back,2)] _Cull ("Cull", Float) = 2
		[Space(25)] [Header(Emissive Map................................................................................................................)] [Toggle(EMISSIVE_MAP)] _EMON ("Use Emissive map", Float) = 0
		_EmPower ("Emission Power", Range(0, 50)) = 1
		_EmTex ("Emission Tex (RGB)", 2D) = "white" {}
		_EmColor ("Emission Color", Color) = (0,0,0,1)
		[Space(25)] [Header(Fresnel Emission............................................................................................................)] [Toggle(FRESNEL_EMISSION)] _FEON ("Enable Fresnel Emission", Float) = 0
		[NoScaleOffset] _FresnelRamp ("Fresnel Ramp", 2D) = "white" {}
		_FresnelPower ("Fresnel Power", Range(0.1, 20)) = 1
		_FresnelMask ("Fresnel Mask", 2D) = "white" {}
		_FresnelBoost ("Fresnel Boost", Range(0, 100)) = 1
		[Space(25)] [Header(MAIN Blend between channels  ...............................................................................................)] [Header(.)] _RedChannelBias ("Red Channel Bias", Range(-2, 5)) = 0
		_GreenChannelBias ("Green Channel Bias", Range(-2, 5)) = 0
		_BlueChannelBias ("Blue Channel Bias", Range(-2, 5)) = 0
		_Depth ("Blend Depth", Range(0, 1)) = 0.2
		[Space(25)] [Header(Maps......................................................................................................................)] [Toggle(USE_ALPHA_AS_MASK)] _MaskOn ("Use Alpha Channels as Weight Mask", Float) = 0
		_TextureFactor ("Tiling Factor", Range(0, 1)) = 1
		_overallColor ("Overall Tint", Color) = (1,1,1,1)
		_Color ("Main Albedo Tint", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGBA)", 2D) = "white" {}
		[Space(10)] [Toggle(USE_VERTEX_COLORS)] _MixColorsOn ("Use Vertex Colors instead of RGB Mask", Float) = 0
		[Toggle(MIX_VERTEX_COLORS)] _ColorsOn ("Use Both Vertex Colors and RGB Mask", Float) = 0
		_SplatmapTex ("RGB Mask Tex (RGB)", 2D) = "white" {}
		[Toggle(USE_CHANNELS)] _ChannelOn ("Use Green and Blue Channel Maps", Float) = 1
		_GreenTint ("Green Channel Tint", Color) = (1,1,1,1)
		_GreenSplatTex ("Green Channel Texture (RGBA)", 2D) = "white" {}
		_BlueTint ("Blue Channel Tint", Color) = (1,1,1,1)
		_BlueSplatTex ("Blue Channel Texture (RGBA)", 2D) = "white" {}
		[Space(10)] [Toggle(ADD_ALPHAMASK)] _HeightOn ("Blend Height with Vertex Colors", Float) = 0
		_HeightTex ("Height (RGB) - height influences Green / Blue distribution", 2D) = "white" {}
		_HeigtIntensity ("Height Intensity", Range(0, 2)) = 0
		_HeightCap ("Height MidPoint", Range(0, 1)) = 0
		[Space(10)] _NormalTex ("Normal Map", 2D) = "bump" {}
		_NormalStrength ("Normal Strength", Range(0, 5)) = 1
		_NormalsTextureFactor ("Normal Texture Factor", Range(0, 10)) = 1
		[Space(25)] [Header(Splat......................................................................................................................)] _RedTint ("Combined Splatmap Tint", Color) = (1,1,1,1)
		_RandomSplatColor ("Main Splat Tint", Color) = (1,1,1,1)
		_RedSplatTex ("Main Splat Texture (RGBA)", 2D) = "grey" {}
		_RandomSplatIntensity ("Main Splat Intensity", Range(0, 5)) = 1
		_SplatTextureFactor ("Main Splat Texture Tiling", Range(0, 4)) = 1
		[Space(10)] [Toggle(SEC_SPLAT)] _UseSecSplat ("Use Detail Splat", Float) = 0
		[Toggle(SPLAT_ON_TOP)] _SPLATTOP ("Put Detail Splat On top", Float) = 0
		[MaterialEnum(Multiply,0,Overlay,1,Screen,3,Softlight,4)] _SplatInfo ("Detail Splat Blending Mode Choice", Float) = 0
		_SecSplatColor ("Detail Splat Tint", Color) = (1,1,1,1)
		_secSplatTex ("Detail Splat Texture (RGBA)", 2D) = "grey" {}
		_SecSplatIntensity ("Detail Splat Intensity - (0-3)", Range(0, 20)) = 0
		_SecSplatTextureFactor ("Detail Splat Texture Tiling", Range(0, 4)) = 1
		[Space(25)] [Header(Smoothness)] _RedChannelSmoothness ("Red Channel Smoothness", Range(0, 1)) = 0
		_RedChannelSpecularStrength ("Red Channel Specular Strength", Range(0, 1)) = 0
		_RedChannelSpecularExponent ("Red Channel Specular Exponent", Range(0.1, 20)) = 0
		_GreenChannelSmoothness ("Green Channel Smoothness", Range(0, 1)) = 0
		_GreenChannelSpecularStrength ("Green Channel Specular Strength", Range(0, 1)) = 0
		_GreenChannelSpecularExponent ("Green Channel Specular Exponent", Range(0.1, 20)) = 0
		_BlueChannelSmoothness ("Blue Channel Smoothness", Range(0, 1)) = 0
		_BlueChannelSpecularStrength ("Blue Channel Specular Strength", Range(0, 1)) = 0
		_BlueChannelSpecularExponent ("Blue Channel Specular Exponent", Range(0.1, 20)) = 0
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