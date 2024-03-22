using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Card : ScriptableObject
{
    public string CardName;

    public Sprite CardSprite;
    public string CardDescription;

    public CardStatistics CardStatisticsData;

    public abstract void OnSubmitCard(GameContext ctx);
}
