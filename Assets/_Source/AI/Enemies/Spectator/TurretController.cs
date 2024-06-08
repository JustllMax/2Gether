using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class TurretController : MonoBehaviour, IShooterPoint
{
    [SerializeField]
    private GameObject turret;

    [SerializeField]
    private GameObject turret2;

    [SerializeField]
    private GameObject turretSpinner;

    private float spinTime = 0.0f;
    public void OnFire()
    {
        spinTime = 0.2f;
    }

    public void PositionShooter(in Vector3 targetPosition, out Vector3 direction, out Vector3 position)
    {
        Vector3 dir = (turret.transform.position - targetPosition).normalized;
        Quaternion q = Quaternion.LookRotation(dir);

        turret.transform.rotation = Quaternion.Euler(0, q.eulerAngles.y, 0);
        turret2.transform.rotation = Quaternion.Euler(q.eulerAngles.x, q.eulerAngles.y, 0);

        direction = -dir;
        position = turret2.transform.TransformPoint(new Vector3(0.0f, 0.2f, -0.4f));
    }

    public void Update()
    {
        if (spinTime > 0.0f)
        {
            spinTime -= Time.deltaTime;
            turretSpinner.transform.Rotate(0, 0, 720f * Time.deltaTime);
        }
    }
}
