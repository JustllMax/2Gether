using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

[System.Serializable]
public struct EnemyWeight
{
    public GameObject enemy;
    public int weight;
}

[System.Serializable]
public class EnemyWeightPool //: UnityEngine.Object
{
    public EnemyWeight[] enemies;
    int totalWeight;

    public void Init()
    {
        totalWeight = 0;
        foreach (EnemyWeight enemy in enemies)
            totalWeight += enemy.weight;
    }

    public GameObject GetRandomEnemy()
    {
        int weight = UnityEngine.Random.Range(0, totalWeight);
        int iterWeight = 0;

        foreach (EnemyWeight enemy in enemies)
        {
            iterWeight += enemy.weight;
            if (iterWeight >= weight)
                return enemy.enemy;
        }
        return null;
    }

}


[System.Serializable]
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
    [SerializeField] EnemyWeightPool _enemyFollow;
    [SerializeField] EnemyWeightPool _enemyAttack;
    [SerializeField] EnemyWeightPool _enemyBoss;
    [SerializeField] List<SingleWave> waves = new List<SingleWave>();
    private WaveSystem _waveSystem;
    public WaveSystem waveSystem { get { return _waveSystem; } }
    private float waveClearExpected = 240;
    private float waveClear = 0;

    private int _enemyLeftForNight = 0;
    public int enemyLeftForNight { get{ return _enemyLeftForNight;}}
    //[SerializeField] int[] _weightsFollow;
    //[SerializeField] int[] _weightsAttack;
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

        _enemyFollow.Init();
        _enemyAttack.Init();
        _enemyBoss.Init();

        //_weightsFollow = SplitNumber(_enemyFollowPrefabs.Count*30, _enemyFollowPrefabs.Count);
        //_weightsAttack = SplitNumber(_enemyAttackPrefabs.Count*30, _enemyAttackPrefabs.Count);

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


    private void LateUpdate()
    {

        if (_waveSystem.enemyCount <= 0 && _waveSystem.isWaveActive && !_waveSystem.isSpawnActive)
        {
            _waveSystem.isWaveActive = false;
            _ = InvokeNightEnd();
        }
        waveClear = _waveSystem.elapsedTime;
    }

    #region Wave 

    #region Next Wave

    //int[] SplitNumber(int total, int count)
    //{
    //    int[] result = new int[count];
    //    float[] weights = new float[count];
    //
    //    float weightSum = 0;
    //    for (int i = 0; i < count; i++)
    //    {
    //        weights[i] = 1.0f / (i + 1);
    //        weightSum += weights[i];
    //    }
    //
    //    float totalWeight = 0;
    //    for (int i = 0; i < count; i++)
    //    {
    //        weights[i] = weights[i] / weightSum * total;
    //        totalWeight += weights[i];
    //    }
    //
    //    int intTotalWeight = Mathf.RoundToInt(totalWeight);
    //    int sum = 0;
    //    for (int i = 0; i < count; i++)
    //    {
    //        result[i] = Mathf.RoundToInt(weights[i]);
    //        sum += result[i];
    //    }
    //
    //    result[count - 1] += total - sum;
    //
    //    return result;
    //}
    public void NextNightWaveData()
    {
        int followEnemiesNumberToAdd = UnityEngine.Random.Range(1, 5);
        int attackEnemiesNumberToAdd = UnityEngine.Random.Range(1, 4);

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
                    wave.EnemyPool.Add(_enemyFollow.GetRandomEnemy());
                }

                for (int followCount = 0; followCount < attackEnemiesNumberToAdd; followCount++)
                {
                    wave.EnemyPool.Add(_enemyAttack.GetRandomEnemy());
                }
            }
            waveClearExpected += followEnemiesNumberToAdd + attackEnemiesNumberToAdd;
        }
        if (WaveSystem.nightCount % 5 == 0)
        {
            waves[waves.Count - 1].EnemyPool.Add(_enemyBoss.GetRandomEnemy());
        }
        waveClearExpected += followEnemiesNumberToAdd + attackEnemiesNumberToAdd;
    }

    int DetermineEnemyIndex(int randomNumber, int[] elements)
    {
        int cumulativeSum = 0;
        for (int i = 0; i < elements.Length; i++)
        {
            cumulativeSum += elements[i];
            if (randomNumber >= (100 - cumulativeSum))
            {
                return i;
            }
        }

        // На случай если что-то пойдет не так, возвращаем последний тип врага
        return elements.Length - 1;
    }
    #endregion

    #region Wave start/End

    private void NightBeginCycle()
    {
        _enemyLeftForNight = _waveSystem.GetEnemyForNightCount(waves);
        StartWave();
    }
    public void StartWave()
    {
        _waveSystem.BeginWave(waves);
    }

    public void EndNightCycle()
    {
        _waveSystem.isWaveActive = false;
        //DeleteEnemies();
        //_waveSystem.enemyCount = 0;

        if (WaveSystem.nightCount > 0)
            NextNightWaveData();
    }

    public void DeleteEnemies()
    {
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

    public void EnemyHasBeenKilled()
    {
        _enemyLeftForNight--;
        waveSystem.enemyCount--;
        Debug.LogErrorFormat("Enemy count: " + _waveSystem.enemyCount);
        HUDManager.Instance.SetEnemyCounter(_enemyLeftForNight);
    }

    async UniTaskVoid InvokeNightEnd()
    {
        HUDManager.Instance.ShowEndNightText();
        await UniTask.WaitForSeconds(nightEndDelay);
        DayNightCycleManager.Instance.EndNightCycle();
    }
}
