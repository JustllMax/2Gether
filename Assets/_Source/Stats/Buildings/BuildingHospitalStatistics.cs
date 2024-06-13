using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hospital_", menuName = "2Gether/Buildings/Utility/Hospital")]
public class BuildingHospitalStatistics : BuildingOffensiveStatistics
{
    public float healAmount;
    public float delayBetweenActivation;
}
