using UnityEngine;

public class HospitalBuilding : Building
{
  
    public ParticleSystem healingParticles;


    float activationTimer = 0f;
    float healingTimer = 0f;
    bool healOnce;
    public override void Start()
    {
        base.Start();
        GetStatistics();
    }

    public void Update()
    {
        PlayerController player = GameManager.Instance.GetPlayerController();
        float distance = Vector3.Distance(transform.position, player.transform.position);
        bool hasMaxHP = player.Health >= player._maxHealth;

        if (distance < GetStatistics().AttackRange && !hasMaxHP)
        {
            if (activationTimer < GetStatistics().ActivationTime)
            {
                activationTimer += Time.deltaTime;
            }
            else
            {
                //Heal after activating, then repeat after delay
                if (healOnce)
                {
                    healOnce = false;
                    OnAttack();
                }
                healingTimer += Time.deltaTime;
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

        var hits = Physics.OverlapSphere(transform.position, GetStatistics().AttackRange, targetLayerMask);

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


    #endregion ChildrenMethods

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GetStatistics().AttackRange);

    }

    BuildingHospitalStatistics GetStatistics()
    {
        return GetBaseStatistics() as BuildingHospitalStatistics;
        
    }
}