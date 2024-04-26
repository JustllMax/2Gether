using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChasePlayerState", menuName = ("2Gether/AI/States/ChasePlayer"))]
public class ChasePlayerState : AIState
{
    Transform target;
    NavMeshAgent agent;

    public override void OnStart(AIController controller)
    {
        if(target == null)
        {
            target = GameManager.Instance.GetPlayerController().transform;
        }
        if(agent == null)
        {
            agent = controller.GetNavMeshAgent();
        }

        
    }

    public override void OnUpdate(AIController controller)
    {
        agent.SetDestination(target.position);
        if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
        {
            controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
        }
    }
    public override void OnExit(AIController controller)
    {
        agent.ResetPath();

    }

    public override bool CanChangeToState(AIController controller)
    {
        return !controller.IsStunned() && !controller.CanAttack() && GameManager.Instance.IsPlayerAlive();
    }
}
    