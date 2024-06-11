using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "SearchForPlayerState", menuName = ("2Gether/AI/States/SearchForPlayer"))]
public class SearchForPlayerState : AIState
{
    public float wanderRadius;

    public override void OnStart(AIController controller)
    {
        controller.SetMovementStats(controller.GetEnemyStats().Movement * 0.5f);
        controller.PlayAnimation("WALK");
    }

    public override void OnUpdate(AIController controller)
    { 
        if (controller.AllAnimationsComplete())
        {
            controller.PlayAnimation("WALK");
        }

        controller.GetNavMeshAgent().SetDestination(controller.wanderTarget);

        if (controller.GetNavMeshAgent().remainingDistance <= 1f)
        {
            RelocateWanderTarget(controller);
        }
    }

    public override void OnExit(AIController controller)
    {

    }

    public override bool CanExitState(AIController controller)
    {
        return controller.HasTarget();
    }

    public override bool CanChangeToState(AIController controller)
    {
        return !controller.HasTarget();
    }

    public override void OnLateUpdate(AIController controller)
    {

    }

    void RelocateWanderTarget(AIController controller)
    {
        PlayerController player = GameManager.Instance.GetPlayerController();
        Vector3 center = controller.transform.position;
        if (player)
        {
            Vector3 playerDir = (player.transform.position - controller.transform.position).normalized;
            center += playerDir * wanderRadius * 1.5f;
        }
        

        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Cos(angle) * wanderRadius;
        float z = Mathf.Sin(angle) * wanderRadius;
        controller.wanderTarget = new Vector3(center.x + x, center.y, center.z + z);
    }
}
    