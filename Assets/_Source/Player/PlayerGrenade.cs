using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenade : MonoBehaviour
{
    float projectileSpeed;
    Vector3 direction;
    float disappearDistance;
    Vector3 spawnPos;
    float explosionDamage; 
    float explosionRadius; 
    LayerMask explosionMask;
    public void SetUp(float speed, Vector3 dir, float maxDistance, float explosionDamage, float explosionRadius, LayerMask explosionMask)
    {
        direction = dir;
        projectileSpeed = speed;
        disappearDistance = maxDistance;
        spawnPos = transform.position;
        this.explosionDamage = explosionDamage;
        this.explosionRadius = explosionRadius;
        this.explosionMask = explosionMask;
    }

    void Update()
    {
        transform.position += direction * projectileSpeed * Time.deltaTime;
        float distance = Vector3.Distance(spawnPos, transform.position);
        if(distance > disappearDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Explosion explosion = ExplosionSpawner.SpawnExplosion(transform.position).GetComponent<Explosion>();
        explosion.SetUpExplosion(explosionDamage, explosionRadius, explosionMask);
    }
}
