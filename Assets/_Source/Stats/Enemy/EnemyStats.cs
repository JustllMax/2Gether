using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public struct EnemyAttack
{
    public float Damage;
    public float Delay;

    [Foldout("Melee")]
    [Tooltip("Radius of sphere that detects targets")]
    public float DamagerRadius;
}


[CreateAssetMenu(fileName = "AIStats_", menuName = "2Gether/AI/Data/Statistics")]
public class EnemyStatistics : ScriptableObject
{
    public float Health;

    [Tooltip("Range to enter attack state")]
    [Range(1.5f, 25f)]
    public float AttackRange;

    public float MovementSpeed;

    [Tooltip("Attack combo")]
    public EnemyAttack[] attackCombo;

    [Tooltip("Time in seconds between combos")]
    [Range(0.25f, 10f)]
    public float ComboDelay;

}
