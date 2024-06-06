using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private static WaveManager _instance;
    public static WaveManager Instance { get { return _instance; } }
    [SerializeField] int waveMaxCount;
    private WaveSystem _waveSystem;
    public WaveSystem WaveSystem { get { return _waveSystem; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
        _waveSystem = GetComponent<WaveSystem>();
    }
    void OnEnable()
    {
        GameManager.OnGameManagerReady += OnStart;
    }
    void OnDisable()
    {
        GameManager.OnGameManagerReady -= OnStart;
    }
    void FixedUpdate()
    {
        if(_waveSystem.enemyCount <= 0)
        {
            EndWave();
        }

        if(DayNightCycleManager.Instance.nightBeginTasks <= 0)
        {
            DayNightCycleManager.Instance.nightBeginTasks = 3;
            Debug.LogWarning("Wave start " + DayNightCycleManager.Instance.nightBeginTasks);
            WaveStart();
        }
    }

    public void EndWave()
    {
        DayNightCycleManager.Instance.EndNightCycle();
    }

    private void OnStart()
    {
        if (SlotPlacer.Instance == null)
        {
            return;
        }
        foreach (var spawn in SlotPlacer.Instance.spawnSlots)
        {
            if (spawn == null)
            {
                continue;
            }
            _waveSystem.AddSpawnPoint(spawn.gameObject);
        }
    }
    void WaveStart()
    {
        NextWaveStart();
    }
    public void NextWaveStart()
    {
        _waveSystem.BeginWave(Resources.Load<WaveData>("Waves/wave_" + ((WaveSystem.nightIndex < waveMaxCount) ? WaveSystem.nightIndex : 0)));
    }
}
