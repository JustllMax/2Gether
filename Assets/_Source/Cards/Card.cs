using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


abstract public class Card : ScriptableObject
{
    public CardType Type { get; protected set; }
    public CardTypeSymbol cardTypeSymbol;
    public string CardName;

    public Sprite CardSprite;
    public string CardDescription;

    public CardStatistics CardStatisticsData;
    protected List<(string, string)> statistics = new List<(string, string)>();
    public abstract void OnCardSubmitted(GameContext ctx);

    public abstract void OnBeginUseCard(GameContext ctx);
    public abstract void OnEndUseCard(GameContext ctx);


}
