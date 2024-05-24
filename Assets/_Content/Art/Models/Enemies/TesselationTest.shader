Shader "Custom/GeometryShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Geometry" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2g
            {
                float4 pos : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct g2f
            {
                float4 pos : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 _ShadowCoord : TEXCOORD1;
            };

            sampler2D _MainTex;

            v2g vert(appdata v)
            {
                v2g o;
                o.pos = mul(UNITY_MATRIX_M, v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = v.uv;
                return o;
            }

            float3 lineIntersect(float3 minBounds, float3 maxBounds, float3 v0, float3 v1)
            {
                float3 tMin = (minBounds - v0) / (v1 - v0);
                float3 tMax = (maxBounds - v0) / (v1 - v0);
                float3 t1 = min(tMin, tMax);
                float3 t2 = max(tMin, tMax);
                float tNear = max(max(t1.x, t1.y), t1.z);
                float tFar = min(min(t2.x, t2.y), t2.z);

                if (tNear > 0.0 && tNear <= tFar)
                {
                    return v0 + (v1 - v0) * tNear;
                }
                return float3(0.0, 0.0, 0.0);
            }



            [maxvertexcount(76)]
            void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
            {
                g2f o;
                float3 cubeSize = float3(0.25, 0.25, 0.25);
                
                for (uint i = 0; i < 3; i++)
                {
                    v2g v0 = IN[i];
                    v2g v1 = IN[(i + 1) % 3];
                    float3 v0Pos = v0.pos;
                    float3 v1Pos = v1.pos;

                    //Insert v0
                    o.pos = mul(UNITY_MATRIX_VP, v0.pos);
                    UNITY_TRANSFER_FOG(o,o.pos);
                    o.uv = v0.uv;
                    o.normal = v0.normal;
                    o._ShadowCoord = ComputeScreenPos(o.pos);
                    #if UNITY_PASS_SHADOWCASTER
                    o.vertex = UnityApplyLinearShadowBias(o.pos);
                    #endif
                    triStream.Append(o);

                    //Region
                    float3 minCoord = min(v0Pos, v1Pos) / cubeSize;
                    float3 maxCoord = max(v0Pos, v1Pos) / cubeSize;
                
                    for (int x = (int)minCoord.x; x <= (int)maxCoord.x; x++)
                    {
                        for (int y = (int)minCoord.y; y <= (int)maxCoord.y; y++)
                        {
                            for (int z = (int)minCoord.z; z <= (int)maxCoord.z; z++)
                            {
                                float3 cubeCenter = float3(x, y, z) * cubeSize + cubeSize * 0.5;
                                float3 minBounds = cubeCenter - cubeSize * 0.5;
                                float3 maxBounds = cubeCenter + cubeSize * 0.5;
                
                                float3 intersection = lineIntersect(minBounds, maxBounds, v0Pos, v1Pos);
                
                                if (length(intersection) > 0.0)
                                {
                                    //New vertices
                                    float3 v0PosNew = lerp(v0Pos, v1Pos, (length(v0Pos - intersection) + 0.0001) / length(v0Pos - v1Pos));
                                    float3 v1PosNew = lerp(v0Pos, v1Pos, (length(v1Pos - intersection) + 0.0001) / length(v0Pos - v1Pos));
                
                                    o.pos = mul(UNITY_MATRIX_P, float4(intersection, 1.0));
                                    o.normal = v0.normal;
                                    o.uv = v0.uv;
                                    triStream.Append(o);
                
                                    o.pos = mul(UNITY_MATRIX_P, float4(intersection, 1.0));
                                    o.normal = v1.normal;
                                    o.uv = v1.uv;
                                    triStream.Append(o);
                                }
                            }
                        }
                    }
                }
            }

            fixed4 frag(g2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }

            ENDCG
        }
    }
}