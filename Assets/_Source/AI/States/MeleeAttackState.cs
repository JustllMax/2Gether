using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackState_", menuName = ("2Gether/AI/States/AttackState"))]
public class EnemyAttackState : AIState
{
    public override bool CanChangeToState(AIController controller)
    {
        return true;
    }

    public override void OnStart(AIController controller)
    {
       
    }

    public override void OnExit(AIController controller)
    {
       
    }


    public override void OnUpdate(AIController controller)
    {
       
    }
}
