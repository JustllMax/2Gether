using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
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
    public Damage(DamageType type, float value)
    {
        this.type = type;
        this.value = value;
    }
}