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
            Debug.Log(this + " searching for player");
            playerTransform = GameManager.Instance.GetPlayerController().transform;
            playerTarget = new AITarget(playerTransform, GameManager.Instance.GetPlayerController().GetComponent<ITargetable>());
            controller.SetCurrentTarget(playerTarget);

        }

        if (controller.GetCurrentTarget().transform == null)
        {
            Debug.Log(this + " " + GameManager.Instance.GetPlayerController().GetComponent<ITargetable>().TargetType);

            controller.SetCurrentTarget(playerTarget);
        }


        if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
        {
            controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
        }

        Debug.Log(this + " set destination to player");

        controller.GetNavMeshAgent().SetDestination(controller.GetCurrentTarget().transform.position);
    }

    public override void OnUpdate(AIController controller)
    {
        controller.GetNavMeshAgent().SetDestination(controller.GetCurrentTarget().transform.position);
        if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
        {
            controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
        }
    }
    public override void OnExit(AIController controller)
    {
        controller.GetNavMeshAgent().ResetPath();

    }

    public override bool CanExitState(AIController controller)
    {
        return true;
    }

    public override bool CanChangeToState(AIController controller)
    {
        return GameManager.Instance.IsPlayerAlive();
    }
}
    