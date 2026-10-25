Shader "StubbedRoR2/Base/Shaders/HGDamageNumber" {
	Properties {
		[HDR] _TintColor ("Tint", Color) = (1,1,1,1)
		_CritColor ("Crit Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_CharacterLimit ("Character Limit", Float) = 3
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
}