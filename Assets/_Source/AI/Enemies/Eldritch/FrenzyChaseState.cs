using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "FenzyChaseState", menuName = ("2Gether/AI/States/FenzyChase"))]
public class FenzyChaseState : AIState
{
    public float relocateDamage = 40;
    public float minRelocateDistance = 30;
    public float maxRelocateDistance = 60;
    public AudioClip ChaseStartSound;

    public override void OnStart(AIController controller)
    {
        controller.PlayAnimation("WALK");
        (controller as EldritchController).receivedDamage = 0;
        controller.PlaySound(ChaseStartSound);
    }

    public override void OnTick(AIController controller)
    {
        controller.ApplyTargetMovement();
        controller.RefreshTargetPos();    

        EldritchController eldritchController = controller as EldritchController;
        if (eldritchController.receivedDamage >= relocateDamage)
        {
            eldritchController.Relocate(minRelocateDistance, maxRelocateDistance);
        }
    }

    public override void OnExit(AIController controller)
    {
        
    }

    public override bool CanExitState(AIController controller)
    {
        return true;
    }

    public override bool CanChangeToState(AIController controller)
    {
        return controller.HasTarget();
    }

    public override void OnLateUpdate(AIController controller)
    {
        if (controller.AllAnimationsComplete())
        {
            controller.PlayAnimation("WALK");
        }
    }

    public override void OnTargetChanged(AIController controller)
    {

    }
}
    