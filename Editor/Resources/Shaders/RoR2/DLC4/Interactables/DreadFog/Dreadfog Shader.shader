Shader "StubbedRoR2/DLC4/Dreadfog Shader" {
	Properties {
		[MaterialEnum(None,0,Front,1,Back,2)] _Cull ("Culling Mode", Float) = 0
		_InvFade ("Soft Factor", Range(0, 2)) = 0.1
		_DepthOffset ("_DepthOffset", Range(-10, 10)) = 0
		_fade ("Fade", Range(0, 1)) = 1
		_FadeCloseDistance ("Fade Close Distance", Range(0, 1)) = 0.5
		[Toggle(FADECLOSE)] _FadeCloseOn ("Fade when near camera", Float) = 0
		[Header(_)] [Header(Side Scrolling)] [Header(_)] _XZScrollColor1 ("XZ Scroll Color 1", Color) = (1,1,1,1)
		_XZScrollTex1 ("XZ Scroll Tex 1", 2D) = "white" {}
		_XZScrollTiling1 ("XZ Scroll Tiling 1", Vector) = (-6,1,0,0)
		_XZScrollSpeed1 ("XZ Scroll Speed 1", Vector) = (0.2,0.5,0,0)
		_XZScrollColor2 ("XZ Scroll Color 2", Color) = (1,1,1,1)
		_XZScrollTex2 ("XZ Scroll Tex 2", 2D) = "white" {}
		_XZScrollTiling2 ("XZ Scroll Tiling 2", Vector) = (5,0.5,0,0)
		_XZScrollSpeed2 ("XZ Scroll Speed 2", Vector) = (0.5,-0.5,0,0)
		[HideInInspector] _TextureSample5 ("Texture Sample 5", 2D) = "bump" {}
		[Header(_)] [Header(Top and Bottom Rotation)] [Header(_)] _YRotationTex1 ("Y Rotation Tex 1", 2D) = "white" {}
		_YRotationTiling1 ("Y Rotation Tiling 1", Vector) = (0.07,0.07,0,0)
		[HideInInspector] _TextureSample4 ("Texture Sample 4", 2D) = "bump" {}
		_YRotationSpeed1 ("Y Rotation Speed 1", Vector) = (0.2,0,0,0)
		_YRotationTex2 ("Y Rotation Tex 2", 2D) = "white" {}
		_YRotationTiling2 ("Y Rotation Tiling 2", Vector) = (0.07,0.07,0,0)
		_YRotationSpeed2 ("Y Rotation Speed 2", Vector) = (-0.68,0,0,0)
		[Header(_)] [Header(Top  and Bottom Falloff)] [Header(_)] _TopCoverageAmount ("Top Coverage Amount", Range(-1, 2)) = 0.11
		_TopCoverageFalloff ("Top Coverage Falloff", Range(0.01, 4)) = 3.38
		_BottomCoverageAmount ("Bottom Coverage Amount", Range(-1, 2)) = 1.08
		_BottomCoverageFalloff ("Bottom Coverage Falloff", Range(0.01, 4)) = 2.1
		[Header(_)] [Header(Vertex Displacement)] [Header(_)] _VertexOffsetTex1 ("Vertex Offset Tex 1", 2D) = "white" {}
		_VertexOffset1Speed ("Vertex Offset 1 Speed", Vector) = (0.2,0.5,0,0)
		_VertexOffset1Tiling ("Vertex Offset 1 Tiling", Vector) = (-6,1,0,0)
		_VertexOffsetTex2 ("Vertex Offset Tex 2", 2D) = "white" {}
		_VertexOffset2Speed ("Vertex Offset 2 Speed", Vector) = (0.5,0,0,0)
		_VertexOffset2Tiling ("Vertex Offset 2 Tiling", Vector) = (5,0.5,0,0)
		_VertexOffsetPower ("Vertex Offset Power", Float) = 0.12
		[Header(_)] [Header(Bottom Color)] [Header(_)] _BottomColor ("Bottom Color ", Color) = (0.2941177,0.05882353,0.3960785,1)
		_BottomColorCoverageAmount ("Bottom Color Coverage Amount", Range(-1, 2)) = 0.8494361
		_BottomColorCoverageFalloff ("Bottom Color Coverage Falloff", Range(0.01, 4)) = 1.277708
		[ToggleUI] [HideInInspector] _WorldtoObjectSwitch ("World to Object Switch", Range(0, 1)) = 0
		[HideInInspector] _XTex ("X Tex", 2D) = "white" {}
		[HideInInspector] _TopNormal ("Top Normal", 2D) = "bump" {}
		[HideInInspector] _TriplanarNormal ("Triplanar Normal", 2D) = "bump" {}
		[Header(_)] [Header(Fresel Fade)] [Header(_)] _FresnelBias ("Fresnel Bias", Float) = -0.69
		_FresnelScale ("Fresnel Scale", Float) = 0.37
		_FresnelPower ("Fresnel Power", Float) = -1.87
		_FresnelSize ("Fresnel Size", Range(0, 1)) = 0.21
		[HideInInspector] _ZTex ("Z Tex", 2D) = "white" {}
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
	//CustomEditor "ASEMaterialInspector"
}