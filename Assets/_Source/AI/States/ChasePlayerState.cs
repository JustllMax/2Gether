using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChasePlayerState", menuName = ("2Gether/AI/States/ChasePlayer"))]
public class ChasePlayerState : AIState
{

    public override void OnStart(AIController controller)
    {
        if(controller.GetCurrentTarget() == null)
        {
            controller.SetCurrentTarget(GameManager.Instance.GetPlayerController().transform);
        }



        if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
        {
            controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
        }

    }

    public override void OnUpdate(AIController controller)
    {
        controller.GetNavMeshAgent().SetDestination(controller.GetCurrentTarget().position);
        controller.distanceToTarget = controller.GetNavMeshAgent().remainingDistance;
    }
    public override void OnExit(AIController controller)
    {
        controller.GetNavMeshAgent().ResetPath();

    }

    public override bool CanChangeToState(AIController controller)
    {
        return !controller.CanAttack();
    }
}
    