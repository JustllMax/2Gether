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

    private void Awake()
    {
        GameManager.OnGameManagerReady += OnStart;
    }

    private void OnStart()
    {
        _spawnPoints = new List<GameObject>();
        foreach (var spawn in SlotPlacer.Instance.spawnSlots)
        {
            Debug.Log(spawn.GetInstanceID() + "" + spawn.gameObject.name);

            _spawnPoints.Add(spawn.gameObject);
        }
        
        Debug.Log("Count " + _spawnPoints.Count);
        foreach (var spawn in _spawnPoints)
        {
            Debug.Log(spawn.GetInstanceID() + "" + spawn.gameObject.name);
        }
        BeginWave(Resources.Load<WaveData>("wave_test"));
    }
    
    public GameObject GetRandomSpawnPoint()
    {
        return _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)];
    }

    public void BeginWave(WaveData waveData)
    {
        if (_isWaveActive)
            return;

        _isWaveActive =true;
        _waveData = waveData;

        _ = handleWave(_waveData);
    }

    private async UniTaskVoid handleWave(WaveData data)
    {
        foreach (var wave in data.Waves)
        {
            for (int i = 0; i < wave.EnemyPool.EnemyCount; i++)
            {
                Instantiate(wave.EnemyPool.GetNextEnemy(i), GetRandomSpawnPoint().transform.position + Vector3.up, Quaternion.identity);
                await UniTask.WaitForSeconds(wave.EnemySpawnInterval);
            }
     
            await UniTask.WaitForSeconds(wave.Cooldown);
        }
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
        _spawnPoints.Add(spawnPoint);
    }

}
