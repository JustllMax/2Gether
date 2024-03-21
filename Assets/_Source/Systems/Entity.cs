using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamagable
{
    [SerializeField]
    protected HealthModifier _healthModifier;

    [SerializeField]
    protected float _health;
    public virtual float Health
    {
        get
        {
            return _health;
        }
        set
        {
            float previousHealth = _health;
            _health = value;
            if (_health > _healthModifier._maxHealth)
            {
                _health = _healthModifier._maxHealth;
            }

            if (_health <= 0f)
            {
                Kill();
            }
        }
    }

    protected float _damageCooldownTime;
    public float DamageCooldownTime { get => _damageCooldownTime; set => _damageCooldownTime = value; }

    public virtual float ApplyDamage(Damage damage)
    {
        return _healthModifier.ApplyDamage(this, damage);
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    public virtual void Heal(float value)
    {
        Health += value;
    }

    public virtual void Start()
    {
        _health = _healthModifier._maxHealth;
    }
}
