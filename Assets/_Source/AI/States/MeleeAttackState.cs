using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttackState", menuName = ("2Gether/AI/States/MeleeAttack"))]
public class MeleeAttackState : AIState
{

    public override void OnStart(AIController controller)
    {
       
    }

    public override void OnUpdate(AIController controller)
    {

    }
    public override void OnExit(AIController controller)
    {

    }

    public override bool CanChangeToState(AIController controller)
    {
        return true;
    }

}
