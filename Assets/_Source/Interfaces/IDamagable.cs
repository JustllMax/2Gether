using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public float TakeDamage(float damage);
    public void Kill();
    public float Health { get; set; }
}
