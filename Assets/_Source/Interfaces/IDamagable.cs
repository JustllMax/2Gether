using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public bool TakeDamage(float damage);
    public bool Heal(float amount);
    public void Kill();
    public float Health { get; set; }
}
