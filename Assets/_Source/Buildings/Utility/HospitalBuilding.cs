using UnityEngine;

public class HospitalBuilding : Building
{
  
    public ParticleSystem healingParticles;


    float activationTimer = 0f;
    float healingTimer = 0f;

    PlayerController player;
    bool isPlayerInRange = false;
    bool healOnce;
    public override void Start()
    {
        base.Start();
        GetStatistics();
    }

    public void Update()
    {
        if (isPlayerInRange)
        {
            if (activationTimer < GetStatistics().ActivationTime)
            {
                activationTimer += Time.time;
            }
            else
            {
                //Heal after activating, then repeat after delay
                if (healOnce)
                {
                    healOnce = false;
                    OnAttack();
                }
                healingTimer += Time.time;
                if (healingTimer > GetStatistics().delayBetweenActivation)
                {
                    healingTimer = 0f;
                    OnAttack();
                }
            }
        }
        else
        {
            activationTimer = 0f;
            healOnce = true;
        }


    }

    #region ChildrenMethods


    public override void OnAttack()
    {
        if (healingParticles != null)
            healingParticles.Play();

        var hits = Physics.OverlapSphere(transform.position, GetStatistics().AreaRange, targetLayerMask);

        AudioManager.Instance.PlaySFXAtSource(activationSound, audioSource);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IDamagable damagable))
            {
                damagable.Heal(GetStatistics().healAmount);
            } 
        }
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
        if(createDestroyParticles != null)
            createDestroyParticles.Play();
        Invoke("DestroyObj", DestroyObjectDelay);
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }

    public override void OnSell()
    {
        base.OnSell();
        AudioManager.Instance.PlaySFX(createDestroySound);

        Kill();
    }

    #endregion ChildrenMethods

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GetStatistics().AreaRange);

    }

    BuildingHospitalStatistics GetStatistics()
    {
        return GetBaseStatistics() as BuildingHospitalStatistics;
        
    }
}