using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttackState", menuName = ("2Gether/AI/States/RangedAttack"))]
public class RangedAttackState : AIState
{
  

    public override void OnStart(AIController controller)
    {
       
    }


    public override void OnUpdate(AIController controller)
    {
       
    }

    public override void OnExit(AIController controller)
    {

    }


    public override bool CanChangeToState(AIController controller)
    {
        return  controller.distanceToTarget <= controller.GetEnemyStats().AttackRange && controller.CanAttack();
    }
}
