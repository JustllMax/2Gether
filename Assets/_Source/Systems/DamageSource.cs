using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField]
    private Damage _damage = new Damage(DamageType.Kinetic, 10f);

    [SerializeField]
    private float _damageInterval = 0.25f;
    private float _damageIntervalTime;

    [SerializeField]
    private float _lifeTime = 1f;
    private float _spawnTime;

    private BoxCollider _bCollider;
    private SphereCollider _sCollider;

    public event Action<GameObject> onDespawn;

    void Start()
    {
        _bCollider = GetComponent<BoxCollider>();
        _sCollider = GetComponent<SphereCollider>();
        _spawnTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (Time.time < _damageIntervalTime)
        {
            return;
        }

        _damageIntervalTime = Time.time + _damageInterval;

        Collider[] colliders = GetCollidersType();
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == gameObject)
            {
                continue;
            }

            DamageReceiver receiver = collider.GetComponent<DamageReceiver>();
            if (receiver != null)
            {
                receiver.ApplyDamage(_damage);
            }
        }

        if (_lifeTime >= 0 && Time.time > _spawnTime + _lifeTime)
        {
            Despawn();
        }
    }

    private Collider[] GetCollidersType()
    {
        if (_bCollider != null)
        {
            return Physics.OverlapBox(transform.TransformPoint(_bCollider.center), Vector3.Scale(transform.localScale, _bCollider.size / 2), transform.rotation);
        }
        else if (_sCollider != null)
        {
            Vector3 scale = transform.localScale;
            float maxScale = Mathf.Max(scale.x, scale.y, scale.z);
            return Physics.OverlapSphere(transform.TransformPoint(_sCollider.center), maxScale * _sCollider.radius);
        }
        else
        {
             return new Collider[0];
        }
    }

    private void Despawn()
    {
        onDespawn?.Invoke(gameObject);
        Destroy(gameObject);
    }
}
