using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "MainBase_", menuName = "2Gether/Buildings/Offensive/MainBase")]
public class MainBaseStatistics : BuildingOffensiveStatistics
{
    [Range(0,1f)]
    [Tooltip("In percentage, for example: Slowed down by 25%")]
    public float slowModifier;
    [Range(0,10f)]
    [Tooltip("In seconds, for exampled: Slowed down for 3 seconds")]
    public float slowDuration;

}

