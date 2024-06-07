using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurretController : MonoBehaviour
{
    private AIController controller;

    [SerializeField]
    private float attackRange = 5f;

    [SerializeField]
    private float attackCooldown = 0.25f;
    private float nextAttackTime = 0f;

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private GameObject turret;

    [SerializeField]
    private GameObject turret2;

    void Awake()
    {
        controller = GetComponent<AIController>();
    }

    void LateUpdate()
    {
        if (controller.GetCurrentTarget().transform == null || !controller.IsAlive)
        {
            return;
        }

        Vector3 dir = (turret.transform.position - controller.GetCurrentTarget().transform.position).normalized;
        Quaternion q = Quaternion.LookRotation(dir);

        turret.transform.rotation = Quaternion.Euler(0, q.eulerAngles.y, 0);
        turret2.transform.rotation = Quaternion.Euler(q.eulerAngles.x, q.eulerAngles.y, 0);

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            OnFire(dir, turret2.transform.TransformPoint(new Vector3(0.0f, 0.2f, -0.4f)));
        }
    }

    void OnFire(in Vector3 dir, in Vector3 pos)
    {
        AIBullet bullet = AIBulletManager.Instance.Pool.Get();
        bullet.SetDirection(dir);
        bullet.SetDamage(controller.GetEnemyStats().AttackDamage);
        bullet.transform.position = pos;
    }

}
