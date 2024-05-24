using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Transform target;
    public float speed = 1000.0f;

    public void setTarget(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if(target == null) 
        { 
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distThisFrame = speed * Time.deltaTime;
        if(dir.magnitude <= distThisFrame)
        {
            hitTarget();
            return;
        }

        transform.Translate(dir.normalized * distThisFrame, Space.World);

    }

    void hitTarget()
    {
        Damage(transform.position, 1f);
        Destroy(gameObject);
    }



    private void Damage(Vector3 center, float radius)
    {

        int enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, enemyLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out AIController controller))
            {

                controller.TakeDamage(3);

            }
        }
    }
}
