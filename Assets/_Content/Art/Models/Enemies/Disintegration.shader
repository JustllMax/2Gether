Shader "2Gether/Disintegration"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            Tags { "Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent" }
            LOD 200
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float disintegration : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _InstanceDisintegrationTime;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);

                o.disintegration = clamp(pow(3.25, _InstanceDisintegrationTime - 3.0) - 0.03, 0.0, 1.0);

                float4 worldPos = mul(UNITY_MATRIX_M, v.vertex);
                worldPos += float4(o.normal, 0.0) * o.disintegration;
                o.pos = mul(UNITY_MATRIX_VP, worldPos);

                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);

                float4 finalColor = lerp(texColor, float4(0.0, 0.5, 0.6, 1.0), i.disintegration);
                finalColor.a = 1.0 - i.disintegration;
                if (finalColor.a < 0.05)
				{
					discard;
				}
                return finalColor;
            }
            ENDCG
        }

        Pass
        {
            Tags {"LightMode"="ShadowCaster"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f { 
                float4 pos : SV_POSITION;
            };
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            float _InstanceDisintegrationTime;

            v2f vert(appdata  v)
            {
                v2f o;
                float3 normal = UnityObjectToWorldNormal(v.normal);

                float disintegration = clamp(pow(3.25, _InstanceDisintegrationTime - 3.0) - 0.03, 0.0, 1.0);

                float4 worldPos = mul(UNITY_MATRIX_M, v.vertex);
                worldPos += float4(normal, 0.0) * disintegration;
                o.pos = mul(UNITY_MATRIX_VP, worldPos);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
