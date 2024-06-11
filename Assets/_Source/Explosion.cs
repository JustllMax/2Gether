using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    AudioSource source;
    AudioClip clip;
    ParticleSystem explosionParticles;
    float explosionDamage;
    float explosionRadius;
    float knockbackForce = 12f;
    LayerMask AIMask;
    LayerMask PlayerBuildingMask;
    LayerMask targetLayerMask;

    private void Awake()
    {
        explosionParticles = GetComponent<ParticleSystem>();
        source = GetComponent<AudioSource>();
        AIMask = 1 << LayerMask.NameToLayer("Enemy");
        PlayerBuildingMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Building");
    }

    void Update()
    {
        if(explosionParticles.isEmitting == false)
        {
            Destroy(gameObject);
        }
    }



    public void SetUpExplosion(float damage, float radius, LayerMask targetLayer, float particleScale=1f)
    {
        explosionDamage = damage;
        explosionRadius = radius;
        targetLayerMask = targetLayer;

        if(explosionParticles != null)
        {
            explosionParticles.gameObject.transform.localScale *= particleScale;
            explosionParticles.Play();
        }
            

        AudioManager.Instance.PlaySFXAtSource(source.clip, source);
        Explode();
    }
    void Explode()
    {
        var hits = Physics.OverlapSphere(transform.position, explosionRadius, targetLayerMask);
        HashSet<IDamagable> controllers = new HashSet<IDamagable>();
        PlayerController player = null;
        foreach (var hit in hits)
        {

            IDamagable controller = hit.GetComponentInParent<IDamagable>();
            if (controller != null)
            {
                controllers.Add(controller);
            }
            if (hit.TryGetComponent(out PlayerController p))
            {
                player = p;
            }
        }

        foreach (var c in controllers)
        {
            c.TakeDamage(explosionDamage);
        }
        if (player != null)
        {
            player.Velocity = (player.transform.position - transform.position).normalized * knockbackForce;
        }
    }
    void ChooseTargeting()
    {
        if(targetLayerMask == AIMask)
        {
            CheckForAICollision();
        }
        else if(targetLayerMask == PlayerBuildingMask)
        {
            CheckForPlayerBuildingCollision();
        }
    }

    void CheckForAICollision()
    {
        var hits = Physics.OverlapSphere(transform.position, explosionRadius, targetLayerMask);
        HashSet<AIController> controllers = new HashSet<AIController>();
        foreach (var hit in hits)
        {

            AIController controller = hit.GetComponentInParent<AIController>();
            if (controller != null)
            {
                controllers.Add(controller);
            }
        }

        foreach (var c in controllers)
        {
            c.TakeDamage(explosionDamage);
        }
    }

   

    void CheckForPlayerBuildingCollision()
    {
        var hits = Physics.OverlapSphere(transform.position, explosionRadius, targetLayerMask);
        HashSet<Building> buildings = new HashSet<Building>();
        PlayerController player = null;
        foreach (var hit in hits)
        {

            Building building = hit.GetComponentInParent<Building>();
            if (building != null)
            {
                buildings.Add(building);
            }
            else
            {
                player = hit.GetComponent<PlayerController>();
                if(player != null)
                {
                    player.TakeDamage(explosionDamage);
                    player.Velocity = (player.transform.position - transform.position).normalized * knockbackForce;
                }
            }
        }

        foreach (var b in buildings)
        {
            b.TakeDamage(explosionDamage);
        }
    }
}
