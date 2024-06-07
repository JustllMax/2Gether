using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building
{
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] GameObject model;
    BuildingOffensiveStatistics statistics;


    public override void Start()
    {
        statistics = GetBaseStatistics() as BuildingOffensiveStatistics;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AIController controller))
        {
            OnAttack();
        }
    }

    #region ChildrenMethods

    public override void OnCreate()
    {

    }

    public override void OnAttack()
    {
        if (explosionParticles != null)
            explosionParticles.Play();
        audioSource.PlayOneShot(activationSound);

        ExplosionDamage();
        Kill();
    }


    public override bool TakeDamage(float damage)
    {
        audioSource.PlayOneShot(takeHitSound);
        Health -= damage;
        if (Health <= 0)
        {
            Kill();
            return true;
        }
        return false;
    }

    public override void Kill()
    {


        model.SetActive(false);
        IsTargetable = false;

       
        Invoke("DestroyObj", DestroyObjectDelay);
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }

    public override void OnSell()
    {
        base.OnSell();

        if (createDestroyParticles != null)
            createDestroyParticles.Play();

        audioSource.PlayOneShot(createDestroySound);
        Kill();
    }

    #endregion ChildrenMethods

    private void ExplosionDamage()
    {


        var hits = Physics.OverlapSphere(transform.position, GetStatistics().AttackRange, targetLayerMask);
        foreach (var hit in hits)
        { 
             if (hit.TryGetComponent(out AIController controller))
             {
                controller.TakeDamage(GetStatistics().AttackDamage);
             }
        }
    }
    BuildingOffensiveStatistics GetStatistics()
    {
        statistics = GetBaseStatistics() as BuildingOffensiveStatistics;
        return statistics;
    }
}
