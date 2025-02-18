using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplodeState", menuName = ("2Gether/AI/States/ExplodeState"))]
public class Explode : AIState
{
    [Tooltip("Range to enter attack state")]
    [Range(1.5f, 25f)]
    public float AttackRange;

    public override void OnStart(AIController controller)
    {

    }

    public override void OnTick(AIController controller)
    {
        BomberbotController bc = controller as BomberbotController;
        if (bc == null)
        {
            controller.Kill();
        } else
        {
            bc.Kill();
        }
    }

    public override void OnExit(AIController controller)
    {

    }


    public override bool CanExitState(AIController controller)
    {
        return false;
    }

    public override bool CanChangeToState(AIController controller)
    {
        return controller.distanceToTarget <= AttackRange && controller.CanAttack();
    }

    public override void OnLateUpdate(AIController controller)
    {

    }

    public override void OnTargetChanged(AIController controller)
    {

    }
}
