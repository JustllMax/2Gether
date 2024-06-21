using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "ToxicFabric_", menuName = "2Gether/Buildings/Offensive/ToxicFabric")]
public class ToxicFabricBuildingStatistics : BuildingOffensiveStatistics
{
    [Header("Use those for poison effect")]
    [Range(1,15f)]
    [Tooltip("Damage applied every time poison activates")]
    public float damagePerTick;
    [Range(0,5f)]
    [Tooltip("In seconds, for example: Apply damage once per 0.65 s")]
    public float tickDelay;
    [Range(0, 120f)]
    [Tooltip("In seconds, for example: Poisoned for 15 seconds")]
    public float effectDuration;

    public override List<(string, string)> GetStatistics()
    {
        statistics = base.GetStatistics();
        int strPercent = (int)damagePerTick;
        int durPercent = (int)effectDuration;

        addStat("Poison Dmg", strPercent.ToString());
        addStat("Poison Dur", durPercent.ToString());
        return collectStat();
    }

}

