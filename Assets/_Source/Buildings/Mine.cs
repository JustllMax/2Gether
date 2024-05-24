using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public ParticleSystem particles;
    public MeshRenderer meshRenderer;


    void Start()
    {

    }

    void Update()
    {

    }

    public void OnAttack()
    {
        if (particles != null)
        {
            particles.Play();
            meshRenderer.enabled = false;
        }
        else
        {
            Debug.Log("particle system not detected");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        OnAttack();
        ExplosionDamage(transform.position, 2f, 7f);
        Destroy(gameObject,1.1f);
    }

    private void OnDestroy()
    {
        if (particles != null)
        {
            particles.Play();
        }
    }

    private void ExplosionDamage(Vector3 center, float radius, float force)
    {

        int enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, enemyLayer);

        foreach (var hitCollider in hitColliders)
        {
            Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //push
                Vector3 direction = hitCollider.transform.position - center;
                rb.AddForce(direction.normalized * force, ForceMode.Impulse);
            }



            if (hitCollider.TryGetComponent(out AIController controller))
            {

                controller.TakeDamage(controller.GetEnemyStats().AttackDamage);

            }
        }
    }
}
