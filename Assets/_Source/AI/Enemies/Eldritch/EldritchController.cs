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

    [SerializeField]
    AudioClip teleportSound;

    public override void Kill() 
    {
        isDead = true;
        foreach (Collider col in hitboxColliders)
        {
            col.enabled = false;
        }

        if (currentState != null)
            currentState.OnExit(this);

        GetNavMeshAgent().enabled = false;

        PlayAnimation("DEATH");

        WaveManager.Instance.waveSystem.enemyCount--;
        StartCoroutine(KillEffects());
    }

    IEnumerator KillEffects()
    {
        yield return new WaitForSeconds(DeathInvokeTime);
        TpEffect(transform.position - new Vector3(0, 20, 0));

        yield return new WaitForSeconds(0.5f);
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
            randomPos += CurrentTarget.transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, maxDistance, 1))
            {
                if (Vector3.Distance(CurrentTarget.transform.position, hit.position) >= minDistance)
                {
                    TpEffect(hit.position);
                    Debug.Log("Eldritch teleported to: " + hit.position);
                    break;
                }
            }
        }

        Debug.LogWarning("Could not find a valid position after " + maxAttempts + " attempts.");
    }

    private void TpEffect(in Vector3 pos)
    {
        AudioManager.Instance.PlaySFXAtSource(teleportSound, audioSource);
        var particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var particle in particles)
        {
            particle.Play();
        }
        StartCoroutine(Warp(pos));
    }

    IEnumerator Warp(Vector3 position)
    {
        yield return null;
        CurrentTarget = null;
        _navMeshAgent.Warp(position);
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

