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
        controller.GetNavMeshAgent().Stop();
        timer = 0f;
        if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
        {
            controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
        }

    }


    public override void OnUpdate(AIController controller)
    {
        timer += Time.deltaTime;
        if(timer > stunTime)
        {
            controller.SetStun(false);
        }
    }

    public override void OnExit(AIController controller)
    {
        controller.GetNavMeshAgent().Resume();

    }
    public override bool CanExitState(AIController controller)
    {
        return true;
    }

    public override bool CanChangeToState(AIController controller)
    {
        return controller.IsStunned();
    }






}
