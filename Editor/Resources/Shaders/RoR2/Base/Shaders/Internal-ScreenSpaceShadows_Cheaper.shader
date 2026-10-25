Shader "StubbedRoR2/Base/Shaders/Internal-ScreenSpaceShadows_Cheaper" {
	Properties {
		_ShadowMapTexture ("", any) = "" {}
		_ODSWorldTexture ("", 2D) = "" {}
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