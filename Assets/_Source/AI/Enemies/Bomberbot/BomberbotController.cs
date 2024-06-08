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
        hitboxCollider.enabled = false;
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
            if (hit.TryGetComponent(out IDamagable damagable))
            {

                if (damagable.TakeDamage(explosionDamage))
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

