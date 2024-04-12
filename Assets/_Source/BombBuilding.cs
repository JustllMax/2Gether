using UnityEngine;

public class BombBuilding : Building
{
    private float range = 15f;
    public GameObject bombPrefab;
    public Transform firePoint;

    private Transform target;
    private string enemyTag = "Enemy";

    private float HealthPoint = 700f;
    private int price = 200;

    public float fireContDown = 0f;
    public float fireRate = .3f;


    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if (target == null)
            return;

        if(fireContDown <= 0f)
        {
            OnAttack();
            Debug.Log("bam");
            fireContDown = 1f / fireRate;
        }
        fireContDown -= Time.deltaTime;
    }

    public override void OnAttack()
    {
        if (target != null)
        {
            GameObject bombGO = Instantiate(bombPrefab, firePoint.position, firePoint.rotation);
            Bomb bomb = bombGO.GetComponent<Bomb>();
            if(bomb != null)
            {
                bomb.SeekTarget(target);
            }

        }
    }
    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        target = nearestEnemy;
    }


    public override void OnCreate()
    {
        //todo
    }

    public override void OnTakeDamage()
    {
        //todo
    }

    public override void OnUpgrade()
    {
        //todo
    }

    public override void OnSell()
    {
        GoldManager.Instance.GoldAdd(price);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            UnityEngine.Debug.Log("Enemy entered zone");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            target = other.transform;
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
