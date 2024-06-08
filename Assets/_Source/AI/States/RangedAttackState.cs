using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

[CreateAssetMenu(fileName = "RangedAttackState", menuName = ("2Gether/AI/States/RangedAttack"))]
public class RangedAttackState : AIState
{
    [Tooltip("Range to enter attack state")]
    [Range(5f, 200f)]
    public float AttackRange;

    [Tooltip("Range to exit attack state")]
    [Range(1.5f, 20f)]
    public float MinAttackRange;

    [SerializeField]
    private float ProjectileSpeed;

    [SerializeField]
    private float ProjectileDamage;

    [SerializeField]
    private float ProjectileCooldown;

    [SerializeField]
    private float BurstCooldown;

    [SerializeField]
    private uint BurstCount;

    [SerializeField]
    private GameObject ProjectilePrefab;

    [SerializeField]
    private float OnBurstRelocateChance;


    public override void OnStart(AIController controller)
    {
        controller.PlayAnimation("WALK");
        controller.ammoCount = BurstCount;
    }

    public override void OnUpdate(AIController controller)
    {
        controller.ApplyDefaultMovement();
        if (controller.distanceToTarget >= 0.75 * AttackRange)
        {
            controller.RefreshTargetPos();
            controller.wanderTarget = controller.transform.position;
        } else
        {
            controller.GetNavMeshAgent().SetDestination(controller.wanderTarget);
        }
    }

    public override void OnExit(AIController controller)
    {
        controller.lastAttackTime = 0;
    }

    public override bool CanExitState(AIController controller)
    {
        return true;
    }

    public override bool CanChangeToState(AIController controller)
    {
        return controller.distanceToTarget > MinAttackRange && controller.distanceToTarget <= AttackRange && controller.CanAttack();
    }

    public override void OnLateUpdate(AIController controller)
    {
        if (!controller.CanAttack())
        {
            return;
        }

        IShooterPoint shooter = controller.GetComponent<IShooterPoint>();
        shooter.PositionShooter(controller.GetCurrentTarget().transform.position, out Vector3 direction, out Vector3 position);

        if (controller.lastAttackTime >= ProjectileCooldown)
        {
            controller.lastAttackTime = 0;
            controller.ammoCount--;
            if (controller.ammoCount <= 0)
            {
                controller.ammoCount = BurstCount;
                controller.lastAttackTime -= BurstCooldown;

                if (Random.Range(0.0f, 1.0f) <= OnBurstRelocateChance)
                {
                    RelocateWanderTarget(controller);
                }
            }

            shooter.OnFire();
            OnFire(direction, position);
        }
    }
    void OnFire(in Vector3 dir, in Vector3 pos)
    {
        AIBullet bullet = AIBulletManager.Instance.Pool.Get();
        bullet.SetDirection(dir);
        bullet.SetDamage(ProjectileDamage);
        bullet.transform.position = pos;
        bullet.SetSpeed(ProjectileSpeed);
    }

    void RelocateWanderTarget(AIController controller)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Cos(angle) * controller.distanceToTarget;
        float z = Mathf.Sin(angle) * controller.distanceToTarget;

        Vector3 center = controller.transform.position;
        controller.wanderTarget = new Vector3(center.x + x, center.y, center.z + z);
    }
}
    