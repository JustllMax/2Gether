using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract public class Card : ScriptableObject
{
    public CardType Type { get; protected set; }

    public string CardName;

    public Sprite CardSprite;
    public string CardDescription;

    public CardStatistics CardStatisticsData;

    public abstract void OnCardSubmitted(GameContext ctx);

    public abstract void OnBeginUseCard(GameContext ctx);
    public abstract void OnEndUseCard(GameContext ctx);

}
