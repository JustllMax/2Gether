using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExplosionSpawner
{
    public static Explosion SpawnExplosion(Vector3 position)
    {
        GameObject prefab = Resources.Load<GameObject>("P_Explosion");

        if (prefab == null)
        {
            Debug.LogError($"Prefab 'P_Explosion' not found in Resources folder.");
            return null;
        }
        Explosion instance = Object.Instantiate(prefab, position, Quaternion.identity).GetComponent<Explosion>();
        return instance;
    }
}
