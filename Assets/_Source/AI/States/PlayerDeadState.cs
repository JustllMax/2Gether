using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerDeadState", menuName = ("2Gether/AI/States/PlayerDeadState"))]
public class PlayerDeadState : AIState
{
    public TargetProperties mainBaseTarget;

    public override void OnStart(AIController controller)
    {
        if (GameManager.Instance.GetMainBaseTransform() != null)
        {
            Transform mainBase = GameManager.Instance.GetMainBaseTransform();
            AITarget target = controller.GetClosestTarget(mainBaseTarget);
            controller.CurrentTarget = target;
            controller.canChangeTarget = false;
        
            controller.RefreshTargetPos();
        }
    }

    public override void OnTick(AIController controller)
    {

    }


    public override void OnExit(AIController controller)
    {

    }

    public override bool CanChangeToState(AIController controller)
    {
        
        return !GameManager.Instance.IsPlayerAlive() && !controller.HasTarget();
    }


    public override bool CanExitState(AIController controller)
    {
        return true;
    }

    public override void OnLateUpdate(AIController controller)
    {

    }

    public override void OnTargetChanged(AIController controller)
    {

    }
}
