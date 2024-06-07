using UnityEngine;

public class HospitalBuilding : Building
{
  
    public ParticleSystem particles;

    BuildingHospitalStatistics statistics;

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

    void SearchForPlayer()
    {

       
    }

    #region ChildrenMethods


    public override void OnCreate()
    {

    }

    public override void OnAttack()
    {
        if (particles != null)
            particles.Play();

        var hits = Physics.OverlapSphere(transform.position, GetStatistics().AreaRange, targetLayerMask);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(-GetStatistics().healAmount);
                return;
            }
        }
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
        IsTargetable = false;
        audioSource.PlayOneShot(createDestroySound);
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
        Kill();
    }

    #endregion ChildrenMethods
    public void OnTakeDamage()
    {
        //todo 
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GetStatistics().AreaRange);

    }

    BuildingHospitalStatistics GetStatistics()
    {
        return statistics = GetBaseStatistics() as BuildingHospitalStatistics;
        
    }
}