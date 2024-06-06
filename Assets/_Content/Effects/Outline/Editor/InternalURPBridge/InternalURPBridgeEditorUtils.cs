using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class InternalURPBridgeEditorUtils
{
    public static void DrawRenderingLayerMask(MaterialProperty p, GUIContent c)
    {
        Rect controlRect = EditorGUILayout.GetControlRect(true);
        int renderingLayer = p.intValue;

        string[] renderingLayerMaskNames = UniversalRenderPipelineGlobalSettings.instance.renderingLayerMaskNames;
        int maskCount = (int)Mathf.Log(renderingLayer, 2) + 1;
        if (renderingLayerMaskNames.Length < maskCount && maskCount <= 32)
        {
            var newRenderingLayerMaskNames = new string[maskCount];
            for (int i = 0; i < maskCount; ++i)
            {
                newRenderingLayerMaskNames[i] = i < renderingLayerMaskNames.Length ? renderingLayerMaskNames[i] : $"Unused Layer {i}";
            }
            renderingLayerMaskNames = newRenderingLayerMaskNames;

            EditorGUILayout.HelpBox($"One or more of the Rendering Layers is not defined in the Universal Global Settings asset.", MessageType.Warning);
        }

        EditorGUI.BeginChangeCheck();
        renderingLayer = EditorGUI.MaskField(controlRect, c, renderingLayer, renderingLayerMaskNames);

        if (EditorGUI.EndChangeCheck())
            p.floatValue = RenderingLayerUtils.ToValidRenderingLayers((uint)renderingLayer);

        EditorGUI.EndProperty();
    }
}
