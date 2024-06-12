using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private Card _card;

    //ui stuff
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _description;

    [SerializeField]
    private Image _thumbnail;

    public void Execute()
    {
        GameContext ctx = new GameContext();
        ctx.buildingPlacer = BuildingPlacer.Instance;
        ctx.cardUi = UIFlow.Instance;
        _card.OnBeginUseCard(ctx);
    }

    public void EndExecute()
    {
        GameContext ctx = new GameContext();
        ctx.buildingPlacer = BuildingPlacer.Instance;
        _card.OnEndUseCard(ctx);
    }

    public void PopulateCard(Card card)
    {
        _card = card;

        _title.text = card.CardName;
        _description.text = card.CardDescription;
        _thumbnail.sprite = card.CardSprite;
    }
}
