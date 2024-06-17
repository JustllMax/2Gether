using UnityEngine;

public class PlayerGrenade : MonoBehaviour
{
    float lifetime;
    Vector3 lastPos;
    float explosionDamage; 
    float explosionRadius; 
    LayerMask explosionMask;
    [SerializeField] LayerMask raycastBlacklist;

    Rigidbody rb;
    float delay = 0.03f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetUp(float speed, Vector3 dir, float lifetime, float explosionDamage, float explosionRadius, LayerMask explosionMask)
    {
        lastPos = transform.position;
        this.lifetime = lifetime;
        this.explosionDamage = explosionDamage;
        this.explosionRadius = explosionRadius;
        this.explosionMask = explosionMask;

        rb.velocity = dir * speed;
    }

    void FixedUpdate()
    {
        delay -= Time.fixedDeltaTime;
        if (delay <= 0f)
        {
            Vector3 offset = transform.position - lastPos;
            if (Physics.Raycast(lastPos, offset.normalized, out RaycastHit hit, offset.magnitude, ~raycastBlacklist))
            {
                Explode(hit.point);
            }
        }
        lastPos = transform.position;

        lifetime -= Time.fixedDeltaTime;
        if(lifetime < 0)
        {
            Explode(transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode(transform.position);
    }

    private void Explode(Vector3 pos)
    {
        Explosion explosion = ExplosionSpawner.SpawnExplosion(transform.position).GetComponent<Explosion>();
        explosion.SetUpExplosion(explosionDamage, explosionRadius, explosionMask);
        Destroy(gameObject);
    }
}
