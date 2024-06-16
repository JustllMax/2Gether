using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct EnemyMovement
{
    public float movementSpeed;
    public float turnSpeed;
    public float acceleration;

    public float extraRotationSpeed;

    public static EnemyMovement operator *(EnemyMovement a, float b)
    {
        a.movementSpeed *= b;
        a.turnSpeed *= b;
        a.acceleration *= b;
        a.extraRotationSpeed *= b;
        return a;
    }
}