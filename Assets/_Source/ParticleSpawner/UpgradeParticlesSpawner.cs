using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpgradeParticlesSpawner
{
    public static UpgradeParticles SpawnParticles(Vector3 position, float scaleModifier =1f)
    {
        GameObject prefab = Resources.Load<GameObject>("P_UpgradeParticles");

        if (prefab == null)
        {
            Debug.LogError($"Prefab 'P_UpgradeParticles' not found in Resources folder.");
            return null;
        }
        UpgradeParticles instance = Object.Instantiate(prefab, position, Quaternion.identity).GetComponent<UpgradeParticles>();
        instance.SetUpParticles(scaleModifier);
        return instance;
    }
}
