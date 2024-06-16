using System;
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


    protected List<(string, string)> statistics = new List<(string, string)>();


    protected void addStat(string name, string value)
    {
        statistics.Add(ValueTuple.Create<string, string>(name, value));
    }

    protected List<(string, string)> collectStat() 
    {
        var newStat = new List<(string, string)>(statistics);
        statistics.Clear();
        return newStat;
    }


    public virtual List<(string, string)> GetStatistics() { return statistics; }
}
