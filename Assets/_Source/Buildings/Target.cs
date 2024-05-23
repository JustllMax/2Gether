using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, ITargetable, IDamagable
{
    private float _health;
    public float Health { get => _health; set => _health = value; }

    public bool IsTargetable { get ; set ; }
    public TargetType TargetType { get; set; }

    private void Awake()
    {
        Health = 25;
        IsTargetable = true;
        TargetType = TargetType.Building;
    }

    public bool TakeDamage(float damage)
    {
        Debug.Log("Took damage! " + damage);
        Health -= damage;
        if (Health <= 0)
        {
            Kill();
            return true;
        }
        return false;
    }

    public void Kill()
    {
        Debug.Log("I died!");
        IsTargetable = false;
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
