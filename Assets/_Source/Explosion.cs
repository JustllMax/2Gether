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
            source.Stop();
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
}
