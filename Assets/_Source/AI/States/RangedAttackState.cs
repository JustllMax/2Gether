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
    [Range(0f, 20f)]
    public float MinAttackRange;

    [Tooltip("Time between shots in a burst")]
    [SerializeField]
    private float ProjectileCooldown;

    [Tooltip("Time between bursts")]
    [SerializeField]
    private float BurstCooldown;

    [Tooltip("Projectiles in a single bursts")]
    [SerializeField]
    private uint BurstCount;

    [SerializeField]
    private ProjectileProperties Projectile;

    [SerializeField]
    private float OnBurstRelocateChance;

    [Tooltip("Use shooting stance before firing")]
    [SerializeField]
    private bool UseShootingStance;

    [Tooltip("How long to wait in shooting stance before firing")]
    [SerializeField]
    private float StanceWait;

    [SerializeField]
    private AudioClip ShotSound;


    public override void OnStart(AIController controller)
    {
        controller.PlayAnimation("WALK");
        controller.ammoCount = BurstCount;
        controller.isWalking = true;
        controller.burstReady = true;
    }

    public override void OnTick(AIController controller)
    {
        controller.ApplyDefaultMovement();
        if (controller.distanceToTarget >= 0.75 * AttackRange)
        {
            controller.isWalking = true;
            controller.RefreshTargetPos();
        } else if (controller.isWalking)
        {
            controller.GetNavMeshAgent().ResetPath();
            controller.isWalking = false;
        }

        controller.isInShootingStance = false;
        controller.GetAnimator().SetBool("is_shooting", false);

        if (!controller.CanAttack())
        {
            return;
        }

        if (UseShootingStance && !controller.ShouldChangePath())
        {
            return;
        }
        controller.GetAnimator().SetBool("is_shooting", true);

        if (UseShootingStance && (!controller.AnimationComplete("ATTACK_START") || !controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("ATTACK_START")))
        {
            return;
        }
        controller.isInShootingStance = true;

        if (controller.lastAttackTime >= ProjectileCooldown)
        {
            //First shot, wait instead of shooting
            if (controller.burstReady && StanceWait > 0)
            {
                controller.lastAttackTime = ProjectileCooldown - StanceWait;
                controller.burstReady = false;
                return;
            }


            IShooterPoint shooter;
            if (!controller.TryGetComponent<IShooterPoint>(out shooter))
                return;

            shooter.PositionShooter(controller.CurrentTarget.transform, Projectile.speed, out Vector3 direction, out Vector3 position);

            controller.lastAttackTime = 0;
            controller.ammoCount--;
            if (controller.ammoCount <= 0)
            {
                controller.ammoCount = BurstCount;
                controller.lastAttackTime -= BurstCooldown;
                controller.burstReady = true;

                if (Random.Range(0.0f, 1.0f) <= OnBurstRelocateChance)
                {
                    RelocateWanderTarget(controller);
                }
            }

            shooter.OnFire();
            controller.PlaySound(ShotSound);
            OnFire(direction, position);
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
        return controller.distanceToTarget >= MinAttackRange && controller.distanceToTarget <= AttackRange && controller.CanAttack();
    }

    public override void OnLateUpdate(AIController controller)
    {
        if (controller.isInShootingStance && controller.TryGetComponent<IShooterPoint>(out var shooter))
        {
            shooter.PositionShooter(controller.CurrentTarget.transform, Projectile.speed, out Vector3 direction, out Vector3 position);
        }
            
    }

    void OnFire(in Vector3 dir, in Vector3 pos)
    {
        Projectile bullet = ProjectileManager.Instance.SpawnProjectile(pos, dir, Projectile);
    }

    void RelocateWanderTarget(AIController controller)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Cos(angle) * controller.distanceToTarget;
        float z = Mathf.Sin(angle) * controller.distanceToTarget;

        controller.isWalking = false;
        Vector3 center = controller.transform.position;

        if (controller.SampleNavSurface(new Vector3(center.x + x, center.y, center.z + z), out var surfacePoint))
        {
            controller.GetNavMeshAgent().SetDestination(surfacePoint);
        }
    }

    public override void OnTargetChanged(AIController controller)
    {

    }
}
    