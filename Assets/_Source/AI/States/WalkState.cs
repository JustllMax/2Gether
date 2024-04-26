using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;


[CreateAssetMenu(fileName = "WalkState", menuName =("2Gether/AI/States/WalkState"))]
public class WalkState : AIState
{

    float speed;


    public override void OnStart(AIController controller)
    {
        speed = controller.GetEnemyStats().MovementSpeed;

        Debug.Log("WalkStateOnStarted");
    }

    public override void OnUpdate(AIController controller)
    {

    }

    public override void OnExit(AIController controller)
    {
        
    }

    public override bool CanChangeToState(AIController controller)
    {
        //if(AttackRange > distance to target)
        return true;
    }

}
