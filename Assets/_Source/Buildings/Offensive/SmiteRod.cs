using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class SmiteRod : Building
{
    LineRenderer lineRenderer;
    [SerializeField] Transform firePoint;

    float attackTimer;

    #region ChildrenMethods

    public override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(attackTimer < GetStatistics().AttackDelay)
        {
            attackTimer += Time.deltaTime;
        }
        else
        {
            attackTimer = 0f;
            OnAttack();
        }
    }

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

    
    void ShowVFX(HashSet<AIController> controllers){

        lineRenderer.positionCount = controllers.Count*2;
        int index = 0;
        foreach(var controller in controllers)
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
    public SmiteRodBuildingStatistics GetStatistics()
    {
        return GetBaseStatistics() as SmiteRodBuildingStatistics;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.position, GetStatistics().AttackRange);
    }
}
