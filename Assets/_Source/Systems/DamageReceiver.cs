using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct DamageTypeInfo
{
    public DamageType type;
    public float percentage;
}

public class DamageReceiver : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth = 100f;

    [SerializeField]
    private float _minHealth = 0f;

    [SerializeField]
    private float _damageCooldownSeconds = 0.25f;
    private float _damageCooldownTime;

    [SerializeField]
    private float _health = 100f;

    public delegate void HealthChangedEventHandler(GameObject affectedObject, float previousHealth, float newHealth);
    public event HealthChangedEventHandler onHealthChanged;
    public event HealthChangedEventHandler onDamage;
    public event Action<GameObject> onDeath;

    public float Health
    {
        get
        {
            return _health;
        }
        set
        {
            float previousHealth = _health;
            _health = value;
            if (_health < _minHealth)
            {
                _health = _minHealth;
            } else if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }

            onHealthChanged?.Invoke(gameObject, previousHealth, _health);

            if (_health <= 0f)
            {
                Die();
            }
        }
    }

    public DamageTypeInfo[] resistances;
    public DamageTypeInfo[] weaknesses;

    public void ApplyDamage(Damage damage)
    {
        if (Time.time < _damageCooldownTime)
        {
            return;
        }

        _damageCooldownTime = Time.time + _damageCooldownSeconds;

        float previousHealth = _health;
        Health -= ApplyModifiers(damage);
        onDamage?.Invoke(gameObject, previousHealth, _health);
        Debug.Log(_health);
    }

    private float ApplyModifiers(Damage damage)
    {
        foreach (DamageTypeInfo resistance in resistances)
        {
            if (resistance.type == damage.type)
            {
                return damage.value * (1 - resistance.percentage);
            }
        }
        foreach (DamageTypeInfo weakness in weaknesses)
        {
            if (weakness.type == damage.type)
            {
                return damage.value * (1 - weakness.percentage);
            }
        }
        return damage.value;
    }

    private void Die()
    {
        onDeath?.Invoke(gameObject);
    }
}