using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEditorInternal;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerDeadState", menuName = ("2Gether/AI/States/PlayerDeadState"))]
public class PlayerDeadState : AIState
{

    public override void OnStart(AIController controller)
    {
        controller.GetNavMeshAgent().SetDestination(GameManager.Instance.GetMainBaseTransform().position);
        controller.SetCurrentTarget(GameManager.Instance.GetMainBaseTransform());

        

        if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
        {
            controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
        }
    }

    public override void OnUpdate(AIController controller)
    {

    }


    public override void OnExit(AIController controller)
    {
        controller.GetNavMeshAgent().ResetPath();
    }

    public override bool CanChangeToState(AIController controller)
    {
        
        return !GameManager.Instance.IsPlayerAlive() && !controller.CanAttack();
    }

    private bool ShouldSearchForTarget(AIController controller)
    {
        if(controller.GetCurrentTarget() != null)
        {
            if (controller.GetCurrentTarget().GetComponent<ITargetable>().IsTargetable == true)
            {
                return false;
            }
        }
        return true;
    }
    private void SearchForTarget(AIController controller)
    {
        Transform target = null;

        //Layermask that hits everything except the terrain
        int layerMask = ~(1<<LayerMask.NameToLayer("Terrain"));
        float radius = controller.GetEnemyStats().AttackRange;

        float minDistance = float.MaxValue;
        

        Collider[] hits =  Physics.OverlapSphere(controller.GetCurrentPosition(), radius, layerMask);
        foreach (Collider hit in hits)
        {
            if(hit.TryGetComponent<ITargetable>(out ITargetable t))
            {
                if(t.IsTargetable)
                {
                    float distanceToTarget = Vector3.Distance(hit.transform.position, controller.transform.position);
                    if (distanceToTarget < minDistance)
                    {
                        target = hit.transform;
                        minDistance = distanceToTarget;
                    }
                }
            }
        }


        controller.SetCurrentTarget(target);
    }
}
