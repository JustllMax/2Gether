Shader "2Gether/FootPrint/Height Map Update"
{
    Properties
    {
        _DrawBrush ("Brush", 2D) = "white" {}
    }

    SubShader
    {
        Lighting Off
        Blend One Zero
        
        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0
            
            sampler2D _DrawBrush;
            float4 _DrawPosition;
            float _DrawAngle;
            float _RestoreAmount;
            
            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float borderSize = 0.1;

                float4 previousColor = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
                
                float2 pos = IN.localTexcoord.xy - _DrawPosition;
                
                float2x2 rot = float2x2(cos(_DrawAngle), -sin(_DrawAngle),
                                        sin(_DrawAngle), cos(_DrawAngle));
                pos = mul(rot, pos);
                pos /= 0.1;
                pos += float2(0.5, 0.5);
                
                float4 drawColor = tex2D(_DrawBrush, pos);
                
                if (IN.localTexcoord.x < borderSize || IN.localTexcoord.x > 1.0 - borderSize ||
                    IN.localTexcoord.y < borderSize || IN.localTexcoord.y > 1.0 - borderSize)
                {
                    return float4(0, 0, 0, 1); 
                }
                return min(previousColor, drawColor);
            }
            ENDCG
        }
    }
}
