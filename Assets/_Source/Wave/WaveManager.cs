using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public struct SingleWave
{
    public List<GameObject> EnemyPool;
    public float EnemySpawnInterval;
    public float Cooldown;

    public SingleWave(List<GameObject> EnemyPool, float EnemySpawnInterval, float Cooldown)
    {
        this.EnemyPool = EnemyPool;
        this.EnemySpawnInterval = EnemySpawnInterval;
        this.Cooldown = Cooldown;
    }
}

public class WaveManager : MonoBehaviour
{
    #region variables
    private static WaveManager _instance;
    public static WaveManager Instance { get { return _instance; } }
    [SerializeField] float nightEndDelay = 2f;
    [SerializeField] List<GameObject> _enemyFollowPrefabs;
    [SerializeField] List<GameObject> _enemyAttackPrefabs;
    [SerializeField] List<GameObject> _enemyBossPrefabs;
    List<SingleWave> waves = new List<SingleWave>();
    private WaveSystem _waveSystem;
    public WaveSystem waveSystem { get { return _waveSystem; } }
    private float waveClearExpected = 240;
    private float waveClear = 0;

    #endregion

    #region On start 
    void Awake()
    {
        Debug.LogWarning("Wave awake");
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
        _waveSystem = GetComponent<WaveSystem>();
        WaveSystem.nightCount = 0;
        WaveData waveData = Resources.Load<WaveData>("Waves/wave_data");

        foreach (var wave in waveData.Waves)
        {
            waves.Add(new SingleWave(new List<GameObject>(wave.Enemies), wave.EnemySpawnInterval, wave.Cooldown));
            waveClearExpected += wave.Cooldown;
        }
    }
    void OnEnable()
    {
        DayNightCycleManager.NightEnd += EndNightCycle;
        DayNightCycleManager.NightBegin += NightBeginCycle;
        GameManager.OnGameManagerReady += OnStart;
    }
    void OnDisable()
    {
        DayNightCycleManager.NightEnd -= EndNightCycle;
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

        //TEMP, bo co chwile wywala
        //return;

        if (_waveSystem.enemyCount <= 0 && _waveSystem.isWaveActive && !_waveSystem.isSpawnActive)
        {
            _ = InvokeNightEnd();
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
            foreach (var wave in waves)
            {
                //wave.Cooldown += followEnemiesNumberToAdd+attackEnemiesNumberToAdd;

                waveClearExpected += wave.Cooldown;
                for (int followCount = 0; followCount < followEnemiesNumberToAdd; followCount++)
                {
                    followToAdd = UnityEngine.Random.Range(0, _enemyFollowPrefabs.Count);
                    wave.EnemyPool.Add(_enemyFollowPrefabs[followToAdd]);
                }

                for (int followCount = 0; followCount < attackEnemiesNumberToAdd; followCount++)
                {
                    attackToAdd = UnityEngine.Random.Range(0, _enemyAttackPrefabs.Count);
                    wave.EnemyPool.Add(_enemyAttackPrefabs[attackToAdd]);
                }
            }
            waveClearExpected += followEnemiesNumberToAdd + attackEnemiesNumberToAdd;
        }
        if (WaveSystem.nightCount % 5 == 0)
        {
            bossToAdd = UnityEngine.Random.Range(0, _enemyBossPrefabs.Count);
            waves[waves.Count - 1].EnemyPool.Add(_enemyBossPrefabs[bossToAdd]);
        }
        waveClearExpected += followEnemiesNumberToAdd + attackEnemiesNumberToAdd;
    }
    #endregion

    #region Wave start/End

    private void NightBeginCycle()
    {
        StartWave();
    }
    public void StartWave()
    {
        _waveSystem.BeginWave(waves);
    }

    public void EndNightCycle()
    {
        _waveSystem.isWaveActive = false;
        _ = DeleteEnemies();
        _waveSystem.enemyCount = 0;

        if (WaveSystem.nightCount > 0)
            NextNightWaveData();
    }

    public async UniTaskVoid DeleteEnemies()
    {
        await UniTask.WaitForSeconds(0.5f);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
    }
    #endregion
    #endregion

    async UniTaskVoid InvokeNightEnd()
    {
        HUDManager.Instance.ShowEndNightText();
        await UniTask.WaitForSeconds(nightEndDelay);
        DayNightCycleManager.Instance.EndNightCycle();
    }
}
