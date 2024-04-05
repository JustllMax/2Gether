using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardHolderManager : MonoBehaviour
{
    [Header("Card Holder Parameters")]
    [SerializeField] private Transform _cardHolderPosition;
    [SerializeField] private GameObject _card;
    [SerializeField] private GridCard[] _cardSO;
    private int _cardsAmount;

    [Header("Card Parameters")]
    [SerializeField] private GameObject[] _plantedCards;
    private int _cost;
    private Sprite _icon;
    // Start is called before the first frame update
    void Start()
    {
        _cardsAmount = _cardSO.Length;
        _plantedCards = new GameObject[_cardsAmount];

        for(int i = 0; i < _cardsAmount; i++)
            CreateCard(i);
    }

    private void CreateCard(int i)
    {
        var card = Instantiate(_card, _cardHolderPosition);

        BuildCardManager cardManager = card.GetComponent<BuildCardManager>();

        cardManager.CardSO = _cardSO[i];

        _plantedCards[i] = card;

        _icon = _cardSO[i].icon;
        _cost = _cardSO[i].cost;

        card.GetComponentInChildren<SpriteRenderer>().sprite = _icon;
        card.GetComponentInChildren<TMP_Text>().text = _cost.ToString();
    }
}
