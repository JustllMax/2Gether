using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

[CreateAssetMenu(fileName = "GrabAttackState", menuName = ("2Gether/AI/States/GrabAttack"))]
public class GrabAttackState : AIState
{
    [Tooltip("Range to perform grab attack")]
    [Range(1.5f, 25f)]
    public float AttackRange;

    public EnemyAttack Attack;
    public float DamageDelay;
    public float ReleaseDelay;
    public LayerMask AttackMask;

    [Tooltip("Time in seconds between attack attempts")]
    [Range(0.25f, 10f)]
    public float AttackDelay;

    public override void OnStart(AIController controller)
    {
        if (controller.AnimationComplete("ATTACK") && controller.lastAttackTime >= AttackDelay)
        {
            controller.PlayAnimation("ATTACK");
            AudioManager.Instance.PlaySFXAtSource(controller.attackSound, controller.audioSource);

            controller.StartCoroutine(PerformAttack(controller));
        }
    }

    public override void OnUpdate(AIController controller)
    {

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


        yield return new WaitForSeconds(Attack.Delay);

        if (!controller.CanAttack())
        {
            yield break;
        }


        Debug.Log(this + " attack performed");
        Vector3 spawnPos = controller.transform.TransformPoint(Attack.DamagerOffset);

        var hits = Physics.OverlapSphere(spawnPos, Attack.DamagerRadius, AttackMask);
        bool wasHit = false;
        PlayerController player = null;
        GrabController grabController = null;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out player))
            {
                wasHit = true;
                player.IsTargetable = false;

                if (controller.TryGetComponent(out grabController))
                {
                    grabController.Player = player.gameObject;
                }
            }
        }

        AttackVisualization.DrawAttack(spawnPos, Attack.DamagerRadius, wasHit);

        if (wasHit)
        {
            yield return new WaitForSeconds(DamageDelay);

            //Player died
            if (player.TakeDamage(Attack.Damage))
            {
                if (grabController != null)
                    grabController.Player = null;
                controller.distanceToTarget = 100000f;
                controller.SetCurrentTarget(new AITarget(null, null));

            } else {
                //Player survived
                yield return new WaitForSeconds(ReleaseDelay);

                if (grabController != null)
                    grabController.Player = null;

                player.IsTargetable = true;
            }
        }

    }


    public override void OnLateUpdate(AIController controller)
    {

    }
}
