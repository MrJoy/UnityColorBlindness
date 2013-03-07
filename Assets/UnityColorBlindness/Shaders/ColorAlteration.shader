Shader "Hidden/Color Blindness Effect" {
  Properties {
    _MainTex ("Base (RGB)", RECT) = "white" {}
    _RedTx ("Red Coefficients", Vector) = (1,0,0)
    _GreenTx ("Green Coefficients", Vector) = (0,1,0)
    _BlueTx ("Blue Coefficients", Vector) = (0,0,1)
  }

  SubShader {
    GrabPass { }
    Pass {
      ZTest Always
      Cull Off
      ZWrite Off
      Fog { Mode off }

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #pragma fragmentoption ARB_precision_hint_fastest
      #include "UnityCG.cginc"

      struct appdata {
        float4 vertex : POSITION;
        float4 texcoord : TEXCOORD0;
      };

      struct v2f {
        float4 pos : SV_POSITION;
        float2 uv : TEXCOORD0;
      };

      float4 _GrabTexture_TexelSize;
      v2f vert (appdata v) {
        v2f o;
        o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
        o.uv = v.texcoord.xy;
        //#if SHADER_API_D3D9 || SHADER_API_XBOX360 || SHADER_API_D3D11
        //if (_ColorBuffer_TexelSize.y < 0)
        #if UNITY_UV_STARTS_AT_TOP
        o.uv.y = 1.0 - o.uv.y;
        #endif
        return o;
      }

      sampler2D _GrabTexture;
      float3 _RedTx, _GreenTx, _BlueTx;

      float4 frag(v2f i) : COLOR {
        float4 original = tex2D(_GrabTexture, i.uv);

        return float4(
          (original.r*_RedTx[0])  +(original.g*_RedTx[1])  +(original.b*_RedTx[2]),
          (original.r*_GreenTx[0])+(original.g*_GreenTx[1])+(original.b*_GreenTx[2]),
          (original.r*_BlueTx[0]) +(original.g*_BlueTx[1]) +(original.b*_BlueTx[2]),
          original.a
        );
      }
      ENDCG
    }
  }

  Fallback off
}
