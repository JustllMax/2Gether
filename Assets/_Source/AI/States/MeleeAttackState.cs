using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

[CreateAssetMenu(fileName = "MeleeAttackState", menuName = ("2Gether/AI/States/MeleeAttack"))]
public class MeleeAttackState : AIState
{
    [SerializeField] LayerMask mask;
    public override void OnStart(AIController controller)
    {

    }


    public override void OnUpdate(AIController controller)
    {

        Debug.Log(this + " AnimationComplete(controller) " + AnimationComplete(controller));

        if (AnimationComplete(controller) && controller.lastAttackTime >= controller.GetEnemyStats().ComboDelay)
        {

            if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
            {
                controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
            }
            controller.StartCoroutine(PerformAttack(controller));
        }

    }

    public override void OnExit(AIController controller)
    {
        controller.GetNavMeshAgent().ResetPath();
        Debug.Log(this + " exit");

    }


    public override bool CanExitState(AIController controller)
    {
        return AnimationComplete(controller);
    }

    public override bool CanChangeToState(AIController controller)
    {
        return controller.distanceToTarget <= controller.GetEnemyStats().AttackRange && controller.CanAttack();
    }

    public IEnumerator PerformAttack(AIController controller)
    {
        controller.lastAttackTime = 0f;

        for (uint index = 0; index < controller.comboLength; index++)
        {
            EnemyAttack attack = controller.GetEnemyStats().attackCombo[index];

            yield return new WaitForSeconds(attack.Delay);
            Debug.Log(this + " attack performed");
            Vector3 spawnPos = controller.transform.position;


            var hits = Physics.OverlapSphere(spawnPos, attack.DamagerRadius, mask);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out ITargetable targetable))
                {
                    if (hit.GetComponent<IDamagable>().TakeDamage(attack.Damage) == true)
                    {
                        controller.distanceToTarget = 100000f;
                        controller.SetCurrentTarget(new AITarget(null, null));
                        Debug.Log(this + "Target died");

                    }

                }
            }
        }
    }


}
