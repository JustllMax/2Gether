using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;


[CreateAssetMenu(fileName = "ReturnToPathState", menuName = ("2Gether/AI/States/ReturnToPath"))]
public class ReturnToPathState : AIState
{
    public float maxDistanceFromPath = 10.0f;

    public override void OnStart(AIController controller)
    {
        controller.ApplyTargetMovement();
        controller.canChangeTarget = false;

        controller.PlayAnimation("WALK");
        controller.WalksOnPath = false;
        if (SampleNavSurface(controller, controller.transform.position, out var edgePoint, maxDistanceFromPath * 10.0f))
        {
            //Apply margin to avoid getting stuck
            Vector3 position = edgePoint + (edgePoint - controller.transform.position).normalized;
            controller.GetNavMeshAgent().SetDestination(position);
        }
    }

    public override void OnTick(AIController controller)
    {

    }


    public override void OnExit(AIController controller)
    {
        controller.canChangeTarget = true;

        if (controller.CurrentTarget.properties.walkOnPath)
            controller.WalksOnPath = true;
    }

    public override bool CanExitState(AIController controller)
    {
        return controller.GetNavMeshAgent().remainingDistance < 0.05f;
    }

    public override bool CanChangeToState(AIController controller)
    {
        NavMeshAgent agent = controller.GetNavMeshAgent();
        
        //Is walking on path
        if (controller.WalksOnPath && agent.isOnNavMesh)
        {
            return false;
        }
        
        //Samle closest point on path
        if (SampleNavSurface(controller, controller.transform.position, out var position, maxDistanceFromPath))
        {
            if (controller.WalksOnPath && !agent.isOnNavMesh)
            {
                return true;
            }
        
            float distance = Vector3.Distance(controller.transform.position, position);

            //If 1. Target if further then max distance; 2. Enemy is not on path, has no target and should walk on path; 3. Enemy is not on path, and target should be chased on path
            if (distance > maxDistanceFromPath || distance > 0.5f && ((controller.CurrentTarget == null && controller.GetEnemyStats().walksOnPath) || controller.CurrentTarget.properties.walkOnPath))
            {
                return true;
            }
        
            return false;
        }
        
        //Far away from path
        return true;
    }

    public override void OnLateUpdate(AIController controller)
    {
        if (controller.AllAnimationsComplete())
        {
            controller.PlayAnimation("WALK");
        }
    }

    bool SampleNavSurface(AIController controller, in Vector3 point, out Vector3 pointOnSurface, float maxDistance)
    {
        int pathAgentType = AIManager.Instance.GetAgentIDFromType(controller.GetEnemyStats().agentType, true);
        if (NavMesh.SamplePosition(point, out NavMeshHit navhit, maxDistance, new NavMeshQueryFilter { agentTypeID = pathAgentType, areaMask = NavMesh.AllAreas }))
        {
            pointOnSurface = navhit.position;
            return true;
        }

        pointOnSurface = Vector3.zero;
        return false;
    }

    public override void OnTargetChanged(AIController controller)
    {

    }
}
