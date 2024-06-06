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
        GameManager.OnNextWaveStart += OnNextWaveStart;
        WaveSystem.OnEndWave += OnWaveEnd;
    }
    void OnDisable()
    {
        GameManager.OnGameManagerReady -= OnStart;
        GameManager.OnNextWaveStart -= OnNextWaveStart;
        WaveSystem.OnEndWave -= OnWaveEnd;
    }

    private void OnWaveEnd()
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

        //_waveSystem.BeginWave(Resources.Load<WaveData>("Waves/wave_test"));
    }

    private void OnNextWaveStart()
    {
        //DayNightCycleManager.Instance.EndDayCycle();

        _waveSystem.BeginWave(Resources.Load<WaveData>("Waves/wave_" + ((WaveSystem.nightIndex < waveMaxCount) ? WaveSystem.nightIndex : 0)));
    }
}
