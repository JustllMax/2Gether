using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void Heal(float value);
    public float ApplyDamage(Damage damage);
    public void Kill();
    public float Health { get; set; }
    public float DamageCooldownTime { get; set; }
}
