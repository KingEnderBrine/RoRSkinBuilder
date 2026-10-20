Shader "StubbedRoR2/DLC4/Enforcer/Shader_ParticleCustom_BuildIn" {
	Properties {
		[Header(Colors)] [Space(10)] [HDR] _LightColor ("LightColor", Color) = (0,0,0,0)
		[HDR] _DarkColor ("DarkColor", Color) = (0,0,0,0)
		[Header(Textures)] [Space(10)] MainTex ("MainTex", 2D) = "white" {}
		[HideInInspector] _texcoord ("", 2D) = "white" {}
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
}