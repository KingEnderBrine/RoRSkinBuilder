Shader "StubbedRoR2/DLC4/TarAnimatedUVCharacter" {
	Properties {
		[Header(Base)] [Space] _BaseColor ("Base Color", Color) = (0.8,0.8,0.8,1)
		_EmColor ("Emission Color", Color) = (0,0,0,1)
		_EmPower ("Emission Power", Range(0, 10)) = 1
		_Metallic ("Metallic", Range(0, 1)) = 0
		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
		[Normal] _NormalTex ("Normal Map", 2D) = "bump" {}
		_NormalStrength ("Normal Strength", Range(0, 2)) = 1
		[Header(Fresnel)] [Space] _FresnelColor ("Fresnel Rim Color", Color) = (0.2,0.6,1,1)
		_FresnelBias ("Fresnel Bias", Float) = 0
		_FresnelScale ("Fresnel Scale", Float) = 1
		_FresnelPower ("Fresnel Power", Float) = 3
		[Header(Tar)] [Space] [Normal] _NormalMapA ("Tar Normal Map A", 2D) = "bump" {}
		_TilingA ("Tiling A", Vector) = (1,1,0,0)
		_MovementA ("Movement A (XY per second)", Vector) = (0.1,0,0,0)
		_NormalIntensityA ("Normal Intensity A", Range(0, 1)) = 1
		[Normal] _NormalMapB ("Tar Normal Map B", 2D) = "bump" {}
		_TilingB ("Tiling B", Vector) = (1,1,0,0)
		_MovementB ("Movement B (XY per second)", Vector) = (-0.05,0.05,0,0)
		_NormalStrengthB ("Normal Strength B", Range(0, 1)) = 1
		[Header(Limb Removal)] [Space] [Toggle(LIMBREMOVAL)] _LimbRemovalOn ("Enable Limb Removal", Float) = 0
		_LimbPrimeMask ("Limb Prime Mask", Range(1, 10000)) = 1
		[Header(Flowmap)] [Space] [Toggle(FLOWMAP)] _FlowmapOn ("Enable Flowmap", Float) = 0
		[Normal] _FlowNormalMap ("Flow Normal Map", 2D) = "bump" {}
		_FlowHeightmap ("Height Mask (A)", 2D) = "white" {}
		_FlowHeightRamp ("Emission Ramp (X Color)", 2D) = "white" {}
		_FlowHeightBias ("Height Bias", Range(-1, 1)) = 0
		_FlowHeightPower ("Height Power", Range(0.1, 20)) = 1
		_FlowEmissionStrength ("Emission Strength", Range(0, 10)) = 2
		_FlowSpeed ("Speed", Range(0, 10)) = 1
		_FlowNormalStrength ("Normal Distortion", Range(0, 2)) = 0.3
		_FlowTextureScaleFactor ("Tiling", Range(0.1, 10)) = 1
		[Header(Vertex Displacement)] [Space] [Toggle(VERTEX_IGNORE_VERTEXCOLOR)] _IgnoreVertexColor ("Ignore Vertex Color", Float) = 0
		_VertexOffsetTex1 ("Vertex Offset Tex 1", 2D) = "gray" {}
		_VertexOffset1Speed ("Vertex Offset 1 Speed", Vector) = (0.2,0.5,0,0)
		_VertexOffset1Tiling ("Vertex Offset 1 Tiling", Vector) = (-6,1,0,0)
		_VertexOffsetTex2 ("Vertex Offset Tex 2", 2D) = "gray" {}
		_VertexOffset2Speed ("Vertex Offset 2 Speed", Vector) = (0.5,0,0,0)
		_VertexOffset2Tiling ("Vertex Offset 2 Tiling", Vector) = (5,0.5,0,0)
		_VertexOffsetPower ("Vertex Offset Power", Range(0, 12)) = 5
		_VertexInflate ("Vertex Inflate", Range(0, 100)) = 2
		_VertexScale ("Vertex Scale", Float) = 10000
		[Header(Rendering)] [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Culling", Float) = 2
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
}