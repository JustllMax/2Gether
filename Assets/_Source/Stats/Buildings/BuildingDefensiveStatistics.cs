using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Building_", menuName = "2Gether/Buildings/Defensive")]
public class BuildingDefensiveStatistics : BuildingStatistics
{

    [HideInInspector]public bool HasHealthRegen;
    [HideInInspector]public float HealthRegen;

    [HideInInspector] public bool HasThorns;
    [HideInInspector]public float ThornsDamage;

}


#if UNITY_EDITOR
[CustomEditor(typeof(BuildingDefensiveStatistics))]
public class BuildingDefensiveStatistics_Editor : Editor
{
    private bool hpRegenCheck;
    private bool thornsCheck;
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        BuildingDefensiveStatistics script = (BuildingDefensiveStatistics)target;
        hpRegenCheck = EditorGUILayout.Toggle("HasHealthRegen", hpRegenCheck);
        script.HasHealthRegen = hpRegenCheck;

        if (hpRegenCheck)
        {
            EditorGUILayout.LabelField("HP Regen Settings", GUILayout.Width(150));
            script.HealthRegen = EditorGUILayout.FloatField("HP/s ", script.HealthRegen);
        }

        thornsCheck = EditorGUILayout.Toggle("HasThorns", thornsCheck);
        script.HasThorns = thornsCheck; 

        if (thornsCheck)
        {
            EditorGUILayout.LabelField("Thorns settings", GUILayout.Width(150));

            script.ThornsDamage = EditorGUILayout.FloatField("Damage", script.ThornsDamage);

        }
    }
}
#endif