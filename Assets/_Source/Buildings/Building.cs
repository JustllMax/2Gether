using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{

    public abstract void OnCreate();
    public abstract void OnAttack();
    
    public abstract void OnTakeDamage();

    public abstract void OnUpgrade();
    public abstract void OnSell();
}
