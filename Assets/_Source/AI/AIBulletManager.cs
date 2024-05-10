using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AIBulletManager : MonoBehaviour
{
    private static AIBulletManager _instance;
    public static AIBulletManager Instance { get { return _instance; } }
    ObjectPool<AIBullet> _pool;
    GameObject P_Bullet;
    float maxPoolSize = 15;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public IObjectPool<AIBullet> Pool
    {
        get
        {
            if (_pool == null)
            {
                _pool = new ObjectPool<AIBullet>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 1000, 2000);
            }
            return _pool;
        }
    }


    AIBullet CreatePooledItem()
    {
        AIBullet bullet = Instantiate(P_Bullet).GetComponent<AIBullet>();
        bullet.SetPool(_pool);
        return bullet;
    }


    void OnTakeFromPool(AIBullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    void OnReturnedToPool(AIBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    void OnDestroyPoolObject(AIBullet bullet)
    {
        Destroy(bullet);   
    }
}
