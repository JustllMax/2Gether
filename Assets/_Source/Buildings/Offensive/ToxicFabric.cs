using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicFabric : Building
{
    [SerializeField] Transform firePoint;
    float attackTimer;

    [SerializeField] ParticleSystem toxicCloudParticles;

    public override void Awake()
    {
        attackTimer = GetStatistics().AttackDelay;
        base.Awake();
    }

    private void Update()
    {
        if (attackTimer < GetStatistics().AttackDelay)
        {
            attackTimer += Time.deltaTime;
        }
        else
        {
            attackTimer = 0f;
            OnAttack();
        }
    }

    #region ChildrenMethods



    public override void OnAttack()
    {
        var hits = Physics.OverlapSphere(firePoint.position, GetStatistics().AttackRange, targetLayerMask);
        HashSet<AIController> controllers = new HashSet<AIController>();
        foreach (var hit in hits)
        {
            AIController controller = hit.GetComponentInParent<AIController>();
            if (controller != null)
            { 
                controllers.Add(controller);
            }
        }

        ShowVFX(controllers);
        ApplyPoison(controllers);
    }

    public override bool TakeDamage(float damage)
    {
        AudioManager.Instance.PlaySFXAtSource(takeHitSound, audioSource);

        Health -= damage;
        if (Health <= 0)
        {
            AudioManager.Instance.PlaySFXAtSource(createDestroySound, audioSource);
            Kill();
            return true;
        }
        return false;
    }

    public override void Kill()
    {

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

        AudioManager.Instance.PlaySFX(createDestroySound);
        Kill();
    }

    #endregion ChildrenMethods


    void ShowVFX(HashSet<AIController> controllers)
    {

        if(toxicCloudParticles != null)
        {
            toxicCloudParticles.Play();
        }
    }

    void ApplyPoison(HashSet<AIController> controllers)
    {
        foreach (var c in controllers)
        {
            if (c.TryGetComponent(out PoisonDOTComponent poison))
            {
                poison.SetUpPoisonEffect(GetStatistics().damagePerTick, GetStatistics().tickDelay, GetStatistics().effectDuration);
            }
            else
            {
                c.gameObject.AddComponent<PoisonDOTComponent>().SetUpPoisonEffect(GetStatistics().damagePerTick, GetStatistics().tickDelay, GetStatistics().effectDuration);
            }
        }
    }
    public ToxicFabricBuildingStatistics GetStatistics()
    {
        return GetBaseStatistics() as ToxicFabricBuildingStatistics;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.position, GetStatistics().AttackRange);
    }
}
