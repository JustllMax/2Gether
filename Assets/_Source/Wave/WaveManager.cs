using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    #region variables
    private static WaveManager _instance;
    public static WaveManager Instance { get { return _instance; } }
    [SerializeField] int waveMaxCount;
    [SerializeField] List<GameObject> _enemyFollowPrefabs;
    [SerializeField] List<GameObject> _enemyAttackPrefabs;
    [SerializeField] List<GameObject> _enemyBossPrefabs;
    private WaveData _waveData;
    private WaveSystem _waveSystem;
    public WaveSystem WaveSystem { get { return _waveSystem; } }

    #region Wave Spawn hard system

    private List<List<int>> difEnemy;
    int sumDif = 0;
    private float waveClearExpected = 240;
    private float waveClear = 0;

    private static int waveClearAvr;
    private static int waveClearSum = 0;

    #endregion
    #endregion

    #region On start 
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
        _waveSystem = GetComponent<WaveSystem>();
        _waveData = (WaveData)Resources.Load<WaveData>("Waves/wave_" + ((WaveSystem.nightCount < waveMaxCount) ? WaveSystem.nightCount : 0)).Clone();

        foreach(var wave in _waveData.Waves)
        {
            waveClearExpected += wave.Cooldown;
        }
    }
    void OnEnable()
    {

        

        GameManager.OnGameManagerReady += OnStart;
    }
    void OnDisable()
    {
        GameManager.OnGameManagerReady -= OnStart;
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
    #endregion


    void FixedUpdate()
    {
        if (_waveSystem.enemyCount <= 0)
        {
            EndWave();
        }

        if (DayNightCycleManager.Instance.nightBeginTasks <= 0)
        {
            DayNightCycleManager.Instance.nightBeginTasks = 3;
            Debug.LogWarning("Wave start " + DayNightCycleManager.Instance.nightBeginTasks);
            StartWave();
        }
        waveClear = _waveSystem.elapsedTime;
    }

    #region Wave 

    #region Next Wave
    public void NextNightWaveData()
    {
        int followEnemiesNumberToAdd = UnityEngine.Random.Range(0, 5);
        int attackEnemiesNumberToAdd = UnityEngine.Random.Range(0, 5);
        int followToAdd;
        int attackToAdd;
        int bossToAdd;

        if (waveClear >= waveClearExpected)
        {
            
            return;
        }
        else
        {
            /*
            night
                waves - 1, 2, 3


            List<List<int>> difEnemyPrefab = 
            followPrefId +1  [1, 2, 3]  [1, 2, 3]
            attackPrefId +3  [3, 4, 5]  [3]
            bossPrefId   +5  [5, 6, 7]  [5]

            List<List<int>> difEnemy 
            [1, 1, 1]
            [1, 1, 1, 3]
            [1, 1, 3, 2]
            
            sumDif = sum(difEnemy)
            absDif = sumDif

            absDif += Round(waveClearAvg*nightCount*balanceCoefficient)

            while sumDif <= absDif
            {
                enemyIndex = UnityEngine.Random.Range(0, 100) < 30 ? 1:0;
                (waveIndex < 3) ?  waveIndex++:0
                if(absDis - sumDif > 10)
                {
                    if(enemyIndex == 0)
                    {
                        difEnemy[waveIndex].Add(followPrefab[Range(2, absDis - sumDif % followPrefab.Count)]);
                    }

                    if(enemyIndex == 1)
                    {
                        difEnemy[waveIndex].Add(attackPrefab[0, absDis - sumDif % followPrefab.Count]);
                    }
                }
                else
                {
                    difEnemy[0].Add(followPrefab[Range(2, absDis - sumDif % followPrefab.Count)]);
                }
                sumDif = sum(difEnemy)
            }


            
    private int SumDif()
    {
        int sumDif = 0;
        foreach (var enemies in difEnemy)
        {
            foreach (var enemy in enemies)
            {
                sumDif += enemy;
            }
        }
        return sumDif;
    }

    private void GetStartDif()
    {
        for (int wave = 0; wave < _waveData.Waves.Count; wave++)
        {
            difEnemy.Add(new List<int>());
            for (int f = 1; f < _enemyFollowPrefabs.Count; f++)
            {
                difEnemy[wave].Add(f);
            }
            for (int a = 0; a < _enemyAttackPrefabs.Count; a++)
            {
                difEnemy[wave].Add(a);
            }
        }
    }
            */

            foreach (var wave in _waveData.Waves)
            {
                //wave.Cooldown += followEnemiesNumberToAdd+attackEnemiesNumberToAdd;

                waveClearExpected += wave.Cooldown;
                for (int followCount = 0; followCount < followEnemiesNumberToAdd; followCount++)
                {
                    followToAdd = UnityEngine.Random.Range(0, _enemyFollowPrefabs.Count);
                    wave.EnemyPool.Enemies.Add(_enemyFollowPrefabs[followToAdd]);
                }

                for (int followCount = 0; followCount < attackEnemiesNumberToAdd; followCount++)
                {
                    attackToAdd = UnityEngine.Random.Range(0, _enemyAttackPrefabs.Count);
                    wave.EnemyPool.Enemies.Add(_enemyAttackPrefabs[attackToAdd]);
                }
            }
            waveClearExpected += followEnemiesNumberToAdd + attackEnemiesNumberToAdd;
        }

        if (WaveSystem.nightCount % 5 == 0)
        {
            bossToAdd = UnityEngine.Random.Range(0, _enemyBossPrefabs.Count);
            _waveData.Waves[_waveData.Waves.Count - 1].EnemyPool.Enemies.Add(_enemyBossPrefabs[bossToAdd]);
        }
    }
    #endregion
    public void EndWave()
    {
        DayNightCycleManager.Instance.EndNightCycle();
        NextNightWaveData();
    }

    void StartWave()
    {
        _waveSystem.BeginWave(_waveData);
    }
    public void GetNextWaveData()
    {

    }

    #endregion


}
