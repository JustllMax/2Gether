using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType : byte
{
    Explosion,
    Fire,
    Energy,
    Kinetic,
    Water,
    Slash,
    Blunt
}

[System.Serializable]
public struct Damage
{
    public DamageType type;
    public float value;
    public Vector3 knockback;
    public Damage(DamageType type, float value, Vector3 knockback)
    {
        this.type = type;
        this.value = value;
        this.knockback = knockback;
    }

    public Damage(float value)
    {
        this.type = DamageType.Kinetic;
        this.value = value;
        this.knockback = Vector3.zero;
    }
}