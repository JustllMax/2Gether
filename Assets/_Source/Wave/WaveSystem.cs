using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _spawnPoints = new List<GameObject>();

    [Header("System state")]
    private bool _isWaveActive = false;
    private float _elapsedTime = 0;
    public float elapsedTime {get { return _elapsedTime; }}
    private float _spawnEnemyTimer;
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

    public void BeginWave(WaveData waveData)
    {
        if (_isWaveActive)
            return;

        _isWaveActive = true;

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
                    Instantiate(wave.EnemyPool.GetNextEnemy(i), spawnPoint.transform.position + Vector3.up, Quaternion.identity);
                    enemyCount++;
                }
                await UniTask.WaitForSeconds(wave.EnemySpawnInterval);
            }
            await UniTask.WaitForSeconds(wave.Cooldown);
        }
        _isWaveActive = false;
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
        _isWaveActive = false;
    }
#endregion

    private void Update()
    {
        if (_isWaveActive)
        {
            _elapsedTime += Time.deltaTime;
            Debug.LogWarning(_elapsedTime);
        }
    }

    public void AddSpawnPoint(GameObject spawnPoint)
    {
        if (spawnPoint != null)
        {
            _spawnPoints.Add(spawnPoint);
        }
        else
        {
            Debug.LogError("Attempted to add a null spawn point");
        }
    }
}
