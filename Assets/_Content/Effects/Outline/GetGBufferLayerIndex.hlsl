//UNITY_SHADER_NO_UPGRADE

#ifndef GET_GBUFFER_LAYER_INCLUDED
#define GET_GBUFFER_LAYER_INCLUDED

#if !defined(SHADERGRAPH_PREVIEW)
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareRenderingLayerTexture.hlsl"
#endif

void GetGBufferLayerIndex_float(float2 uv, out float layer)
{
#if !defined(SHADERGRAPH_PREVIEW)
    layer = SampleSceneRenderingLayer(uv);
#else
	layer = 1.0f;
#endif
}


#endif