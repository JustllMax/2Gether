using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SingleWaveData
{
    [SerializeField] public WaveEnemyPool EnemyPool;
    [SerializeField] public float EnemySpawnInterval;
    [SerializeField] public float Cooldown;
}

[CreateAssetMenu(menuName = "Game/Wave/Wave Data")]
public class WaveData : ScriptableObject
{
    [SerializeField]
    public List<SingleWaveData> Waves = new List<SingleWaveData>();
}
