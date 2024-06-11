using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;



public class EldritchController : AIController
{
    [Foldout("DEBUG INFO")]
    public float receivedDamage = 0;


    public override void Kill() 
    {
        isDead = true;
        foreach (Collider col in hitboxColliders)
        {
            col.enabled = false;
        }
        GetNavMeshAgent().enabled = false;

        PlayAnimation("DEATH");
        Invoke("DestroyObj", DeathInvokeTime);

        WaveManager.Instance.waveSystem.enemyCount--;
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }

    public void Relocate(float minDistance, float maxDistance)
    {
        receivedDamage = 0.0f;

        if (!HasTarget())
        {
            return;
        }

        uint maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPos = UnityEngine.Random.insideUnitSphere * maxDistance;
            randomPos += GetCurrentTarget().transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, maxDistance, 1))
            {
                if (Vector3.Distance(GetCurrentTarget().transform.position, hit.position) >= minDistance)
                {
                    _navMeshAgent.Warp(hit.position);
                    Debug.Log("Enemy teleported to: " + hit.position);
                    return;
                }
            }
        }

        SetCurrentTarget(new AITarget(null, null));
        Debug.LogWarning("Could not find a valid position after " + maxAttempts + " attempts.");
    }

    public override bool TakeDamage(float damage)
    {
        if (isDead)
            return false;

        receivedDamage += damage;
        Health -= damage;
        AudioManager.Instance.PlaySFXAtSource(hurtSound, audioSource);
        if (Health <= 0)
        {
            AudioManager.Instance.PlaySFXAtSource(deathSound, audioSource);
            Kill();
            return true;
        }
        return false;
    }

}

