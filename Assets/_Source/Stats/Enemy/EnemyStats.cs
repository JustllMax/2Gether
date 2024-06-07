using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
[CreateAssetMenu(fileName = "AIStats_", menuName = "2Gether/AI/Data/Statistics")]
public class EnemyStatistics : ScriptableObject
{
    public float Health;
    [Range(1.5f, 25f)]
    public float AttackRange;
    public float AttackDamage;
    [Tooltip("Time in seconds between attacks")]
    [Range(0.25f, 1.5f)]
    public float AttackFireRate;
    [Tooltip("Radius of sphere that detects targets")] 
    public float AttackRadius;
    public float MovementSpeed;
    [Foldout("ForRanged")] public float AttackReloadTime;
    [Foldout("ForRanged")] public float AttackAmount;

}
