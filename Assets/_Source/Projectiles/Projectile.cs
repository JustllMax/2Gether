using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] ProjectileProperties properties;
    [SerializeField] float activeDelay = 0.02f;
    [SerializeField] Vector3 lastPos;
    [SerializeField] float lifetime;

    ObjectPool<Projectile> pool;
    Collider collider;
    MeshRenderer renderer;
    public Rigidbody rb;

    void Awake()
    {
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        renderer = GetComponent<MeshRenderer>();
    }

    void OnEnable()
    {
        transform.localScale = Vector3.zero;
        lifetime = 0;
        collider.enabled = false;
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }

    public void SetData(in Vector3 velocity, ProjectileProperties properties)
    {
        rb.velocity = velocity;
        rb.useGravity = properties.hasGravity;
        renderer.material = properties.material;
        this.properties = properties;
    }

    private void Update()
    {
        lifetime += Time.deltaTime;

        if (!collider.enabled)
        {
            if (lifetime >= activeDelay) 
            {
                collider.enabled = true;
            }

            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f) * (lifetime / activeDelay);
        } else
        {
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
    }

    void FixedUpdate()
    {
        if (collider.enabled)
        {
            Vector3 offset = transform.position - lastPos;
            if (Physics.Raycast(lastPos, offset.normalized, out RaycastHit hit, offset.magnitude, properties.surfaceMask))
            {
                 transform.position = hit.point;
                OnTriggerEnter(hit.collider);
            }
        }
        lastPos = transform.position;

        if (lifetime > properties.lifetime)
        {
            pool.Release(this);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (properties.explodes)
        {
            Explosion explosion = ExplosionSpawner.SpawnExplosion(transform.position).GetComponent<Explosion>();
            explosion.SetUpExplosion(properties.damage, properties.explosionRadius, properties.damageMask, properties.explosionRadius / 5.0f);
            pool.Release(this);
            return;
        }


        GameObject gObject = null;
        if (other.attachedRigidbody != null)
        {
            gObject = other.attachedRigidbody.gameObject;
        } 
        else
        {
            gObject = other.gameObject;
        }

        if (gObject != null)
        {
            if ((((1 << gObject.layer) & properties.damageMask.value) != 0) && gObject.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.TakeDamage(properties.damage);
            }
        }

        pool.Release(this);
    }

    public void SetPool(ObjectPool<Projectile> pool)
    {
        this.pool = pool;
    }
}
