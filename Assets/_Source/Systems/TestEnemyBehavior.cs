using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyBehavior : Enemy
{
    public override float TakeDamage(Damage damage)
    {
        float tmp = base.TakeDamage(damage);
        Debug.Log("Enemy took " + tmp + " damage. Now: " + _health);
        return tmp;
    }

    public override void Kill()
    {
        base.Kill();
        Debug.Log("Enemy died");
    }
}
