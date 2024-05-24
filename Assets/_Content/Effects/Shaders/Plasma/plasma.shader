Shader "2Gether/Plasma"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SurfaceSize ("Surface Size", Float) = 1.0
        _AnimationSpeed ("Animation Speed", Float) = 1.0
        _EmissionMin ("Min Emission", Float) = 1.0
        _EmissionMax ("Max Emission", Float) = 1.0
        _FresnelPower ("Fresnel Power", Float) = 5.0
        _FresnelIntensity ("Fresnel Intensity", Float) = 1.0
        _ColorOverride ("Color Override", Color) = (0.0,0.0,0.0,0.0)
    }
    
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent" }
        LOD 100
        
        Cull Back
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ZTest Less
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 viewDir : TEXCOORD1;
                float fresnel : TEXCOORD2;
            };
            
            sampler2D _MainTex;
            float _AnimationSpeed;
            float _EmissionMin;
            float _EmissionMax;
            float _SurfaceSize;
            float _FresnelPower;
            float _FresnelIntensity;

            float4 _MainTex_TexelSize;
            float4 _ColorOverride;

            v2f vert (appdata v)
            {
                v2f o;

                float4 offset = float4(sin(v.vertex.y * _Time.y*3+2), cos(v.vertex.z * _Time.y), cos(v.vertex.x * _Time.y), 0);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy + offset * 0.1;
                o.viewDir = ObjSpaceViewDir(v.vertex);
                
                float3 worldNormal = normalize(mul(float4(v.normal, 0), unity_WorldToObject).xyz);
                o.fresnel = pow(1.0 - saturate(dot(normalize(o.viewDir), worldNormal)), _FresnelPower) * _FresnelIntensity;
                
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float cameraDistance = length(i.viewDir);
                
                float2 animUV = i.uv + _Time.y * _AnimationSpeed;
                fixed4 col = tex2D(_MainTex, animUV);

                float4 override = _ColorOverride;
                override.a = col.a;
                col = lerp(col, override, _ColorOverride.a);

                col.a = col.a * 0.5;

                float adjustedEmissionStrength = clamp(_EmissionMin + cameraDistance * 0.1 - 0.4, _EmissionMin, _EmissionMax);
                fixed4 outcolor = col * i.fresnel;
                return outcolor * adjustedEmissionStrength;
            }
            ENDCG
        }
    }
}
