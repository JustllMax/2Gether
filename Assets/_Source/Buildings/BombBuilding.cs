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


    public override void Start()
    {
    }

    public void Update()
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

    #region ChildrenMethods


    public override void OnAttack()
    {
        if (target != null)
        {
            GameObject bombGO = Instantiate(bombPrefab, firePoint.position, firePoint.rotation);
            Bomb bomb = bombGO.GetComponent<Bomb>();
            if (bomb != null)
            {
                bomb.SeekTarget(target);
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

    public override void Kill(bool desintegrate = false)
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
