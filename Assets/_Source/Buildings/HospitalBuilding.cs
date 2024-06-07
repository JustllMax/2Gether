using UnityEngine;

public class Healing : Building
{
  
    public ParticleSystem particles;

    BuildingHospitalStatistics statistics;

    float timer = 0f;
    public override void Start()
    {
        base.Start();
        GetStatistics();
    }

    public void Update()
    {

        if(timer < GetStatistics().delayBetweenActivation)
        {
            timer += Time.time;
        }
        else
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

        timer = 0f;
        if(particles != null)
            particles.Play();

        var hits = Physics.OverlapSphere(transform.position, GetStatistics().AreaRange, targetLayerMask);
        
        foreach(var hit in hits)
        {
            if(hit.TryGetComponent(out PlayerController controller))
            {
                controller.Heal(GetStatistics().healAmount);
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