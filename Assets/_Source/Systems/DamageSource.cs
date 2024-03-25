using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageSource : MonoBehaviour
{
    [SerializeField]
    private Damage _damage = new Damage(10f);

    [SerializeField]
    private float _damageInterval = 0.25f;
    private float _damageIntervalTime;

    [SerializeField]
    private float _lifeTime = 1f;

    [SerializeField]
    protected Vector3 _center;

    //public event Action<GameObject> onDespawn;


    private void FixedUpdate()
    {
        _lifeTime -= Time.fixedDeltaTime;
        if (Time.time < _damageIntervalTime)
        {
            return;
        }

        _damageIntervalTime = Time.time + _damageInterval;

        Collider[] colliders = GetNearColliders();
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == gameObject)
            {
                continue;
            }

            IDamagable receiver = collider.GetComponent<IDamagable>();
            if (receiver != null)
            {
                receiver.TakeDamage(_damage);
            }
        }

        if (_lifeTime <= 0)
        {
            Despawn();
        }
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        DrawGizmos();
    }

    private void Despawn()
    {
        //onDespawn?.Invoke(gameObject);
        Destroy(gameObject);
    }

    protected abstract Collider[] GetNearColliders();
    protected abstract void DrawGizmos();

}