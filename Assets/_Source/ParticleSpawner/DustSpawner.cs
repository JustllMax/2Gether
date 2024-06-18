using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DustSpawner
{
    public static Dust SpawnDust(Vector3 position, float scaleModifier=1f)
    {
        GameObject prefab = Resources.Load<GameObject>("P_DustParticles");

        if (prefab == null)
        {
            Debug.LogError($"Prefab 'P_DustParticles' not found in Resources folder.");
            return null;
        }
        Dust instance = Object.Instantiate(prefab, position, Quaternion.identity).GetComponent<Dust>();
        instance.SetUpParticles(scaleModifier);
        return instance;
    }
}
