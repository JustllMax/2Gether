using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.ParticleSystem;

public class SlowDownComponent : MonoBehaviour
{
    [SerializeField]ParticleSystem particles;
    NavMeshAgent agent;
    float originalSpeed;
    float effectDuration;
    float effectTimer = 0f;

    private void Awake()
    {
        if (TryGetComponent(out NavMeshAgent a))
        {
            agent = a;
            originalSpeed = agent.speed;
            Debug.Log(this + "Got nav mesh agent");
        }
  
    }


    private void Update()
    {
        effectTimer += Time.deltaTime;
        if (effectTimer >= effectDuration)
        {
            DeactivateEffect();
        }
    }

    public void SetUpSlowEffect(float speedModifier, float duration, float particlesScaleModifier = 1f)
    {
        this.enabled = true;
        if (particles == null)
        {
            GameObject poisonPartcilesPrefab =  Resources.Load<GameObject>("P_SlowEffectParticles");
            if (poisonPartcilesPrefab == null)
            {
                Debug.LogError($"Prefab 'P_SlowEffectParticles' not found in Resources folder.");
            }
            particles = Instantiate(poisonPartcilesPrefab, transform).GetComponent<ParticleSystem>();
            particles.transform.localScale *= particlesScaleModifier;
            particles.Play();
        }
        else
        {
            particles.Play();
        }
        effectDuration = duration;
        effectTimer = 0f;
        if (speedModifier >= 100)
        {
            speedModifier /= 100;
        }
        
        float slowAmount = 1 - speedModifier;
        if(agent != null)
        {
            Debug.Log(this + "Applied slow down effect");
            agent.speed = originalSpeed;
            agent.speed *= slowAmount;
        }
    }
    void DeactivateEffect()
    {
        if (particles != null)
        {
            particles.Stop();
        }
        effectTimer = 0f;
        if(agent != null)
        {
            agent.speed = originalSpeed;
        }
        this.enabled = false;
    }
}
