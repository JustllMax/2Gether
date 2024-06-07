using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplodeState", menuName = ("2Gether/AI/States/ExplodeState"))]
public class Explode : AIState
{

    [SerializeField] float AnimDelayForAttack;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] LayerMask mask;
    public override void OnStart(AIController controller)
    {

    }


    public override void OnUpdate(AIController controller)
    {

        Debug.Log(this + " AnimationComplete(controller) " + AnimationComplete(controller));

        if (AnimationComplete(controller) && controller.lastAttackTime >= controller.GetEnemyStats().AttackFireRate)
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
        controller.GetNavMeshAgent().ResetPath();
        Debug.Log(this + " exit");

    }


    public override bool CanExitState(AIController controller)
    {
        return AnimationComplete(controller);
    }

    public override bool CanChangeToState(AIController controller)
    {
        return controller.distanceToTarget <= controller.GetEnemyStats().AttackRange && controller.CanAttack();
    }

    public IEnumerator PerformAttack(AIController controller)
    {
        controller.lastAttackTime = 0f;

        yield return new WaitForSeconds(AnimDelayForAttack);
        Debug.Log(this + " attack performed");


        var hits = Physics.OverlapSphere(controller.GetCurrentPosition(), controller.GetEnemyStats().AttackRadius, mask);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out ITargetable targetable))
            {

                if (hit.GetComponent<IDamagable>().TakeDamage(controller.GetEnemyStats().AttackDamage))
                {
                    Debug.Log("Explosion hit target");
                }

            }
        }
        if (explosionParticles != null)
            controller.InstantiateGameObject(explosionParticles.gameObject, controller.transform);

        AudioManager.Instance.PlaySFXAtSource(controller.attackSound, controller.audioSource);
        controller.Kill();

    }


}
