using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyMovement
{
    public float MovementSpeed;
    public float TurnSpeed;
    public float Acceleration;

    public float ExtraRotationSpeed;

    public static EnemyMovement operator *(EnemyMovement a, float b)
    {
        a.MovementSpeed *= b;
        a.TurnSpeed *= b;
        a.Acceleration *= b;
        a.ExtraRotationSpeed *= b;
        return a;
    }
}
