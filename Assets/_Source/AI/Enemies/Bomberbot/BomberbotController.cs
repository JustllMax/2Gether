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

    public override void Kill() 
    {
        isDead = true;
        foreach (Collider col in hitboxColliders)
        {
            col.enabled = false;
        }
        GetNavMeshAgent().enabled = false;

        PlayAnimation("DEATH");
        Invoke("Explode", DeathInvokeTime);

        WaveManager.Instance.waveSystem.enemyCount--;
    }

    private void Explode()
    {
        Debug.Log(this + " attack performed");

        var hits = Physics.OverlapSphere(GetCurrentPosition(), explosionRadius, explosionMask);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IDamagable damagable1))
            {
                if (damagable1.TakeDamage(explosionDamage))
                {
                    Debug.Log("Explosion hit target");
                }

                if (hit.TryGetComponent(out PlayerController player))
                {
                    player.Velocity = (player.transform.position - transform.position).normalized * 12;
                }

            } else if (hit.attachedRigidbody && hit.attachedRigidbody.TryGetComponent(out IDamagable damagable2))
            {
                if (damagable2.TakeDamage(explosionDamage))
                {
                    Debug.Log("Explosion hit target");
                }
            }
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
            renderer.enabled = false;

        if (explosionParticles != null)
        {
            InstantiateGameObject(explosionParticles.gameObject, transform).transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        }


        AudioManager.Instance.PlaySFXAtSource(attackSound, audioSource);


        Invoke("DestroyObj", 2);

    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }

}

