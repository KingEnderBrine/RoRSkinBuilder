Shader "StubbedRoR2/Base/Shaders/HGFlag" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "white" {}
		[Toggle(EFFECT_HUE_VARIATION)] _EFFECT_HUE ("Enable Hue Variation", Float) = 1
		_HueVariation ("Hue Variation", Vector) = (1,0.5,0,0.1)
		[Toggle(EFFECT_BUMP)] _EFFECT_BUMP ("Enable Normals", Float) = 0
		_Scroll ("Scroll Speed (XY), Distortion Noise Scale (ZW)", Vector) = (8,1,1,1)
		_Cutoff ("Cutoff Alpha", Range(0, 1)) = 0.5
		_VertexOffsetStrength ("Vertex Offset Strength", Range(0, 5)) = 0.25
		_WindVector ("Wind Offset Vector Start", Vector) = (0,0,0,0)
		_WindVectorEnd ("Wind Offset Vector End", Vector) = (0,0,0,0)
		_WindRNG ("Wind RNG (xyz=cosine scalars, w=intensity)", Vector) = (1,4,8,3)
		_TimeScale ("Time speed scale", Float) = 1
		_Smoothness ("Smoothness", Range(0, 1)) = 0
		[MaterialEnum(Two Tone,0,Smoothed Two Tone,1,Unlitish,3,Subsurface,4)] _RampInfo ("Ramp Choice", Float) = 0
		_SpecularStrength ("Specular Strength", Range(0, 1)) = 0
		_SpecularExponent ("Specular Exponent", Range(0, 20)) = 1
		[Toggle(ROTATE_UV)] _RotateUV ("Rotate UVs", Float) = 0
		[Toggle(FLIP_V)] _FlipV ("Flip UV.y for wind", Float) = 0
		_LockStart ("Lock Center", Float) = 1
		_LockWidth ("Lock Radius", Float) = 1
		_FlagRange ("Flag Coordinate Range (xy=xMin, xMax)", Vector) = (0,1,0,0)
		_FlapLength ("Wave Length", Float) = 1
		_FlapLengthenOnDown ("Wave Lengthen Down Flag", Float) = 1
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
	Fallback "Transparent/Cutout/VertexLit"
}