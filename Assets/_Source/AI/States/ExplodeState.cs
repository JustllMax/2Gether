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

        Debug.Log(this + " AnimationComplete(controller) " + AnimationComplete(controller));

        if (AnimationComplete(controller) && controller.lastAttackTime >= controller.GetEnemyStats().AttackFireRate)
        {
            controller.Kill();
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


}
