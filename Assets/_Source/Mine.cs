using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIne : MonoBehaviour
{
    public ParticleSystem particles;
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
            Expode();
        }
    }

    private void Expode()
    {
        OnAttack();
        Destroy(gameObject);
    }
}
