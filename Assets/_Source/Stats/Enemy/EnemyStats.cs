using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
[CreateAssetMenu(fileName = "AIStats_", menuName = "2Gether/AI/Data/Statistics")]
public class EnemyStatistics : ScriptableObject
{
    public float Health;
    public float AttackRange;
    public float AttackDamage;
    [Tooltip("Time in seconds between attacks")]
    [Range(0.25f, 1.5f)]
    public float AttackFireRate;
    public float AttackReloadTime;
    public float AttackRadius;
    public float AttackAmount;
    public float MovementSpeed;
}
