using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackVisualization : MonoBehaviour
{
    static GameObject missedAttackPrefab;
    static GameObject successfulAttackPrefab;

    public static void DrawAttack(Vector3 pos, float radius, bool successful)
    {
        if (missedAttackPrefab == null)
        {
            missedAttackPrefab = Resources.Load<GameObject>("AttackVisualization1");
            successfulAttackPrefab = Resources.Load<GameObject>("AttackVisualization2");
        }

        GameObject attack = Instantiate(successful ? successfulAttackPrefab : missedAttackPrefab, pos, Quaternion.identity);
        attack.transform.localScale = new Vector3(radius, radius, radius);
    }

    private void Start()
    {
        Invoke("DestroyObj", 0.25f);
    }

    private void DestroyObj()
    {
        Destroy(gameObject);
    }
}

