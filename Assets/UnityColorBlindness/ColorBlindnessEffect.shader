// Upgrade NOTE: replaced 'samplerRECT' with 'sampler2D'
// Upgrade NOTE: replaced 'texRECT' with 'tex2D'

Shader "Hidden/Color Blindness Effect" {
Properties {
	_MainTex ("Base (RGB)", RECT) = "white" {}
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#include "UnityCG.cginc"

uniform sampler2D _MainTex;

float4 frag (v2f_img i) : COLOR
{	
	float4 original = tex2D(_MainTex, i.uv);
	float tmp = (original.r + original.g) * 0.5;
	original.r = original.g = tmp;
	
	return original;
}
ENDCG

	}
}

Fallback off

}