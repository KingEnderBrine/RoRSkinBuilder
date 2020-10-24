Shader "Fake RoR/Hopoo Games/FX/Distortion" {
	Properties {
		[HDR] _Colour ("Colour", Vector) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_BumpMap ("Bump Texture", 2D) = "bump" {}
		_Magnitude ("Magnitude", Range(0, 10)) = 0.05
	}
	Fallback "Diffuse"
}