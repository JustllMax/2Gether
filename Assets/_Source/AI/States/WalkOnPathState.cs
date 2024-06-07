using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;


[CreateAssetMenu(fileName = "WalkState", menuName = ("2Gether/AI/States/Walk"))]
public class WalkOnPathState : AIState
{
    Transform dest;
    public override void OnStart(AIController controller)
    {
        //DO NOT SET AITARGET AS MAIN BASE
        if (GameManager.Instance.GetMainBaseTransform() != null)
        {
            dest = GameManager.Instance.GetMainBaseTransform();
            controller.GetNavMeshAgent().SetDestination(dest.position);
        }

        Debug.Log("WalkStateOnStarted");

        if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
        {
            controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
        }
    }

    public override void OnUpdate(AIController controller)
    {
        //Check is the current AITarget main base, if yes then nothing, if no then set destination as found target
        if (controller.GetCurrentTarget().transform != null)
        {
            if(controller.GetCurrentTarget().transform != dest)
            {
                dest = controller.GetCurrentTarget().transform;
                controller.GetNavMeshAgent().SetDestination(dest.position);
            }

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
        
        return true;
    }

   
    
}
