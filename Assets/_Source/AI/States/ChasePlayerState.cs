using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChasePlayerState", menuName = ("2Gether/AI/States/ChasePlayer"))]
public class ChasePlayerState : AIState
{
    public override void OnStart(AIController controller)
    {
        controller.PlayAnimation("WALK");
        controller.ApplyDefaultMovement();
    }

    public override void OnUpdate(AIController controller)
    {
        
        controller.RefreshTargetPos();    
        if (controller.AllAnimationsComplete())
        {
            controller.PlayAnimation("WALK");
        }
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

    }
}
    