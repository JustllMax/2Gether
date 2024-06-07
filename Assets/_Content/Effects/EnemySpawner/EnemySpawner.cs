using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerEffect : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject spawnEffectParticles1;
    public GameObject spawnEffectParticles2;
    public float riftDuration;
    public float enemyWidth;
    public float enemyHeight;

    private float spawnDelay = 1.0f;

    void Start()
    {
        spawnEffectParticles1 = Instantiate(spawnEffectParticles1, transform.position + new Vector3(0.0f,1.0f,0.0f), transform.rotation);
    }

    void Update()
    {
        riftDuration -= Time.deltaTime;

        if (riftDuration <= 0)
        {
            if (riftDuration > -10)
            {
                spawnEffectParticles2 = Instantiate(spawnEffectParticles2, transform.position, transform.rotation);
                riftDuration = -10;
            }

            spawnDelay -= Time.deltaTime;
            if (spawnDelay <= 0)
            {
                if (spawnDelay > -10)
                {
                    Instantiate(enemyPrefab, transform.position, transform.rotation);
                    Destroy(spawnEffectParticles1);
                    spawnDelay = -10;
                }



                if (spawnDelay <= -12)
                {
                    Destroy(spawnEffectParticles2);
                    Destroy(gameObject);
                }
            }
        }
    }
}
