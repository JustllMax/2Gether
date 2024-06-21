using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hospital_", menuName = "2Gether/Buildings/Utility/Hospital")]
public class BuildingHospitalStatistics : BuildingOffensiveStatistics
{
    public float healAmount;
    public float delayBetweenActivation;

    public override List<(string, string)> GetStatistics()
    {
        statistics = base.GetStatistics();

        addStat("Heal", healAmount.ToString());
        return collectStat();
    }
}
