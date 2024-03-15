using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : MonoBehaviour
{

    public abstract void OnCreate();
    public abstract void OnAttack();
    
    public abstract void OnTakeDamage();
}
