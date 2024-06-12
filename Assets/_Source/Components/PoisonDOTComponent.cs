using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoisonDOTComponent : MonoBehaviour
{
    float originalSpeed;
    float effectDuration;
    float effectTimer = 0f;
    float tickTimer = 1f;
    float tickDelay;
    float damage;
    IDamagable target;
    private void Update()
    {
        effectTimer += Time.deltaTime;
        if (effectTimer >= effectDuration)
        {
            DeactivateEffect();
        }
        tickTimer += Time.deltaTime;
        if(tickTimer >= tickDelay)
        {
            tickTimer = 0;
            target.TakeDamage(damage);
        }
    }

    public void SetUpPoisonEffect(float damagePerTick, float tickDelay, float duration)
    {

        this.enabled = true;

        if (target == null && TryGetComponent(out IDamagable damagable))
        {
            target = damagable;
        }
        effectDuration = duration;
        effectTimer = 0f;
        this.tickDelay = tickDelay; 
        damage = damagePerTick;

    }
    void DeactivateEffect()
    {
        tickTimer = 1f;
        effectTimer = 0f;
        this.enabled = false;
    }
}
