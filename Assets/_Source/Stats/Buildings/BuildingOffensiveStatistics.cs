using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Building_", menuName = "2Gether/Buildings/Offensive/Standard")]
public class BuildingOffensiveStatistics : BuildingStatistics
{
    public float AttackDamage;
    public float AttackRange;
    public float AttackFireRate;

    /*
    [HideInInspector]public bool IsAOE;
    [HideInInspector]public GameObject P_AOE;
    [HideInInspector]public float AOERange;
    [HideInInspector]public float AOEDamage;

    [HideInInspector]public bool IsKnockback;
    [HideInInspector]public float AttackKnockback;
    */
}

/*
#if UNITY_EDITOR
[CustomEditor(typeof(BuildingOffensiveStatistics))]
public class BuildingOffensiveStatistics_Editor : Editor
{
    private bool knockbackCheck;
    private bool AOECheck;
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector(); 
        BuildingOffensiveStatistics script = (BuildingOffensiveStatistics)target;
        knockbackCheck = EditorGUILayout.Toggle("IsKnockback", knockbackCheck);
        script.IsKnockback = knockbackCheck;

        if (knockbackCheck) 
        {
            EditorGUILayout.LabelField("Knockback Settings", GUILayout.Width(150) );
            script.AttackKnockback = EditorGUILayout.FloatField("Knockback force", script.AttackKnockback);
        }

        AOECheck = EditorGUILayout.Toggle("IsAOE", AOECheck);
        script.IsAOE = AOECheck;

        if (AOECheck)
        {
            EditorGUILayout.LabelField("AOE settings", GUILayout.Width(150));

            //script.P_AOE = EditorGUILayout.ObjectField("AOE Prefab", script.P_AOE, typeof(GameObject)) as GameObject;
            script.AOEDamage = EditorGUILayout.FloatField("AOE Damage", script.AOEDamage);
            script.AOERange = EditorGUILayout.FloatField("AOE Range", script.AOERange);

        }
    }
}
#endif
*/
