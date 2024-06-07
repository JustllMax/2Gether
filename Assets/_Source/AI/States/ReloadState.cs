using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReloadState", menuName = ("2Gether/AI/States/Reload"))]
public class ReloadState : AIState
{
    float reloadTimer = 0;
    public override void OnStart(AIController controller)
    {
        controller.PlayAnimation("RELOAD");
        controller.isReloading = true;
    }

    public override void OnUpdate(AIController controller)
    {
        reloadTimer += Time.deltaTime;
        if(reloadTimer >= controller.GetEnemyStats().attackCombo[0].Delay)
        {
            controller.isReloading = false;
        }
    }
    public override void OnExit(AIController controller)
    {
        reloadTimer = 0;
        controller.comboLength = controller.GetEnemyStats().attackCombo.Length;
    }

    public override bool CanChangeToState(AIController controller)
    {
        return controller.distanceToTarget <= controller.GetEnemyStats().AttackRange && controller.comboLength <= 0;
    }

    public override bool CanExitState(AIController controller)
    {
        return controller.AnimationComplete("RELOAD");
    }

}
