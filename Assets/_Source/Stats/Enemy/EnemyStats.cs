using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


[CreateAssetMenu(fileName = "AIStats_", menuName = "2Gether/AI/Data/Statistics")]
public class EnemyStatistics : ScriptableObject
{
    public float Health;
    public float MovementSpeed;

    [Tooltip("Range to enter attack state")]
    [Range(1.5f, 25f)]
    public float AttackRange;

    [Tooltip("Time in seconds between combos")]
    [Range(0.25f, 10f)]
    public float ComboDelay;

    [Tooltip("Attack combo")]
    public EnemyAttack[] attackCombo;
}
