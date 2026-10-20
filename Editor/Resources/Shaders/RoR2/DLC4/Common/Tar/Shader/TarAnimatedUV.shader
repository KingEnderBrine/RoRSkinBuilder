Shader "StubbedRoR2/DLC4/TarAnimatedUV" {
	Properties {
		_Cutoff ("Mask Clip Value", Float) = 0.5
		_BaseColor ("BaseColor", Color) = (0,0,0,0)
		_FresnelBias ("FresnelBias", Float) = 0
		_FresnelScale ("FresnelScale", Float) = 0
		_FresnelPower ("FresnelPower", Float) = 0
		_FresnelColor ("FresnelColor", Color) = (0,0,0,0)
		_Metallic ("Metallic", Range(0, 1)) = 0
		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
		[SingleLineTexture] [Normal] _NormalMapA ("NormalMapA", 2D) = "bump" {}
		_TilingA ("TilingA", Vector) = (0,0,0,0)
		_MovementA ("MovementA", Vector) = (0,0,0,0)
		[SingleLineTexture] [Normal] _NormalMapB ("NormalMapB", 2D) = "bump" {}
		_NormalIntensityA ("NormalIntensityA", Range(0, 1)) = 0
		_TilingB ("TilingB", Vector) = (0,0,0,0)
		_MovementB ("MovementB", Vector) = (0,0,0,0)
		_NormalStrengthB ("NormalStrengthB", Range(0, 1)) = 0
		_LimbPrimeMask ("LimbPrimeMask", Range(1, 1000)) = 1
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
	//CustomEditor "AmplifyShaderEditor.MaterialInspector"
}