using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
using static UnityEngine.Rendering.DebugUI.Table;

public class BlasterController : MonoBehaviour, IShooterPoint
{
    [SerializeField]
    private GameObject Arm1;

    [SerializeField]
    private GameObject Arm2;

    [SerializeField]
    private GameObject Hand;
    public void OnFire()
    {

    }


    public void PositionShooter(Transform target, float projectileSpeed, out Vector3 direction, out Vector3 position)
    {
        Vector3 targetPosition = target.position;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        Vector3 dir = (targetPosition - transform.position).normalized;
        Vector3 lookAngles = Quaternion.LookRotation(dir).eulerAngles;

        Vector3 rootAngles = transform.rotation.eulerAngles;
        float delta = rootAngles.y - lookAngles.y;

        if (delta > 0 || delta < -30)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, lookAngles.y, 0), 2f * Time.deltaTime);
        }

        //Arm
        Vector3 arm1GlobalPos = transform.TransformPoint(Arm1.transform.localPosition);

        Vector3 arm1dir = (arm1GlobalPos - targetPosition).normalized;
        Quaternion arm1lookRot = Quaternion.Inverse(transform.rotation) * Quaternion.LookRotation(arm1dir);
        float arm1Angle = arm1lookRot.eulerAngles.y - 180f;
        float arm1Clamp = Mathf.Clamp(arm1lookRot.eulerAngles.y - 180f, -15, 80);

        Arm1.transform.localRotation = Quaternion.Euler(0, arm1Clamp, -60);
        Arm2.transform.localRotation = Quaternion.Euler(70, 0, 0);

        Vector3 handDir;
        if (arm1Angle >= -15 && arm1Angle <= 80)
        {
            //Hand
            handDir = (Hand.transform.position - (targetPosition + new Vector3(0.0f, 0.4f, 0.0f))).normalized;
            Quaternion handLookRot = Quaternion.Inverse(Arm2.transform.rotation) * Quaternion.LookRotation(handDir);
            Vector3 handAngles = handLookRot.eulerAngles + new Vector3(80.0f, 0.0f, -20.0f);

            Hand.transform.localRotation = Quaternion.Euler(handAngles);
        } else
        {
            handDir = Hand.transform.up;
        }



        direction = -handDir;
        position = Hand.transform.TransformPoint(new Vector3(0.0f, 0.0f, 0.0f));
    }

}
