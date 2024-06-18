using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooterPoint
{
    public void PositionShooter(Transform target, float projectileSpeed, out Vector3 direction, out Vector3 position);
    public void OnFire();
}
