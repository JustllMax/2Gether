using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttackState", menuName = ("2Gether/AI/States/RangedAttack"))]
public class RangedAttackState : AIState
{

    GameObject P_Bullet;
    float bulletShootForce;
    bool firstAttackFlag = true;

    public override void OnStart(AIController controller)
    {
        if(controller.lastAttackTime != 0f)
        {
            firstAttackFlag = false;
        }
    }


    public override void OnUpdate(AIController controller)
    {

        if (controller.remainingAttacks <= 0)
        {
            if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(AIAnimNames.RELOAD.ToString()))
            {
                controller.GetAnimator().CrossFade(AIAnimNames.RELOAD.ToString(), 0.1f);
                return;
            }
        }

        if (firstAttackFlag || controller.lastAttackTime >= controller.GetEnemyStats().AttackFireRate )
        {
            if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animName.ToString()))
            {
                controller.GetAnimator().CrossFade(animName.ToString(), 0.1f);
            }

            controller.StartCoroutine(PerformAttack(controller));
        }


    }

    public override void OnExit(AIController controller)
    {
       
    }

    public override bool CanExitState(AIController controller)
    {
        return AttackAnimationComplete(controller);
    }

    public override bool CanChangeToState(AIController controller)
    {
        return  controller.distanceToTarget <= controller.GetEnemyStats().AttackRange && controller.CanAttack();
    }

    private bool AttackAnimationComplete(AIController controller)
    {
        return !controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName(animName.ToString());
    }
    IEnumerator PerformAttack(AIController controller)
    {
        controller.lastAttackTime = 0f;
        yield return new WaitForSeconds(AnimDelay);

        Vector3 dir = (controller.GetCurrentPosition() - controller.GetCurrentTarget().transform.position).normalized;
        AIBullet bullet = AIBulletManager.Instance.Pool.Get();
        bullet.SetDirection(dir);
        bullet.SetDamage(controller.GetEnemyStats().AttackDamage);
        controller.RangedAttackPerformed();
    }
}
