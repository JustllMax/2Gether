using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AIBullet : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 spawnPos;
    private float disappearDistance = 1000f;
    private ObjectPool<AIBullet> _pool;
    private float damage;
    private float _speed = 1;

    void Start()
    {
        spawnPos = transform.position;
    }

    void Update()
    {
        transform.position += direction * _speed * Time.deltaTime;
        float distance = Vector3.Distance(spawnPos, transform.position);
        if(distance > disappearDistance)
        {
            _pool.Release(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ITargetable targetable))
        {
            other.GetComponent<IDamagable>().TakeDamage(damage);
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

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
