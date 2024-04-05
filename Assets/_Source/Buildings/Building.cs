using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{

    protected float AttackCoolDownTimer = 5f;

    public virtual void AttackCooldown()
    {
        AttackCoolDownTimer -= Time.deltaTime;
    }

    public abstract void OnCreate();
    public abstract void OnAttack();
    
    public abstract void OnTakeDamage();

    public abstract void OnUpgrade();
    public abstract void OnSell();
}
