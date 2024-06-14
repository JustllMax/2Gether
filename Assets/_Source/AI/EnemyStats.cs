using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


[CreateAssetMenu(fileName = "AIStats_", menuName = "2Gether/AI/Data/Statistics")]
public class EnemyStatistics : ScriptableObject
{
    public float Health;
    public EnemyMovement Movement;

    public TargetType PrimaryTarget;

    [Tooltip("Range to detect a primary target")]
    [Range(5f, 200f)]
    public float SearchRange;

    public TargetType SecondaryTarget;

    [Tooltip("Range to switch to a secondary target")]
    [Range(1f, 100f)]
    public float SwitchRange;
}
