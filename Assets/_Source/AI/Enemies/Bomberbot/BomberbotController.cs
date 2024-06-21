using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class  BomberbotController : AIController
{

    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] LayerMask explosionMask;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionDamage;
    [SerializeField] float particlesScale = 3f;
    public override void Kill() 
    {
        if (isDead)
            return;

        isDead = true;

        GetNavMeshAgent().enabled = false;

        StartExploding();
    }

    public void StartExploding()
    {
        PlayAnimation("DEATH");
        Invoke("Explode", DeathInvokeTime);
    }

    private void Explode()
    {
        isDead = true;
        GetNavMeshAgent().enabled = false;
        foreach (Collider col in hitboxColliders)
        {
            col.enabled = false;
        }

        ExplosionSpawner.SpawnExplosion(transform.position).SetUpExplosion(explosionDamage, explosionRadius, explosionMask, particlesScale);
        Destroy(gameObject);
        WaveManager.Instance.EnemyHasBeenKilled();
    }
}

