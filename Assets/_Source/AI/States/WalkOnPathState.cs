using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEditorInternal;
using UnityEngine;


[CreateAssetMenu(fileName = "WalkState", menuName = ("2Gether/AI/States/Walk"))]
public class WalkOnPathState : AIState
{
    Transform target;
    public override void OnStart(AIController controller)
    {
        //DO NOT SET TARGET AS MAIN BASE
        target = GameManager.Instance.GetMainBaseTransform();
        controller.GetNavMeshAgent().SetDestination(GameManager.Instance.GetMainBaseTransform().position);
        Debug.Log("WalkStateOnStarted");

        if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
        {
            controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
        }
    }

    public override void OnUpdate(AIController controller)
    {
        if (controller.GetCurrentTarget().transform != null)
        {
            if(controller.GetCurrentTarget().transform != target)
            {
                target = controller.GetCurrentTarget().transform;
                controller.GetNavMeshAgent().SetDestination(target.position);

            }

        }
        controller.distanceToTarget = controller.GetNavMeshAgent().remainingDistance;

    }


    public override void OnExit(AIController controller)
    {
        controller.GetNavMeshAgent().ResetPath();
    }

    public override bool CanChangeToState(AIController controller)
    {
        
        return true;
    }

   
    
}
