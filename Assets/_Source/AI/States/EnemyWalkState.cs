using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;


[CreateAssetMenu(fileName = "WalkState_", menuName =("2Gether/AI/States/WalkState"))]
public class EnemyWalkState : EnemyState
{
    private float speed ;
    private Vector3 position;

    public override bool CanChangeToState(AIController controller)
    {
        return this;
    }


    public override void Enter(AIController controller)
    {
        speed = controller.GetEnemyStats().MovementSpeed;
        position = new(Random.Range(-10.0f, 10.0f), controller.transform.position.y, Random.Range(-10.0f, 10.0f));
        Debug.Log("WalkStateEntered");
    }

    public override void Exit(AIController controller)
    {
        
    }

    public override void OnFixedUpdate(AIController controller)
    {
        var step = speed * Time.deltaTime;
        controller.transform.position = Vector3.MoveTowards(controller.transform.position, position, step);

        if (Vector3.Distance(controller.transform.position, position) < 0.001f)
        {
            //AIController.ChangeState();
        }
    }

    public override void OnUpdate(AIController controller)
    {

    }
}
