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
        Destroy(gameObject,1.1f);
    }

    private void OnDestroy()
    {
        if (particles != null)
        {
            particles.Play();
        }
    }
}
