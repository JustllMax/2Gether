using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChaseState_", menuName = ("2Gether/AI/States/ChaseState"))]
public class EnemyChaseState : AIState
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
