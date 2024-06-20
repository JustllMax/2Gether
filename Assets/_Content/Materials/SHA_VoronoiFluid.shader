Shader "Custom/SHADER_VoronoiFluid_PixelArt"
{
	Properties
	{
		_ScrollSpeed ("Scroll Speed", Vector) = (0.0, 0.05, 0.0, 0.0)
		_Color0 ("Base Color", Color) = (0.054, 0.188, 0.529, 0.8)
		_Color1 ("Color 1", Color) = (0.125, 0.321, 0.6, 0.0)
		_Size1 ("Color 1 Size", Float) = 8.0
		_Jiggle1 ("Color 1 Jiggle", Float) = 0.333
		_Ease1 ("Color 1 Ease", Float) = 1.5
		_Color2 ("Color 2", Color) = (0.321, 0.674, 0.85, 0.0)
		_Size2 ("Color 2 Size", Float) = 6.0
		_Jiggle2 ("Color 2 Jiggle", Float) = 0.1
		_Ease2 ("Color 2 Ease", Float) = 3.0
		_Color3 ("Color 3", Color) = (0.658, 0.847, 0.941, 0.0)
		_Size3 ("Color 3 Size", Float) = 4.0
		_Jiggle3 ("Color 3 Jiggle", Float) = 0.1
		_Ease3 ("Color 3 Ease", Float) = 6.0
		_PixelSize ("Pixel Size", Float) = 10.0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest Less
		Cull Off

		Pass
		{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#pragma alpha:blend

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
			};

			float4 _ScrollSpeed;
			float4 _Color0;
			float4 _Color1;
			float _Size1;
			float _Jiggle1;
			float _Ease1;
			float4 _Color2;
			float _Size2;
			float _Jiggle2;
			float _Ease2;
			float4 _Color3;
			float _Size3;
			float _Jiggle3;
			float _Ease3;
			float _PixelSize;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv + float2(_Time.z * _ScrollSpeed.x, _Time.z * _ScrollSpeed.y);
				return o;
			}

			float2 random2(float2 p)
			{
				return frac(sin(float2(dot(p, float2(117.12, 341.7)), dot(p, float2(269.5, 123.3)))) * 43458.5453);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// Pixelate UV coordinates
				i.uv = float2(floor(i.uv.x * _PixelSize) / _PixelSize, floor(i.uv.y * _PixelSize * 0.5) / _PixelSize);

				fixed4 outputColor = _Color0;

				fixed4 col = _Color1;
				float2 uv = i.uv;
				uv *= _Size1; // Scale the diagram
				float2 iuv = floor(uv);
				float2 fuv = frac(uv);
				float minDist = 1.0; // Shortest distance from the cell point
				for (int y = -1; y <= 1; y++)
				{
					for (int x = -1; x <= 1; x++)
					{
						// Find the position of the neighboring cell point on the grid
						float2 neighbour = float2(float(x), float(y));
						// Randomly determine the position of this point
						float2 pointv = random2(iuv + neighbour);
						// Move points in time
						pointv = 0.5 + 0.5 * sin(_Time.z * _Jiggle1 + 6.2236 * pointv);
						// Determine the vector between the pixel and the point
						float2 diff = neighbour + pointv - fuv;
						// Calculate the distance
						float dist = length(diff);
						// If the distance is less than the current minimum, save it
						minDist = min(minDist, dist);
					}
				}

				col.a += pow(minDist, _Ease1);
				outputColor.rgb = outputColor.rgb * (1.0 - col.a) + col.rgb * col.a;

				col = _Color2;
				uv = i.uv;
				uv *= _Size2; // Scale the diagram
				iuv = floor(uv);
				fuv = frac(uv);
				minDist = 1.0; // Shortest distance from the cell point
				for (int y = -1; y <= 1; y++)
				{
					for (int x = -1; x <= 1; x++)
					{
						// Find the position of the neighboring cell point on the grid
						float2 neighbour = float2(float(x), float(y));
						// Randomly determine the position of this point
						float2 pointv = random2(iuv + neighbour);
						// Move points in time
						pointv = 0.5 + 0.5 * sin(_Time.z * _Jiggle2 + 4.2236 * pointv);
						// Determine the vector between the pixel and the point
						float2 diff = neighbour + pointv - fuv;
						// Calculate the distance
						float dist = length(diff);
						// If the distance is less than the current minimum, save it
						minDist = min(minDist, dist);
					}
				}

				col.a += pow(minDist, _Ease2);
				outputColor.rgb = outputColor.rgb * (1.0 - col.a) + col.rgb * col.a;

				col = _Color3;
				uv = i.uv;
				uv *= _Size3; // Scale the diagram
				iuv = floor(uv);
				fuv = frac(uv);
				minDist = 1.0; // Shortest distance from the cell point
				for (int y = -1; y <= 1; y++)
				{
					for (int x = -1; x <= 1; x++)
					{
						// Find the position of the neighboring cell point on the grid
						float2 neighbour = float2(float(x), float(y));
						// Randomly determine the position of this point
						float2 pointv = random2(iuv + neighbour);
						// Move points in time
						pointv = 0.5 + 0.5 * sin(_Time.z * _Jiggle3 + 4.2236 * pointv);
						// Determine the vector between the pixel and the point
						float2 diff = neighbour + pointv - fuv;
						// Calculate the distance
						float dist = length(diff);
						// If the distance is less than the current minimum, save it
						minDist = min(minDist, dist);
					}
				}

				// Draw the min distance (distance field)
				col.a += pow(minDist, _Ease3);
				outputColor.rgb = outputColor.rgb * (1.0 - col.a) + col.rgb * col.a;

				return outputColor;
			}

			ENDCG
		}
	}
}
