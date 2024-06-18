using System.Collections.Generic;
using UnityEngine;

public class LaserBuilding : Building
{
    
    GameObject bulletPrefab;
    
    
    private AIController target;

    //co to jest
    float fireCountDown = 3f;
    float fireRate = 3f;
    
    //for laser
    [Header("For Laser")]
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    Transform Turret;
    LineRenderer lineRenderer;
    bool useLaser = true;
    float lastLaserDamageTime;
    float laserDamage;
    float laserOriginalDamage;
    float laserDamageDiff = 2f;

    float searchForTargetTimer;
    BuildingOffensiveStatistics GetStatistics()
    {
        return GetBaseStatistics() as BuildingOffensiveStatistics;
    }

    public override void Awake()
    {
        useLaser = true;
        base.Awake();
        laserOriginalDamage = GetStatistics().AttackDamage;
        laserDamage = laserOriginalDamage;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

    }

    public override void Start()
    {
        base.Start();
    }

    public void Update()
    {

        if(searchForTargetTimer < 0.5f){
            searchForTargetTimer+= Time.time;
        }
        else{
            searchForTargetTimer = 0f;
            UpdateTarget();
        }

        LockOnTarget();
        if (target == null && useLaser && lineRenderer.enabled == true) { 
            lineRenderer.enabled = false;
            laserOriginalDamage = GetStatistics().AttackDamage;
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
            if (fireCountDown <= 0f)
            {
                OnAttack();
                fireCountDown = 1f / fireRate;
            }
            fireCountDown -= Time.deltaTime;
        }

    }
    #region ChildrenMethods


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


   

    public override void OnSell()
    {
        base.OnSell();
        AudioManager.Instance.PlaySFX(createDestroySound);
        Kill();
    }
     
    #endregion ChildrenMethods

    private void LockOnTarget()
    {
        if (target != null)
        {
            // Calculate the direction from the turret to the target
            Vector3 direction = target.GetCurrentPosition() - Turret.position;

            // Adjust the direction to account for a 180-degree rotation on the y-axis
            direction = Quaternion.Euler(0, 180, 0) * direction;

            Quaternion lookRotation = Quaternion.LookRotation(direction);

            Quaternion smoothedRotation = Quaternion.Lerp(Turret.rotation, lookRotation, Time.deltaTime * 10f);

            Vector3 smoothedEulerAngles = smoothedRotation.eulerAngles;

            Turret.rotation = Quaternion.Euler(0, smoothedEulerAngles.y, 0);



        }

    }

    private void Laser()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, firePoint.position);

        if (target != null && target.TryGetComponent(out AIController controller))
        {
            lineRenderer.SetPosition(1, target.GetCurrentPosition());
            if (Time.time - lastLaserDamageTime >= GetStatistics().ActivationTime)
            {
                controller.TakeDamage(laserDamage);
                lastLaserDamageTime = Time.time;
                laserDamage += laserDamageDiff;
            }
        }
    }


RaycastHit hitInfo;
    void UpdateTarget()
    {

        Debug.Log(this + " jebany invoke " + GetStatistics().AttackRange);

        List<AIController> enemies = new List<AIController>();
        var hits = Physics.OverlapSphere(transform.position, GetStatistics().AttackRange, targetLayerMask);
        
        foreach (var hit in hits)
        {
            Debug.Log(" find " + hit.name);
            if (hit.TryGetComponent(out AIController controller))
            {
                if (controller.IsDead() == false)
                {
                    Debug.Log(" find AIController " + controller.name);
                    Vector3 directionToTarget = (controller.GetCurrentPosition() - firePoint.position).normalized; 
                    float distanceToTarget = Vector3.Distance(transform.position, controller.GetCurrentPosition());
                    if (!Physics.Raycast(firePoint.position, directionToTarget, out hitInfo, distanceToTarget, obstructionMask))
                    {
                        Debug.LogWarning("Raycast enter " + controller.name + " " + Time.time);
                        enemies.Add(controller);

                    }
                    else
                    {
                        Debug.LogWarning(this + " trafilem sciane przy controllerze " + controller.name + " " + Time.time);
                        if(controller == target)
                        {
                            target = null;
                        }
                    }
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
                return;
            }
        }
        Debug.Log(this + " Not found target");
        target = null;
    }

void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawLine(firePoint.position, hitInfo.point);
}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GetStatistics().AttackRange);

    }

   

}

