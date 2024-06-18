using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleState_", menuName = ("2Gether/AI/States/IdleState"))]
public class StunState : AIState
{

    [SerializeField] float stunTime;
    float timer = 0f;
    
    public override void OnStart(AIController controller)
    {
        controller.GetNavMeshAgent().isStopped = true;
        timer = 0f;
        controller.PlayAnimation("STUN");
    }


    public override void OnTick(AIController controller)
    {
        timer += Time.deltaTime;
        if(timer > stunTime)
        {
            controller.SetStun(false);
        }
    }

    public override void OnExit(AIController controller)
    {
        controller.GetNavMeshAgent().isStopped = false;

    }
    public override bool CanExitState(AIController controller)
    {
        return true;
    }

    public override bool CanChangeToState(AIController controller)
    {
        return controller.IsStunned();
    }

    public override void OnLateUpdate(AIController controller)
    {

    }

    public override void OnTargetChanged(AIController controller)
    {

    }
}
