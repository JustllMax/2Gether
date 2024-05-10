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
    }

    private void Update()
    {

        if(fireContDown <= 0f && target)
        {
            OnAttack();
            fireContDown = 1f / fireRate;
        }
        fireContDown -= Time.deltaTime;
        
        UpdateTarget();
        if (target == null)
            return;
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

    


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
