using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public bool TakeDamage(float damage);
    public bool Heal(float amount);
    public void Kill(bool desintegrate);
    public float Health { get; set; }
}
