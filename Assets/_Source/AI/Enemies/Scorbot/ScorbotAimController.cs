using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
using static UnityEngine.Rendering.DebugUI.Table;

public class ScorboatAimController : MonoBehaviour, IShooterPoint
{
    [SerializeField]
    private GameObject Gun;
    public void OnFire()
    {

    }


    public void PositionShooter(Transform target, float projectileSpeed, out Vector3 direction, out Vector3 position)
    {
        Vector3 targetPosition = target.position;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        if (target.TryGetComponent(out PlayerController playerController))
        {
            direction = PredictiveAim(new Vector3(targetPosition.x, 1f, targetPosition.z), new Vector3(transform.position.x, 1.5f, transform.position.z), playerController.Velocity, projectileSpeed);
        }
        else
        {
            direction = (targetPosition - transform.position).normalized;
        }

        Vector3 lookAngles = Quaternion.LookRotation(direction).eulerAngles;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, lookAngles.y, 0), 8f * Time.deltaTime);


        position = transform.TransformPoint(new Vector3(0.0f, 1.5f, 0.0f));
    }

    public static int SolveQuadratic(float a, float b, float c, out float x1, out float x2)
    {
        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            x1 = float.NaN;
            x2 = float.NaN;
            return 0;
        }

        x1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
        x2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);
        return discriminant > 0 ? 2 : 1;
    }

    public static Vector3 PredictiveAim(Vector3 a, Vector3 b, Vector3 vA, float sB)
    {
        Vector3 aToB = a - b;
        float dC = aToB.magnitude;
        float alpha = Vector3.Angle(aToB, vA) * Mathf.Deg2Rad;
        float sA = vA.magnitude;
        float r = sA / sB;
        if (SolveQuadratic(1-r*r,2*r*dC*Mathf.Cos(alpha), -(dC*dC), out float root1, out float root2) == 0)
        {
            return aToB;
        }

        float dA = Mathf.Max(root1, root2);
        float t = dA / sB;
        Vector3 c = a + vA * t;

        return (c - b).normalized;
    }

}
