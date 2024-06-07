using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SingleWaveData
{
    [SerializeField] public List<GameObject> Enemies;
    [SerializeField] public float EnemySpawnInterval;
    [SerializeField] public float Cooldown;

    public SingleWaveData(List<GameObject> enemies)
    {
        Enemies = new List<GameObject>(enemies);
        EnemySpawnInterval = 0;
        Cooldown = 0;
    }

}

[CreateAssetMenu(menuName = "Game/Wave/Wave Data")]
public class WaveData : ScriptableObject
{
    [SerializeField]
    public List<SingleWaveData> Waves = new List<SingleWaveData>();

    public void AddWave(SingleWaveData singleWaveData)
    {
        Waves.Add(singleWaveData);
    }

    public void RemoveWave(int index)
    {
        Waves.RemoveAt(index);
    }

    public void AddEnemyToWave(GameObject enemy, int index)
    {
        Waves[index].Enemies.Add(enemy);
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public static ScriptableObject DeepCopy(ScriptableObject source)
    {
        ScriptableObject copy = Instantiate(source);
        return copy;
    }

}
