using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyBehavior : Entity
{
    public override float ApplyDamage(Damage damage)
    {
        float tmp = base.ApplyDamage(damage);
        Debug.Log("Enemy took " + tmp + " damage. Now: " + _health);
        return tmp;
    }

    public override void Kill()
    {
        base.Kill();
        Debug.Log("Enemy died");
    }
}
