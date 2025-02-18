using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Laser_", menuName = "2Gether/Buildings/Offensive/Laser")]
public class LaserBuildingStatistics : BuildingOffensiveStatistics
{
    public float laserDamageAmplification;

    public override List<(string, string)> GetStatistics()
    {
        statistics = base.GetStatistics();

        addStat("Damage Amp", laserDamageAmplification.ToString());
        return collectStat();
    }


}

