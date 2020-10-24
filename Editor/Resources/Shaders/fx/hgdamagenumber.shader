Shader "Fake RoR/Hopoo Games/FX/Damage Number" {
	Properties {
		[HDR] _TintColor ("Tint", Vector) = (1,1,1,1)
		_CritColor ("Crit Color", Vector) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_CharacterLimit ("Character Limit", Float) = 3
	}
	Fallback "Diffuse"
}