using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] GameObject model;
    Minefield minefield;


    private void Awake()
    {
        minefield = GetComponentInParent<Minefield>();
    }

    private void Update()
    {
      
    }

    void OnTriggerEnter(Collider other)
    {
        AIController temp = other.GetComponentInParent<AIController>();
        if (temp != null)
        {
            OnAttack();
        }
    }

    void OnAttack()
    {
        ExplosionSpawner.SpawnExplosion(transform.position).SetUpExplosion(GetStatistics().AttackDamage, GetStatistics().AttackRange, minefield.GetTargetLayerMask());
        Kill();
    }

    void Kill()
    {
        gameObject.SetActive(false);
        minefield.OnAttack();
    }


    BuildingOffensiveStatistics GetStatistics()
    {
        return minefield.GetStatistics();
    }

    public void SetMineUp()
    {
        gameObject.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
