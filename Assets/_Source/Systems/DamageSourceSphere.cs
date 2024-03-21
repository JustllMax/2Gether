using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSourceSphere : DamageSource
{
    [SerializeField]
    private float _radius = 1f;

    protected override Collider[] GetNearColliders()
    {
        return Physics.OverlapSphere(transform.TransformPoint(_center), GetRadius());
    }

    protected override void DrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.TransformPoint(_center), GetRadius());
    }

    private float GetRadius()
    {
        Vector3 scale = transform.localScale;
        return Mathf.Max(scale.x, scale.y, scale.z) * _radius / 2f;
    }
}