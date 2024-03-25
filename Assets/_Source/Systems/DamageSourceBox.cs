using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSourceBox : DamageSource
{
    [SerializeField]
    private Vector3 _size = Vector3.one;

    protected override Collider[] GetNearColliders()
    {
        return Physics.OverlapBox(transform.TransformPoint(_center), Vector3.Scale(transform.localScale, _size / 2), transform.rotation);
    }

    protected override void DrawGizmos()
    {
        Gizmos.DrawWireCube(transform.TransformPoint(_center), Vector3.Scale(transform.localScale, _size));
    }
}