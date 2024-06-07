using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyAttack
{
    public float Damage;
    public float Delay;

    [Tooltip("Radius of sphere that detects targets")]
    public float DamagerRadius;
}
