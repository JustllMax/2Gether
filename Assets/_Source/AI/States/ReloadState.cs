using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReloadState", menuName = ("2Gether/AI/States/Reload"))]
public class ReloadState : AIState
{
    float reloadTimer = 0;
    public override void OnStart(AIController controller)
    {
        //controller.PlayAnimation("RELOAD");
        //controller.isReloading = true;
    }

    public override void OnTick(AIController controller)
    {
        //reloadTimer += Time.deltaTime;
        //if(reloadTimer >= controller.GetEnemyStats().attackCombo[0].Delay)
        //{
        //    controller.isReloading = false;
        //}
    }
    public override void OnExit(AIController controller)
    {
        //reloadTimer = 0;
        //controller.comboLength = controller.GetEnemyStats().attackCombo.Length;
    }

    public override bool CanChangeToState(AIController controller)
    {
        //return controller.distanceToTarget <= controller.GetEnemyStats().AttackRange && controller.comboLength <= 0;
        return true;
    }

    public override bool CanExitState(AIController controller)
    {
        //return controller.AnimationComplete("RELOAD");
        return false;
    }

    public override void OnLateUpdate(AIController controller)
    {
        
    }

    public override void OnTargetChanged(AIController controller)
    {

    }
}
