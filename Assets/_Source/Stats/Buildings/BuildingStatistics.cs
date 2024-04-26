using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
abstract public class BuildingStatistics : CardStatistics
{
    public BuildingType BuildingType;
    public float ActivationTime;
    public float BaseHealthPoints;
    public int BaseSellCost;

}
