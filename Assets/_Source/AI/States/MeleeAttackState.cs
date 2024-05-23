using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttackState", menuName = ("2Gether/AI/States/MeleeAttack"))]
public class MeleeAttackState : AIState
{



    bool firstAttackFlag = true;

    public override void OnStart(AIController controller)
    {
        Debug.Log(this + " Started");
        if (controller.lastAttackTime >= 0.1f)
        {
            firstAttackFlag = false;
        }
    }


    public override void OnUpdate(AIController controller)
    {
        if (firstAttackFlag || Time.time > controller.lastAttackTime + controller.GetEnemyStats().AttackFireRate)
        {
            if (controller.CanAttack() == false)
                return;

            if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
            {
                controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
            }

            
            PerformAttack(controller);
        }
    }

    public override void OnExit(AIController controller)
    {
        controller.GetNavMeshAgent().ResetPath();
        Debug.Log(this + " exit");

    }


    public override bool CanChangeToState(AIController controller)
    {
        return controller.distanceToTarget <= controller.GetEnemyStats().AttackRange && controller.CanAttack();
    }

    void PerformAttack(AIController controller)
    {

        Debug.Log(this + " attack performed");

        controller.lastAttackTime = Time.time;
        Vector3 dir = (controller.GetCurrentTarget().transform.position - controller.GetCurrentPosition()).normalized;
        Vector3 spawnPos = controller.transform.position + dir * controller.GetEnemyStats().AttackRange;
        Debug.Log(spawnPos);
        //Layermask that hits everything except the terrain
        int buildingMask = 1 << LayerMask.NameToLayer(TargetType.Player.ToString());
        int playerMask = 1 << LayerMask.NameToLayer(TargetType.Building.ToString());
        int layerMask = buildingMask | playerMask;
            

        var hits = Physics.OverlapSphere(spawnPos, controller.GetEnemyStats().AttackRadius, layerMask);
        foreach ( var hit in hits )
        {
            if(hit.TryGetComponent(out IDamagable damagable))
            {
                if(damagable.TakeDamage(controller.GetEnemyStats().AttackDamage) == true)
                {
                    controller.distanceToTarget = 100000f;
                    controller.SetCurrentTarget(new AITarget( null, null));
                }
            }
        }
    }

}
