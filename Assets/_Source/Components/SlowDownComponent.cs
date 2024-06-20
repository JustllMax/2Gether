using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.ParticleSystem;

public class SlowDownComponent : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    float slowAmount;
    float effectDuration;
    float effectTimer = 0f;
    AIController ai;

    private void Awake()
    {
        ai = GetComponent<AIController>();
        this.enabled = false;
    }

    private void OnDestroy()
    {
        if (ai == null)
        {
            return;
        }
        ai.OnMovementStatsChanged -= ApplySlowEffect;
    }

    void ApplySlowEffect(ref EnemyMovement newMovement)
    {
        newMovement.acceleration *= slowAmount;
        newMovement.turnSpeed *= slowAmount;
        newMovement.movementSpeed *= slowAmount;
        newMovement.extraRotationSpeed *= slowAmount;
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
        if (speedModifier >= 100)
        {
            speedModifier /= 100;
        }
        slowAmount = 1 - speedModifier;

        if (!enabled && ai != null)
        {
            ai.OnMovementStatsChanged += ApplySlowEffect;
            ai.ApplyTargetMovement();
        }

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
    }
    void DeactivateEffect()
    {
        if (particles != null)
        {
            particles.Stop();
        }
        effectTimer = 0f;
        if(ai != null)
        {
            ai.OnMovementStatsChanged -= ApplySlowEffect;
            ai.ApplyTargetMovement();
        }
        this.enabled = false;
    }
}
