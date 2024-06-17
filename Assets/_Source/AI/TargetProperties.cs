using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetProperties
{
    public TargetType targetType;
    public WeightType weight;

    [Range(0f, 500f)]
    public float maxSearchRange;

    [Range(0f, 500f)]
    public float loseTargetRange;

    [Tooltip("Movement properties while pursuing the target")]
    public EnemyMovement movement;

    [Tooltip("If the enemy should walk on paths while pursuing the target")]
    public bool walkOnPath;

    [Tooltip("If the enemy should abandown the target if there is a better target available")]
    public bool canAbandonTarget;
}
