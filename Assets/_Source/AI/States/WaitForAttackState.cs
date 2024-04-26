using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(fileName = "WaitForAttackState", menuName = ("2Gether/AI/States/WaitForAttack"))]
public class WaitForAttackState : AIState
{

    
    public override void OnStart(AIController controller)
    {

        if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
        {
            controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
        }

    }


    public override void OnUpdate(AIController controller)
    {

        if (ShouldSearchForTarget(controller))
        {
            SearchForTarget(controller);
        }

        controller.distanceToTarget = controller.GetNavMeshAgent().remainingDistance;

    }

    public override void OnExit(AIController controller)
    {

    }

    public override bool CanChangeToState(AIController controller)
    {
        //TODO change this, agent can be stuck when waiting for attack and target gets destroyed
        return !controller.CanAttack() && controller.GetEnemyStats().AttackRange <= controller.distanceToTarget;
    }


    private bool ShouldSearchForTarget(AIController controller)
    {
        if (controller.GetCurrentTarget() != null)
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
        int layerMask = ~(1 << LayerMask.NameToLayer("Terrain"));
        float radius = controller.GetEnemyStats().AttackRange;

        float minDistance = float.MaxValue;


        Collider[] hits = Physics.OverlapSphere(controller.GetCurrentPosition(), radius, layerMask);
        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<ITargetable>(out ITargetable t))
            {
                if (t.IsTargetable)
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
