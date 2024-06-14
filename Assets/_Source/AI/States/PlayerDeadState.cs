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
        
        Debug.Log(this + " Started");
        if (GameManager.Instance.GetMainBaseTransform() != null)
        {
            mainBase = GameManager.Instance.GetMainBaseTransform();
            mainBaseTarget = new AITarget(mainBase, mainBase.GetComponent<ITargetable>());
            controller.SetCurrentTarget(mainBaseTarget);
            controller.canSwitchTarget = false;
            Debug.Log(this + " Target set to " + mainBaseTarget.transform);

            controller.GetNavMeshAgent().SetDestination(mainBase.position);

        }


        controller.PlayAnimation("WALK");
    }

    public override void OnUpdate(AIController controller)
    {

    }


    public override void OnExit(AIController controller)
    {
        Debug.Log(this + " Exited");
        controller.GetNavMeshAgent().ResetPath();
    }

    public override bool CanChangeToState(AIController controller)
    {
        
        return !GameManager.Instance.IsPlayerAlive() && !controller.CanAttack();
    }


    public override bool CanExitState(AIController controller)
    {
        return true;
    }

    public override void OnLateUpdate(AIController controller)
    {

    }
}
