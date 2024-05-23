using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AIBullet : MonoBehaviour
{
    Vector3 direction;
    Vector3 spawnPos;
    float disappearDistance;
    ObjectPool<AIBullet> _pool;
    float damage;
    void Start()
    {
        spawnPos = transform.position;
    }

    void Update()
    {
        transform.position += direction * Time.deltaTime;
        float distance = Vector3.Distance(spawnPos, transform.position);
        if(distance > disappearDistance)
        {
            _pool.Release(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(damage);
        }
        _pool.Release(this);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void SetPool(ObjectPool<AIBullet> pool)
    {
        _pool = pool;
    }
}
