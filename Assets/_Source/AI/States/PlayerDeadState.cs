using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerDeadState", menuName = ("2Gether/AI/States/PlayerDeadState"))]
public class PlayerDeadState : AIState
{
    Transform mainBase;
    AITarget mainBaseTarget;
    public override void OnStart(AIController controller)
    {

        if(mainBase == null)
        {
            mainBase = GameManager.Instance.GetMainBaseTransform();
            mainBaseTarget = new AITarget(mainBase, mainBase.GetComponent<ITargetable>());
            controller.SetCurrentTarget(mainBaseTarget);
        }  

        if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
        {
            controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
        }
        controller.GetNavMeshAgent().SetDestination(mainBaseTarget.transform.position);

    }

    public override void OnUpdate(AIController controller)
    {
        controller.distanceToTarget = controller.GetNavMeshAgent().remainingDistance;
    }


    public override void OnExit(AIController controller)
    {
        controller.GetNavMeshAgent().ResetPath();
    }

    public override bool CanChangeToState(AIController controller)
    {
        
        return !GameManager.Instance.IsPlayerAlive() && !controller.CanAttack();
    }


    public override bool CanExitState(AIController controller)
    {
        return AnimationComplete(controller);
    }



}
