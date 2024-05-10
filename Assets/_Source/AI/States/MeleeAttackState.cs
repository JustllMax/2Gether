using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttackState", menuName = ("2Gether/AI/States/MeleeAttack"))]
public class MeleeAttackState : AIState
{


    float lastAttackTime = 0f;
    bool firstAttackFlag = true;

    public override void OnStart(AIController controller)
    {
        if (lastAttackTime != 0f)
        {
            firstAttackFlag = false;
        }
    }


    public override void OnUpdate(AIController controller)
    {
        if (firstAttackFlag || Time.time > lastAttackTime + controller.GetEnemyStats().AttackFireRate)
        {
            if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
            {
                controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
            }

            PerformAttack(controller);
        }
    }

    public override void OnExit(AIController controller)
    {

    }


    public override bool CanChangeToState(AIController controller)
    {
        return controller.distanceToTarget <= controller.GetEnemyStats().AttackRange && controller.CanAttack();
    }

    void PerformAttack(AIController controller)
    {
        lastAttackTime = Time.time;
        Vector3 dir = (controller.GetCurrentPosition() - controller.GetCurrentTarget().transform.position).normalized;
        Vector3 spawnPos = controller.transform.position + dir * controller.GetEnemyStats().AttackRange;
        //Layermask that hits everything except the terrain
        int buildingMask = 1 << LayerMask.NameToLayer(TargetType.Player.ToString());
        int playerMask = 1 << LayerMask.NameToLayer(TargetType.Building.ToString());
        int layerMask = buildingMask | playerMask;
            

        var hits = Physics.OverlapSphere(spawnPos, controller.GetEnemyStats().AttackRadius, layerMask);
        foreach ( var hit in hits )
        {
            if(hit.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(controller.GetEnemyStats().AttackDamage);
            }
        }
        controller.AttackPerformed();
    }

}
