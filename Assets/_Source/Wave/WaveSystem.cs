using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _spawnPoints = new List<GameObject>();

    [SerializeField]
    private List<GameObject> _particlesPrefab = new List<GameObject>();

    [Header("System state")]
    public bool isWaveActive = false;
    public bool isSpawnActive = false;
    private float _elapsedTime = 0;
    public float elapsedTime { get { return _elapsedTime; } }
    public static int nightCount = 0;
    public int enemyCount = 0;

    #region Wave handle
    public GameObject GetRandomSpawnPoint()
    {
        if (_spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points available");
            return null;
        }

        return _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)];
    }
    /*
        public void BeginWave(WaveData waveData)
        {
            if (isWaveActive)
                return;

            isWaveActive = true;

            _ = handleWave(waveData);
            //StartCoroutine(handleWaveCoroutine(_waveData));

            nightCount++;
        }
        private async UniTaskVoid handleWave(WaveData data)
        {
            foreach (var wave in data.Waves)
            {

                for (int i = 0; i < wave.EnemyPool.EnemyCount; i++)
                {
                    var spawnPoint = GetRandomSpawnPoint();
                    if (spawnPoint != null)
                    {
                        Instantiate(wave.EnemyPool.GetNextEnemy(i), spawnPoint.transform.position + (Vector3.one*10) +  Vector3.up, Quaternion.identity);
                        enemyCount++;
                    }
                    await UniTask.WaitForSeconds(wave.EnemySpawnInterval);
                }
                await UniTask.WaitForSeconds(wave.Cooldown);
            }
            isWaveActive = false;
        }

        private IEnumerator handleWaveCoroutine(WaveData data)
        {
            foreach (var wave in data.Waves)
            {
                yield return new WaitForSeconds(wave.Cooldown);
                for (int i = 0; i < wave.EnemyPool.EnemyCount; i++)
                {
                    var spawnPoint = GetRandomSpawnPoint();
                    if (spawnPoint != null)
                    {
                        Instantiate(wave.EnemyPool.GetNextEnemy(i), spawnPoint.transform.position + Vector3.up, Quaternion.identity);
                        enemyCount++;
                    }
                    yield return new WaitForSeconds(wave.EnemySpawnInterval);
                }
            }
            isWaveActive = false;
        }

    */

    public void BeginWave(List<SingleWave> waveData)
    {
        if (isWaveActive)
            return;

        isWaveActive = true;

        _ = handleWave(waveData);

        nightCount++;
    }

    private async UniTaskVoid handleWave(List<SingleWave> data)
    {
        isSpawnActive = true;
        foreach (var wave in data)
        {
            for (int i = 0; i < wave.EnemyPool.Count; i++)
            {
                
                var spawnPoint = GetRandomSpawnPoint();
                if (spawnPoint != null)
                {
                    var riff = spawnPoint.transform.GetChild(1).GetComponent<ParticleSystem>();
                    var outer = spawnPoint.transform.GetChild(2).GetComponent<ParticleSystem>();

                    riff.Play();
                    await UniTask.WaitForSeconds(wave.EnemySpawnInterval + 1f);
                    riff.Stop();
                    outer.Play();
                    if (!isWaveActive)
                    {
                        isSpawnActive = false;
                        return;
                    }
                    Instantiate(wave.EnemyPool[i], spawnPoint.transform.position + new Vector3(10, 1, 10), Quaternion.identity);
                    enemyCount++;
                }
                //await UniTask.WaitForSeconds(wave.EnemySpawnInterval);
            }
            do
            {
                await UniTask.WaitForSeconds(1);
                if (enemyCount <= 0 && isWaveActive && isSpawnActive)
                    break;
            } while (_elapsedTime < wave.Cooldown);
        }
        isSpawnActive = false;
    }
    #endregion

    private void Update()
    {
        if (isWaveActive)
        {
            _elapsedTime += Time.deltaTime;
        }
    }

    public void AddSpawnPoint(GameObject spawnPoint)
    {
        if (spawnPoint != null)
        {

            var particleRiff = Instantiate(_particlesPrefab[0], spawnPoint.transform.position + new Vector3(10, 1, 10), Quaternion.identity);
            var particleOut = Instantiate(_particlesPrefab[1], spawnPoint.transform.position + new Vector3(10, 1, 10), Quaternion.identity);

            particleRiff.transform.SetParent(spawnPoint.transform);
            particleRiff.GetComponent<ParticleSystem>().Stop();

            particleOut.transform.SetParent(spawnPoint.transform);
            particleOut.GetComponent<ParticleSystem>().Stop();

            _spawnPoints.Add(spawnPoint);
        }
        else
        {
            Debug.LogError("Attempted to add a null spawn point");
        }
    }
}
