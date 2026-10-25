Shader "StubbedRoR2/DLC4/Enforcer/ShieldParry" {
	Properties {
		[HDR] _Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture2D", 2D) = "white" {}
		_InsidePattern ("InsidePatternTexture", 2D) = "white" {}
		_Mask ("Mask", 2D) = "white" {}
		_SmoothStep ("Smoothstep", Vector) = (1,1,1,1)
		_RippleT ("RippleT", Float) = 1
		_RippleS ("RippleS", Range(-1, 1)) = 0.1
		_RippleThick ("RippleThick", Float) = 0.1
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
}