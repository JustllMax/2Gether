Shader "2Gether/Forcefield"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask", 2D) = "white" {}
        _ForcefieldTex ("Forcefield Texture", 2D) = "white" {}
        _AnimationSpeed ("Animation Speed", Float) = 1.0
        _EffectAlpha ("Forcefield Alpha", Float) = 1.0
        _EmissionMin ("Min Emission", Float) = 1.0
        _EmissionMax ("Max Emission", Float) = 1.0
    }
    
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent" }
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
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
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 viewDir : TEXCOORD1;
            };
            
            sampler2D _MainTex;
            sampler2D _MaskTex;
            sampler2D _ForcefieldTex;
            float _AnimationSpeed;
            float _EffectAlpha;
            float _EmissionMin;
            float _EmissionMax;

            float4 _ForcefieldTex_TexelSize;
            float4 _MaskTex_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.viewDir = ObjSpaceViewDir(v.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float cameraDistance = length(i.viewDir);

                //Main texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= 0.5;

                float mask = tex2D(_MaskTex, i.uv).r;
                
                //Animation
                float2 uvScale = float2(_MaskTex_TexelSize.z / _ForcefieldTex_TexelSize.z, _MaskTex_TexelSize.w / _ForcefieldTex_TexelSize.w);
                float2 animUV = i.uv * uvScale.xy + _Time.y * _AnimationSpeed;
                fixed4 fcol = tex2D(_ForcefieldTex, animUV);
                fcol.a = fcol.a * 0.5 * mask * _EffectAlpha;

                float adjustedEmissionStrength = clamp(_EmissionMin + cameraDistance * 0.1 - 0.4, _EmissionMin, _EmissionMax);

                float4 outcolor = col + fcol;
                return outcolor * adjustedEmissionStrength;
            }
            ENDCG
        }
    }
}
