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
    private float _spawnEnemyTimer;

    private WaveData _waveData;

    public static int nightIndex = 0;

    public static event Action OnEndWave;

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
        _waveData = waveData;

        _ = handleWave(_waveData);

        WaveSystem.nightIndex++;
    }

    private async UniTaskVoid handleWave(WaveData data)
    {
        int waveNumber = (WaveSystem.nightIndex < data.Waves.Count) ? WaveSystem.nightIndex : data.Waves.Count - 1;
        Debug.Log("Current wave: " + waveNumber + " Wave count: " + data.Waves.Count);

        foreach (var wave in data.Waves)
        {
            await UniTask.WaitForSeconds(wave.Cooldown);
            for (int i = 0; i < wave.EnemyPool.EnemyCount; i++)
            {
                var spawnPoint = GetRandomSpawnPoint();
                if (spawnPoint != null)
                {
                    Instantiate(wave.EnemyPool.GetNextEnemy(i), spawnPoint.transform.position + Vector3.up, Quaternion.identity);
                }
                await UniTask.WaitForSeconds(wave.EnemySpawnInterval);
            }
        }
        _isWaveActive = false;

        OnEndWave?.Invoke();
    }

    private void Update()
    {
        if (_isWaveActive)
        {
            _elapsedTime += Time.deltaTime;
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
