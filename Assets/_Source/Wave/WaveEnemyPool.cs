using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WaveEnemyPool : ScriptableObject
{
    public List<GameObject> Enemies;
    public int EnemyCount;

    public abstract GameObject GetNextEnemy(int index);
}
