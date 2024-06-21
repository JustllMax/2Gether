using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "SmiteRod_", menuName = "2Gether/Buildings/Offensive/SmiteRod")]
public class SmiteRodBuildingStatistics : BuildingOffensiveStatistics
{
    [Range(0,1f)]
    [Tooltip("In percentage, for example: Slowed down by 25%")]
    public float slowModifier;
    [Range(0,10f)]
    [Tooltip("In seconds, for exampled: Slowed down for 3 seconds")]
    public float slowDuration;

    public override List<(string, string)> GetStatistics()
    {
        statistics = base.GetStatistics();
        int strPercent = (int)slowModifier * 10;
        int durPercent = (int)slowDuration * 10;

        addStat("Slow Str", strPercent.ToString());
        addStat("Slow Dur", durPercent.ToString());
        return collectStat();
    }
}

