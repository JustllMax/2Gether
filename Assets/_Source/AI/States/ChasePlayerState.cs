using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChasePlayerState", menuName = ("2Gether/AI/States/ChasePlayer"))]
public class ChasePlayerState : AIState
{

    Transform playerTransform;
    AITarget playerTarget;
    public override void OnStart(AIController controller)
    {
        if(playerTransform == null)
        {
            playerTransform = GameManager.Instance.GetPlayerController().transform;
            playerTarget = new AITarget(playerTransform, playerTransform.GetComponent<ITargetable>());
        }

        if(controller.GetCurrentTarget().transform == null)
        {
            controller.SetCurrentTarget(playerTarget);
        }


        if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
        {
            controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
        }

        controller.GetNavMeshAgent().SetDestination(controller.GetCurrentTarget().transform.position);
    }

    public override void OnUpdate(AIController controller)
    {
        controller.GetNavMeshAgent().SetDestination(controller.GetCurrentTarget().transform.position);
        controller.distanceToTarget = controller.GetNavMeshAgent().remainingDistance;
    }
    public override void OnExit(AIController controller)
    {
        controller.GetNavMeshAgent().ResetPath();

    }

    public override bool CanChangeToState(AIController controller)
    {
        return GameManager.Instance.IsPlayerAlive() && !controller.CanAttack();
    }
}
    