using UnityEngine;

public class Turret : Building
{
    private float range = 15f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private Transform target;
    private string enemyTag = "Enemy";

    private float HealthPoint = 1000f;
    private int price = 100;
    public float fireContDown = 0f;
    public float fireRate = 3f;

    //for laser
    [Header("For Laser")]
    public bool useLaser = false;
    public LineRenderer lineRenderer;


    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

   
        

    private void Update()
    {
        LockOnTarget();
        if(target == null && useLaser && lineRenderer.enabled == true)
            lineRenderer.enabled = false;

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
                Debug.Log("bam");

                fireContDown = 1f / fireRate;
            }
            fireContDown -= Time.deltaTime;
        }

    }

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
                bullet.setTarget(target);
            }
        }
    }

    public override void OnTakeDamage()
    {
        //todo 
    }

    public override void OnUpgrade()
    {
        
    }

    public override void OnSell()
    {
        GoldManager.Instance.GoldAdd(price);
    }

    //


    private void LockOnTarget()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

    }

    private void Laser()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

    }
   

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }


}

