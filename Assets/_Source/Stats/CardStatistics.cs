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

    public virtual Dictionary<string, string> GetStatistics() { return new Dictionary<string, string>(); }
}
