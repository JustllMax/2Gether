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
    private TMP_Text _title;
    [SerializeField]
    private TMP_Text _description;
    [SerializeField]
    List<Image> levelMarks;
    [SerializeField]
    private Image _thumbnail;
    [SerializeField]
    private Image _backgroundRaycastTarget;

    [SerializeField]
    Color whiteColor;
    [SerializeField]
    Color blueColor;
    [SerializeField]
    Color redColor;
    
    [SerializeField]
    private CardStatDisplay _statDisplay;

    [SerializeField]
    private Image _activeBorder;

    [SerializeField]
    private Image _typeIcon;
    [SerializeField]
    List<Sprite> symbolTypeIcons;
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
        _activeBorder.color = GetColorForRarity(card.CardStatisticsData.Rarity);
        SetTypeIcon(card.cardTypeSymbol);
        EnableLevelMarks(card.CardStatisticsData.Rarity);
        _statDisplay.Display(card.CardStatisticsData.GetStatistics());
    }

    private Color GetColorForRarity(Rarity r)
    {
        switch (r)
        {
            case Rarity.Generic:
                return whiteColor;
                break;
            case Rarity.Enhanced:
                return blueColor;
                break;
            case Rarity.Prototype:
                return redColor;
                break;
            default: return whiteColor;
        }
        
    }
    void SetTypeIcon(CardTypeSymbol type)
    {
        _typeIcon.sprite = symbolTypeIcons[(int)type];
    }

    void EnableLevelMarks(Rarity r)
    {
        int level = (int)r;
        //base level is 0
        for(int i = 0; i <= level; i++)
        {
            levelMarks[i].gameObject.SetActive(true);
            levelMarks[i].color = _activeBorder.color;
        }
    }

    public Image GetRaycastableBackground()
    {
        return _backgroundRaycastTarget;
    }

    public void SetRaycastable(bool value)
    {
        _backgroundRaycastTarget.raycastTarget = value;
    }
}
