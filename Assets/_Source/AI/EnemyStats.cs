using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


[CreateAssetMenu(fileName = "AIStats_", menuName = "2Gether/AI/Data/Statistics")]
public class EnemyStatistics : ScriptableObject
{
    public float Health;
    public EnemyMovement Movement;

    [Tooltip("Range to detect a target")]
    [Range(5f, 200f)]
    public float SearchRange;
}
