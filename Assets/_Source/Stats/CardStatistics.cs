using System.Collections;
using System.Collections.Generic;
using UnityEngine;



abstract public class CardStatistics : ScriptableObject
{

    [SerializeField]
    CardType CardType;
    public Rarity Rarity;
    public string Name;
    public string Description;

}
