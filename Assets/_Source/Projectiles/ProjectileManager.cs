using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager _instance;
    public static ProjectileManager Instance { get { return _instance; } }
    ObjectPool<Projectile> _pool;
    [SerializeField] private GameObject P_Projectile;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public Projectile SpawnProjectile(in Vector3 position, in Vector3 direction, ProjectileProperties properties)
    {
        Projectile p = Pool.Get();
        if (p == null)
            return null;

        p.rb.position = position;
        p.SetData(direction * properties.speed, properties);
        return p;
    }

    public IObjectPool<Projectile> Pool
    {
        get
        {
            if (_pool == null)
            {
                _pool = new ObjectPool<Projectile>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 1000, 2000);
            }
            return _pool;
        }
    }


    Projectile CreatePooledItem()
    {
        Projectile bullet = Instantiate(P_Projectile).GetComponent<Projectile>();
        bullet.SetPool(_pool);
        Debug.Log("Created new bullet " + bullet.gameObject.GetInstanceID());
        return bullet;
    }


    void OnTakeFromPool(Projectile bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    void OnReturnedToPool(Projectile bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    void OnDestroyPoolObject(Projectile bullet)
    {
        Destroy(bullet);   
    }
}
