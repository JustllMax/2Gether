using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public float TakeDamage(Damage damage);
    public void Kill();
    public float Health { get; set; }
}
