using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : Building
{
    LineRenderer lineRenderer;
    [SerializeField] Transform firePoint;
    float attackTimer;
    public Transform playerSpawnPoint;
    public override void Awake()
    {
        base.Awake();

        attackTimer = GetStatistics().AttackDelay;
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void Start()
    {
        base.Start();
        HUDManager.Instance.SetMainBasesMaxHealth(GetBaseStatistics().HealthPoints);
        HUDManager.Instance.SetMainBaseCurrentHealth(Health);
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
                Vector3 directionToTarget = (controller.GetCurrentPosition() - firePoint.position).normalized;
                float distanceToTarget = Vector3.Distance(firePoint.position, controller.GetCurrentPosition());

                if (!Physics.Raycast(firePoint.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    controllers.Add(controller);
                }
            }
        }

        ShowVFX(controllers);
        ApplySlowAndDamage(controllers);
    }

    public override bool TakeDamage(float damage)
    {
        AudioManager.Instance.PlaySFXAtSource(takeHitSound, audioSource);

        Health -= damage;
        HUDManager.Instance.SetMainBaseCurrentHealth(Health);
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
        LostScreenManager.Instance.EnableLostScreen();
        
    }


    public override void OnSell()
    {
        return;
    }

    #endregion ChildrenMethods


    void ShowVFX(HashSet<AIController> controllers)
    {

        lineRenderer.positionCount = controllers.Count * 2;
        int index = 0;
        foreach (var controller in controllers)
        {
            lineRenderer.SetPosition(index, firePoint.position);
            index++;
            lineRenderer.SetPosition(index, controller.transform.position);
            index++;
        }

    }

    void ApplySlowAndDamage(HashSet<AIController> controllers)
    {
        foreach (var c in controllers)
        {
            c.TakeDamage(GetStatistics().AttackDamage);
            if (c.TryGetComponent(out SlowDownComponent slow))
            {
                slow.SetUpSlowEffect(GetStatistics().slowModifier, GetStatistics().slowDuration);
            }
            else
            {
                c.gameObject.AddComponent<SlowDownComponent>().SetUpSlowEffect(GetStatistics().slowModifier, GetStatistics().slowDuration);
            }
        }
    }
    public MainBaseStatistics GetStatistics()
    {
        return GetBaseStatistics() as MainBaseStatistics;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.position, GetStatistics().AttackRange);
    }
}
