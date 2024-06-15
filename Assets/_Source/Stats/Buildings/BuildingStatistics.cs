using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
abstract public class BuildingStatistics : CardStatistics
{
    public BuildingType BuildingType;
    public float ActivationTime;
    public float HealthPoints;
    public int SellCost;

    public override List<(string, string)> GetStatistics()
    {
        addStat("Sell Cost", SellCost.ToString());
        addStat("HP", HealthPoints.ToString());
        return collectStat();
    }
}
