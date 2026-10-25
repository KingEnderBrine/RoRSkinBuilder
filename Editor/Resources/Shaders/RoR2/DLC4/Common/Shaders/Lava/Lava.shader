Shader "StubbedRoR2/DLC4/Lava" {
	Properties {
		_LavaSpecular ("Lava Specular", Float) = 0
		_LavaSmoothness ("Lava Smoothness", Float) = 0
		[Header(Main Texturing)] [Header(_)] [SingleLineTexture] _MainTexture ("Main Texture", 2D) = "white" {}
		_MainColor ("Main Color", Color) = (1,1,1,0)
		_MainTiling ("Main Tiling", Vector) = (1,1,0,0)
		[Header(Light and Shadow)] _LightHighlightColor ("Light Highlight Color", Color) = (0,0,0,0)
		_LightShadowColor ("Light Shadow Color", Color) = (1,1,1,0)
		_LightPower ("Light Power", Range(0, 5)) = 0
		_LightRampThreshold ("Light Ramp Threshold", Range(0, 1)) = 0.001
		_LightRampSmooth ("Light Ramp Smooth", Range(0.001, 1)) = 0.3764768
		[Header(Pulse Effect)] _PulseMask1 ("Pulse Mask 1", 2D) = "white" {}
		_PulseMask2 ("Pulse Mask 2", 2D) = "white" {}
		_PulseEmissionColor ("Pulse Emission Color", Color) = (1,1,1,1)
		_PulseEmissionMax ("Pulse Emission Max", Range(0.74, 1)) = 0.7967194
		_PulseEmissionMin ("Pulse Emission Min", Range(0.15, 1)) = 0.15
		_PulseSpeed ("Pulse Speed", Float) = 1
		[Header(Fresnel)] [Header(_)] _FresnelColor ("Fresnel Color", Color) = (1,0.682353,0,1)
		_FresnelBias ("Fresnel Bias", Range(0, 1)) = 0
		_FresnelScale ("Fresnel Scale", Range(0, 1)) = 0.4169914
		_FresnelPower ("Fresnel Power", Range(0, 5)) = 2.617954
		[Header(Foam)] _FoamTexture ("Foam Texture", 2D) = "white" {}
		_FoamSmoothness ("Foam Smoothness", Float) = 0
		_FoamSpecular ("Foam Specular", Float) = 0
		_FoamFalloff ("Foam Falloff", Float) = 0
		_FoamDepth ("Foam Depth", Float) = 0
		[Header(Vertex Animation)] _VertexAnimationGuide ("Vertex Animation Guide", 2D) = "white" {}
		_VertexAnimationHeight ("Vertex Animation Height", Range(0, 5)) = 0
		_VertexAnimationSpeed ("Vertex Animation Speed", Range(0, 5)) = 0
		[Header(UV Wave Distortion)] [Space] _UVWaveAmplitude ("UV Wave Amplitude", Range(0.001, 0.5)) = 0.5
		_UVWaveSpeed ("UV Wave Speed", Range(0, 5)) = 1
		_UVWaveFrequency ("UV Wave Frequency", Range(0, 10)) = 1
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] __dirty ("", Float) = 1
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
	//CustomEditor "ASEMaterialInspector"
}