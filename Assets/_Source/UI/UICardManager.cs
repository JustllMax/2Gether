using System;
using System.Collections.Generic;
using UnityEngine;

public class UICardManager : MonoBehaviour
{
    [SerializeField]
    CardPoolData cardPoolData;

    [Range(1,100)]
    [SerializeField] float chanceForGeneric;
    [Range(1, 100)]
    [SerializeField] float chanceForEnhanced;
    [Range(1, 100)]
    [SerializeField] float chanceForPrototype;
    
    private static UICardManager _instance;
    public static UICardManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public List<Card> GetRandomCards(int numberOfCards)
    {
        List<Card> cards = new List<Card>(5);

        List<Card> deck = cardPoolData.Cards;

        for(int i=0; i<numberOfCards; i++)
        {
            cards.Add(GetRandomizedCard(deck));
        }

        return cards;

    }

    public List<Card> GetRandomCards(CardPoolData cardPoolData)
    {
        List<Card> cards = new List<Card>(5);

        return cards;

    }

    Card GetRandomizedCard(List<Card> deck)
    {
        Card card = deck[0];

        return card;
    }
}