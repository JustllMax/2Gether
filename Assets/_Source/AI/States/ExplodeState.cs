using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplodeState", menuName = ("2Gether/AI/States/ExplodeState"))]
public class Explode : AIState
{
    public override void OnStart(AIController controller)
    {

    }

    public override void OnUpdate(AIController controller)
    {
        if (controller.lastAttackTime >= controller.GetEnemyStats().ComboDelay)
        {
            controller.Kill();
        }
    }

    public override void OnExit(AIController controller)
    {

    }


    public override bool CanExitState(AIController controller)
    {
        return false;
    }

    public override bool CanChangeToState(AIController controller)
    {
        return controller.distanceToTarget <= controller.GetEnemyStats().AttackRange && controller.CanAttack();
    }


}
