Shader "StubbedRoR2/DLC4/Enforcer/Shader_HexagonEnergy_BuildIn" {
	Properties {
		[Header(MAIN TEXTURE)] [Space(10)] _MainTexture ("MainTexture", 2D) = "white" {}
		[Header(NOISE)] [Space(10)] _NoiseTexture ("NoiseTexture", 2D) = "white" {}
		_NoiseDarkTonesEdge ("NoiseDarkTonesEdge", Float) = 0
		_NoiseLightTonesEdge ("NoiseLightTonesEdge", Float) = 0
		_NoiseIntensity ("NoiseIntensity", Range(0, 100)) = 1
		[Header(COLOR)] [Space(10)] [HDR] _Color ("Color", Color) = (1,1,1,1)
		[Header(CIRCLE MASK)] [Space(10)] _InnerCircleMaskControl ("InnerCircleMaskControl", Float) = -0.33
		_OuterCircleMaskControl ("OuterCircleMaskControl", Float) = -0.16
		[HideInInspector] _texcoord ("", 2D) = "white" {}
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
}