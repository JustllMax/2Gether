using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WaveSequentialEnemyPool : WaveEnemyPool
{
    public override GameObject GetNextEnemy(int index)
    {
        return Enemies[index];
    }

    private void OnValidate()
    {
        EnemyCount = Enemies.Count;
    }
}
