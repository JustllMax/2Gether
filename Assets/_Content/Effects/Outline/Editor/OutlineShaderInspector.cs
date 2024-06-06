using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Unity.Rendering.Universal;
using UnityEngine.Rendering.Universal;
using System;

public class OutlineShaderInspector : ShaderGUI
{
    
    


    override public void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        // render the shader properties using the default GUI
        base.OnGUI(materialEditor, properties);

        Material targetMat = materialEditor.target as Material;


        var pLayerMask = FindProperty("_AffectedLayer", properties);
        InternalURPBridgeEditorUtils.DrawRenderingLayerMask(pLayerMask, EditorGUIUtility.TrTextContent("Rendering Layers", "Specify the rendering layer mask for outline."));
    }
}
