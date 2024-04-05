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


    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if (target == null)
            return;
    }

    public override void OnAttack()
    {
        if (target != null)
        {
            GameObject bombGO = Instantiate(bombPrefab, firePoint.position, firePoint.rotation);
            Bomb bomb = bombGO.GetComponent<Bomb>();
            if (bomb != null)
            {
                bomb.SetTargetPosition(target.position);
            }
        }
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
            target = other.transform;
            OnAttack();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
