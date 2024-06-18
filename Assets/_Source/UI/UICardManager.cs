using System;
using System.Collections.Generic;
using UnityEngine;

using Random=UnityEngine.Random;

public class UICardManager : MonoBehaviour
{
    [SerializeField]
    CardPoolData cardPoolData;

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
        List<Card> cards = new List<Card>(numberOfCards);

        List<Card> deck = cardPoolData.Cards;

        for(int i=0; i<numberOfCards; i++)
        {
            cards.Add(GetRandomizedCard(deck));
        }

        return cards;

    }

    public List<Card> GetRandomCards(CardPoolData cardPoolData, int numberOfCards)
    {
        List<Card> cards = new List<Card>(numberOfCards);

        List<Card> deck = cardPoolData.Cards;

        for (int i = 0; i < numberOfCards; i++)
        {
            cards.Add(GetRandomizedCard(deck));
        }

        return cards;
        
    }

    Card GetRandomizedCard(List<Card> deck)
    {
        Card card;
        int rarityChance = Random.Range(1, 100);
        Rarity rarity = Rarity.Generic;
        if(rarityChance <= chanceForPrototype)
        {
            rarity = Rarity.Prototype;
        }
        else if(rarityChance <= chanceForEnhanced)
        {
            rarity = Rarity.Enhanced;
        }

        int randomCardIndex = Random.Range(0, deck.Count-1);
        card = deck[randomCardIndex];

        int a = 0;
        while(card.CardStatisticsData.Rarity != rarity)
        {
            a++;
            randomCardIndex = Random.Range(0, deck.Count - 1);
            card = deck[randomCardIndex];
            if (a > 1000)
            {
                // we cant find rarity we wanted, so just give up
                card = deck[randomCardIndex];
                break;
            }
        }

        return card;
    }
}