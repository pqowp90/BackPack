Shader "Custom/ProjectiveTexture" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _MainTex_ST ("Texture Scale/Offset", Vector) = (1, 1, 0, 0)
        _ScaleFactor ("Scale Factor", Range(0.1, 10.0)) = 1.0
    }

    SubShader {
        Pass {
            Tags { "RenderType"="Opaque" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ScaleFactor;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float3 uvw = float3(i.uv * _MainTex_ST.xy + _MainTex_ST.zw, _ScaleFactor);
                float2 uv = uvw.xy / uvw.z;
                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
}