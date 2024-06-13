using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooterPoint
{
    public void PositionShooter(in Vector3 targetPosition, out Vector3 direction, out Vector3 position);
    public void OnFire();
}
