using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttackState", menuName = ("2Gether/AI/States/MeleeAttack"))]
public class MeleeAttackState : AIState
{
    [Tooltip("Range to enter attack state")]
    [Range(1.5f, 25f)]
    public float AttackRange;

    [Tooltip("Time in seconds between combos")]
    [Range(0.25f, 10f)]
    public float ComboDelay;

    public LayerMask AttackMask;

    [Tooltip("Attack combo")]
    public EnemyAttack[] AttackCombo;

    [Tooltip("Movement modifier to apply when attacking")]
    public EnemyMovementModifier[] MovementModifier;

    public override void OnStart(AIController controller)
    {

    }


    public override void OnTick(AIController controller)
    {
        controller.RefreshTargetPos();
        Debug.Log(this + " AnimationComplete(controller) " + controller.AnimationComplete("ATTACK"));

        if (controller.AnimationComplete("ATTACK") && controller.lastAttackTime >= ComboDelay)
        {
            controller.PlayAnimation("ATTACK");
            controller.PlaySound(controller.attackSound);
            controller.StartCoroutine(PerformAttack(controller));
            controller.StartCoroutine(ApplyMovementModifier(controller));
        }

    }

    public override void OnExit(AIController controller)
    {

    }


    public override bool CanExitState(AIController controller)
    {
        return controller.AnimationComplete("ATTACK");
    }

    public override bool CanChangeToState(AIController controller)
    {
        return controller.distanceToTarget <= AttackRange && controller.CanAttack();
    }

    public IEnumerator PerformAttack(AIController controller)
    {
        controller.lastAttackTime = 0f;

        for (uint index = 0; index < AttackCombo.Length; index++)
        {
            EnemyAttack attack = AttackCombo[index];

            yield return new WaitForSeconds(attack.Delay);

            if (!controller.CanAttack())
            {
                break;
            }


            Debug.Log(this + " attack performed");
            Vector3 spawnPos = controller.transform.TransformPoint(attack.DamagerOffset);

            var hits = Physics.OverlapSphere(spawnPos, attack.DamagerRadius, AttackMask);
            bool wasHit = false;
            foreach (var hit in hits)
            {
                IDamagable damagable = null;
                if (hit.attachedRigidbody != null)
                    hit.attachedRigidbody.TryGetComponent<IDamagable>(out damagable);
                else
                    hit.TryGetComponent<IDamagable>(out damagable);

                if (damagable != null)
                {
                    wasHit = true;
                    if (damagable.TakeDamage(attack.Damage) == true)
                    {
                        controller.distanceToTarget = 100000f;
                        controller.CurrentTarget = null;
                        Debug.Log(this + "Target died");

                    }

                }
            }

            AttackVisualization.DrawAttack(spawnPos, attack.DamagerRadius, wasHit);
        }
    }

    public IEnumerator ApplyMovementModifier(AIController controller)
    {
        for (uint index = 0; index < MovementModifier.Length; index++)
        {
            EnemyMovementModifier movement = MovementModifier[index];
            yield return new WaitForSeconds(movement.Delay);


            if (!controller.CanAttack())
            {
                break;
            }

            controller.SetMovementStats(movement.Movement);

            if (movement.ResetMomentum)
            {
                controller.GetNavMeshAgent().ResetPath();
                controller.GetNavMeshAgent().velocity = Vector3.zero;
            } else
            {
                controller.RefreshTargetPos();
            }
        }
    }

    public override void OnLateUpdate(AIController controller)
    {

    }

    public override void OnTargetChanged(AIController controller)
    {

    }
}
