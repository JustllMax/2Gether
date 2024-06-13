using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyMovementModifier
{
    public EnemyMovement Movement;
    public float Delay;
    public bool ResetMomentum;
}
