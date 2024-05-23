using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIStats_", menuName = "2Gether/AI/Data/Statistics")]
public class EnemyStatistics : ScriptableObject
{
    public float Health;
    public float AttackRange;
    public float AttackDamage;
    public float AttackFireRate;
    public float AttackReloadTime;
    public float AttackRadius;
    public float AttackAmount;
    public float MovementSpeed;
}
