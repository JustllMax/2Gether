using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    AudioSource source;
    AudioClip clip;
    ParticleSystem particles;


    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(particles.isEmitting == false && particles.particleCount == 0)
        {
            source.Stop();
            Destroy(gameObject);
        }
    }



    public void SetUpParticles(float particleScale=1f)
    {

        if(particles != null)
        {
            particles.gameObject.transform.localScale *= particleScale;
            particles.Play();
        }
            
        AudioManager.Instance.PlaySFXAtSource(source.clip, source);
    }
    
}
