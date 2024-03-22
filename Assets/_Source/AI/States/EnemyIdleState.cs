using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleState_", menuName = ("2Gether/AI/States/IdleState"))]
public class EnemyIdleState : EnemyState
{
  

    public override bool CanChangeToState(AIController controller)
    {
        return this;
    }

    

    public override void Enter(AIController controller)
    {
        Debug.Log("IdleStateEntered");
    }

    public override void Exit(AIController controller)
    {
       
    }

    public override void OnFixedUpdate(AIController controller)
    {
       
    }

    

    public override void OnUpdate(AIController controller)
    {
        if (Input.GetKey(KeyCode.E))
        {
            //AIController.ChangeState();
        }
        if (Input.GetKey(KeyCode.R))
        {
            //AIController.ChangeState();
        }
    }
}
