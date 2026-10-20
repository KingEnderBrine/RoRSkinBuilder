Shader "StubbedRoR2/Base/Shaders/HGTriplanarTerrainBlend_Emissive" {
	Properties {
		[Header(Hopoo Shader Type ...................................................................................................)] [MaterialEnum(Two Tone,0,Smoothed Two Tone,1,Unlitish,3,Subsurface,4,Grass,5)] _RampInfo ("Ramp Choice", Float) = 0
		[MaterialEnum(Default,0,Environment,1,Character,2, Misc,3)] _DecalLayer ("Decal Layer", Float) = 0
		[MaterialEnum(Off,0,Front,1,Back,2)] _Cull ("Cull", Float) = 2
		[Space(25)] [Header(Emission ............................................................................................................)] [Toggle(USE_EMISSIVE)] _EmON ("Use Emission Channels", Float) = 0
		[MaterialEnum(Triplanar,0,UVs,1)] _EmissiveUseCase ("Select Projection", Float) = 0
		[MaterialEnum(Red,0,Green,1,Blue,2, RGB,3)] _ChannelSelector ("Background Texture on x channel", Float) = 1
		_emissiveTex ("Emissive Texture", 2D) = "black" {}
		_backgroundTex ("Background Texture", 2D) = "black" {}
		_emColorRed ("Red channel Emission Tint", Color) = (0,0,0,0)
		_emColorGreen ("Green channel Emission Tint", Color) = (0,0,0,0)
		_emColorBlue ("Blue channel Emission Tint", Color) = (0,0,0,0)
		_emBackground ("Backgrounnd Emission Tint", Vector) = (0,0,0,0)
		_emIntensityRed ("Red Emission Power", Range(0, 50)) = 0
		_emIntensityGreen ("Green Emission Power", Range(0, 50)) = 0
		_emIntensityBlue ("Blue Emission Power", Range(0, 50)) = 0
		_emIntensityBackground ("Background Emission Power", Range(0, 50)) = 0
		_emissiveTextureScale ("Emissive Texture Scale", Range(0, 5)) = 0
		[Space(25)] [Header(MAIN Blend between channels  ........................................................................................)] [Header(.)] [Toggle(USE_VERTEX_COLORS)] _ColorsOn ("Use Vertex Colors Instead of RGB Splatmap", Float) = 0
		[Toggle(MIX_VERTEX_COLORS)] _MixColorsOn ("Mix Vertex Colors with RGB splatmap", Float) = 0
		[Toggle(USE_ALPHA_AS_MASK)] _MaskOn ("USE texture alpha channel to blend between textures", Float) = 0
		[Toggle(USE_VERTICAL_BIAS)] _VerticalBiasOn ("Bias Green Channel to Vertical - saturate green on Y - Legacy", Float) = 0
		[Toggle(DOUBLESAMPLE)] _DoublesampleOn ("Double Sample UVs", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] [Header(.)] [Toggle(CHANNEL_BLEND)] _channelBlend ("Blur Vertex colors on separate Channels", Float) = 0
		[MaterialEnum(Lerp,0,Additive,1)] _useMINMAX ("Alpha Blurring Type", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _vertexBlend ("All channel Blur", Range(0, 1)) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _vertexBlendR ("Vertex Blur Red ", Range(0, 1)) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _vertexBlendG ("Vertex Blur Green", Range(0, 1)) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _vertexBlendB ("Vertex Blur Blue", Range(0, 1)) = 0
		[Header(.)] _Depth ("Blend Depth", Range(0, 1)) = 0.2
		_RedChannelBias ("Red Channel Bias", Range(-2, 5)) = 0
		_GreenChannelBias ("Green Channel Bias", Range(-2, 5)) = 0
		_BlueChannelBias ("Blue Channel Bias", Range(-2, 5)) = 0
		[Space(25)] [Header(MAPS  ................................................................................................................)] [Space(10)] _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_TextureFactor ("Texture Scale", Range(0, 1)) = 1
		[Space(10)] [Header(Splatmap)] _SplatmapTex ("RGB splatmap Tex", 2D) = "white" {}
		[Header(Normal)] _NormalTex ("Normal Tex (RGB)", 2D) = "bump" {}
		_NormalStrength ("Normal Strength", Range(0, 1)) = 1
		[Header(Red Channel)] _RColor (" Tint", Color) = (1,1,1,1)
		_RedChannelTopTex ("Red Channel Top Albedo (RGB) alpha mask (A)", 2D) = "white" {}
		_RedChannelSideTex ("Red Channel Side Albedo (RGB) alpha mask (A)", 2D) = "white" {}
		[Header(Green Channel)] _GColor (" Tint", Color) = (1,1,1,1)
		_GreenChannelTex ("Green Channel Albedo (RGB) alpha mask (A)", 2D) = "white" {}
		[Header(Blue Channel)] _BColor (" Tint", Color) = (1,1,1,1)
		_BlueChannelTex ("Blue Channel Albedo (RGB) alpha mask (A)", 2D) = "white" {}
		[Space(25)] [Header(Specular  ............................................................................................................)] [Space(10)] [Header(Red Channel)] _RedChannelSmoothness ("Red Channel Smoothness", Range(0, 1)) = 0
		_RedChannelSpecularStrength ("Red Channel Specular Strength", Range(0, 1)) = 0
		_RedChannelSpecularExponent ("Red Channel Specular Exponent", Range(0.1, 20)) = 0
		[Header(Green Channel)] _GreenChannelSmoothness ("Green Channel Smoothness", Range(0, 1)) = 0
		_GreenChannelSpecularStrength ("Green Channel Specular Strength", Range(0, 1)) = 0
		_GreenChannelSpecularExponent ("Green Channel Specular Exponent", Range(0.1, 20)) = 0
		[Header(Blue Channel)] _BlueChannelSmoothness ("Blue Channel Smoothness", Range(0, 1)) = 0
		_BlueChannelSpecularStrength ("Blue Channel Specular Strength", Range(0, 1)) = 0
		_BlueChannelSpecularExponent ("Blue Channel Specular Exponent", Range(0.1, 20)) = 0
		[Space(25)] [Header(Other  ............................................................................................................)] [Space(10)] [Toggle(MICROFACET_SNOW)] _SnowOn ("Treat G.Channel as Snow", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}