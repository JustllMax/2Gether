using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlowDownComponent : MonoBehaviour
{
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

    public void SetUpSlowEffect(float speedModifier, float duration)
    {
        this.enabled = true;
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
        effectTimer = 0f;
        if(agent != null)
        {
            agent.speed = originalSpeed;
        }
        this.enabled = false;
    }
}
