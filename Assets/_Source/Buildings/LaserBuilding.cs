using System.Collections.Generic;
using UnityEngine;

public class LaserBuilding : Building
{
    
    public GameObject bulletPrefab;
    public Transform firePoint;

    private AIController target;


    public float fireContDown = 0f;
    public float fireRate = 3f;

    //for laser
    [Header("For Laser")]
    [SerializeField] LineRenderer lineRenderer;
    bool useLaser;
    float laserCooldown = 1f;
    float lastLaserDamageTime;
    float laserDamage = 2f;
    float laserOriginalDamage;
    float laserDamageDiff = 2f;
    private float range;
    BuildingOffensiveStatistics statistics;

    BuildingOffensiveStatistics GetStatistics()
    {
        statistics = GetBaseStatistics() as BuildingOffensiveStatistics;
        return statistics;
    }

    public override void Awake()
    {
        base.Awake();
        laserOriginalDamage = GetStatistics().AttackDamage;
        laserDamage = laserOriginalDamage;
        range = GetStatistics().AttackRange;

    }

    public override void Start()
    {
        statistics = GetBaseStatistics() as BuildingOffensiveStatistics;
        base.Start();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    public void Update()
    {
        LockOnTarget();
        if (target == null && useLaser && lineRenderer.enabled == true) { 
            lineRenderer.enabled = false;
            laserDamage = laserOriginalDamage;
        }

        if (target == null)
                return;
        if (useLaser)
        {
            Laser();

        }
        else
        {
            if (fireContDown <= 0f)
            {
                OnAttack();
                fireContDown = 1f / fireRate;
            }
            fireContDown -= Time.deltaTime;
        }

    }
    #region ChildrenMethods

    public override void OnCreate()
    {
        
    }

    public override void OnAttack()
    {
        if (target != null)
        {
            GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.setTarget(target.transform);
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

    private void LockOnTarget()
    {
        if (target != null)
        {
            Vector3 direction = target.GetCurrentPosition() - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

    }

    private void Laser()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.GetCurrentPosition());
        if (target != null && target.TryGetComponent(out AIController controller))
        {
            if (Time.time - lastLaserDamageTime >= laserCooldown)
            {
                controller.TakeDamage(laserDamage);
                lastLaserDamageTime = Time.time;
                laserDamage += laserDamageDiff;
            }
        }
    }

    void UpdateTarget()
    {
        List<AIController> enemies;
        enemies = new List<AIController>();
        var hits = Physics.OverlapSphere(transform.position, GetStatistics().AttackRange, targetLayerMask);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out AIController controller))
            {
                if (controller.IsDead() == false)
                {
                    Vector3 directionToTarget = (controller.GetCurrentPosition() - transform.position).normalized; 
                    float distanceToTarget = Vector3.Distance(transform.position, controller.GetCurrentPosition());

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                        enemies.Add(controller);
                }

            }
        }
        float minDistance = float.MaxValue;
        AIController tempTarget = null;
        if(enemies != null)
        {
            if(enemies.Count != 0)
            {
                foreach(var e in enemies)
                {
                    float distance = Vector3.Distance(e.GetCurrentPosition(), transform.position);
                    if (distance < minDistance)
                        tempTarget = e;
                }
                target = tempTarget;
            }
        }
        target = null;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

   

}

