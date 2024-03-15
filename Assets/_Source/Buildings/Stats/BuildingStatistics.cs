using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BuildingStatistics : ScriptableObject
{
    public float HealthPoints;
    public float AttackRange;
    public float AttackSpeed;
    public float AttackDamage;
    public float SellCost;
    public int Level;
    public Rarity Rarity;
    public string Name;
}
