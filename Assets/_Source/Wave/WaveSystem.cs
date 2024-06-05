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

    private int _subWaveIndex;

    void OnEnable()
    {
        Debug.Log("WaveSystem Enable");
        GameManager.OnGameManagerReady += OnStart;
    }
    void OnDisable()
    {
        GameManager.OnGameManagerReady -= OnStart;
    }
    private void OnStart()
    {
        Debug.Log("WaveSystem Invoke");
        // Clear the list before populating to avoid duplication
        //_spawnPoints.Clear();

        if (SlotPlacer.Instance == null)
        {
            Debug.LogError("SlotPlacer instance is null");
            return;
        }

        foreach (var spawn in SlotPlacer.Instance.spawnSlots)
        {
            if (spawn == null)
            {
                Debug.LogError("Spawn point is null");
                continue;
            }

            Debug.Log(spawn.GetInstanceID() + " " + spawn.gameObject.name);
            AddSpawnPoint(spawn.gameObject);
            //_spawnPoints.Add(spawn.gameObject);
        }
        Debug.Log("Count " + _spawnPoints.Count);
        foreach (var spawn in _spawnPoints)
        {
            Debug.Log(spawn.GetInstanceID() + " " + spawn.gameObject.name);
        }

        BeginWave(Resources.Load<WaveData>("wave_test"));
    }

    public GameObject GetRandomSpawnPoint()
    {
        Debug.Log("Rand spawn point");
        if (_spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points available");
            return null;
        }

        return _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)];
    }

    public void BeginWave(WaveData waveData)
    {
        Debug.Log("Begin wave");
        if (_isWaveActive)
            return;

        _isWaveActive = true;
        _waveData = waveData;

        _ = handleWave(_waveData);
    }

    private async UniTaskVoid handleWave(WaveData data)
    {
        Debug.Log("Handle wave");
        foreach (var wave in data.Waves)
        {
            for (int i = 0; i < wave.EnemyPool.EnemyCount; i++)
            {
                var spawnPoint = GetRandomSpawnPoint();
                if (spawnPoint != null)
                {
                    Instantiate(wave.EnemyPool.GetNextEnemy(i), spawnPoint.transform.position + Vector3.up, Quaternion.identity);
                }
                await UniTask.WaitForSeconds(wave.EnemySpawnInterval);
            }

            await UniTask.WaitForSeconds(wave.Cooldown);
        }

        _isWaveActive = false; // Reset wave active state when done
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
        Debug.Log("Add spawn point");
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
