using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building
{
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] GameObject model;


    #region ChildrenMethods

    public override void OnCreate()
    {

    }

    public override void OnAttack()
    {
        if (explosionParticles != null)
        {
            explosionParticles.Play();
            model.SetActive(false);
        }
        else
        {
            Debug.Log("particle system not detected");
        }
    }


    public override bool TakeDamage(float damage)
    {
        audioSource.PlayOneShot(takeHitSound);
        Health -= damage;
        if (Health <= 0)
        {
            Kill();
            return true;
        }
        return false;
    }

    public override void Kill()
    {
        IsTargetable = false;
        audioSource.PlayOneShot(createDestroySound);
        createDestroyParticles.Play();
        Invoke("DestroyObj", DestroyObjectDelay);
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }

    public override void OnSell()
    {
        base.OnSell();
        Kill();
    }

    #endregion ChildrenMethods

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
        Destroy(gameObject,0.2f);
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
