using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SingleWaveData
{
    public WaveEnemyPool EnemyPool;
    public float EnemySpawnInterval;
    public float Cooldown;
}

[CreateAssetMenu(menuName = "Game/Wave/Wave Data")]
public class WaveData : ScriptableObject
{
    [SerializeField]
    public List<SingleWaveData> Waves = new List<SingleWaveData>();
}
