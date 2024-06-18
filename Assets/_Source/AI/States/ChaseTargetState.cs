using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChaseTargetState", menuName = ("2Gether/AI/States/ChaseTarget"))]
public class ChaseTargetState : AIState
{
    public override void OnStart(AIController controller)
    {
        controller.PlayAnimation("WALK");
        controller.ApplyTargetMovement();
    }

    public override void OnTick(AIController controller)
    {
        controller.RefreshTargetPos();    
    }

    public override void OnExit(AIController controller)
    {

    }

    public override bool CanExitState(AIController controller)
    {
        return true;
    }

    public override bool CanChangeToState(AIController controller)
    {
        return controller.HasTarget();
    }

    public override void OnLateUpdate(AIController controller)
    {
        if (controller.AllAnimationsComplete())
        {
            controller.PlayAnimation("WALK");
        }
    }

    public override void OnTargetChanged(AIController controller)
    {

    }
}
    