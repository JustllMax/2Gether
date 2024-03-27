using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BuildingUpgradePair
{
    public int Level;
    public BuildingStatistics Statistics;
}

[CreateAssetMenu(fileName ="UpgradePath_BUILDINGNAME", menuName= "2Gether/BuildingUpgradePath")]
public class BuildingUpgradePath : ScriptableObject
{
    public List<BuildingUpgradePair> UpgradePairs = new List<BuildingUpgradePair>();

    private void OnValidate()
    {
        if (UpgradePairs != null && UpgradePairs.Count != 3)
        {
            Debug.LogWarning(this.name + " has to have 3 upgrade pairs!");
        }
    }

    public BuildingStatistics GetStatsForLevel(int level) 
    { 
        foreach (var pair in UpgradePairs)
        {
            if (pair.Level == level)
                return pair.Statistics;
        }

        return null;
    }
}
