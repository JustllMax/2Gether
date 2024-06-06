using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes;

public class UIFlow : MonoBehaviour
{
    public Button boosterPackButton;
    public Button rerollButton;
    public Button continueButton;
    public GameObject cardPrefab;
    public GameObject cardpackPanel;
    public GameObject cardpackOpenPanel;
    public GameObject gamePanel;
    public GameObject cardsPanel;

    [SerializeField]
    private UICards currentClickedCard = null;

    [SerializeField, ReadOnly]
    private CardPoolData _currentCardPoolData;

    void Awake()
    {
        gamePanel.SetActive(false);
        cardpackPanel.SetActive(false);
        rerollButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }

    public void ShowPanel(CardPoolData pd)
    {
        _currentCardPoolData = pd;

        gamePanel.SetActive(false);
        cardpackPanel.SetActive(true);
        rerollButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        boosterPackButton.onClick.AddListener(OpenBoosterPack);
        rerollButton.onClick.AddListener(Reroll);
        continueButton.onClick.AddListener(Continue);
    }

    void OpenBoosterPack()
    {
        boosterPackButton.gameObject.SetActive(false);

        SpawnCards();
        FadeInCards();

        rerollButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(true);
        rerollButton.transform.DOScale(0, 0);
        continueButton.transform.DOScale(0, 0);
        rerollButton.transform.DOScale(1, 1).SetDelay(1f);
        continueButton.transform.DOScale(1, 1).SetDelay(3f);
    }

    void Reroll()
    {
        FadeOutCards();
        rerollButton.interactable = false;
        continueButton.interactable = false;
        UnityEngine.UI.Image rerollButtonImage = rerollButton.GetComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Image continueButtonImage = continueButton.GetComponent<UnityEngine.UI.Image>();
        rerollButtonImage.DOFade(0f, 1f);
        continueButtonImage.DOFade(0f, 1f);

        DOTween.Sequence()
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                DestroyCards();
                SpawnCards();
                FadeInCards();
            });

        DOTween.Sequence()
            .AppendInterval(3f)
            .OnComplete(() =>
            {
                Continue();
            });
    }

    void Continue()
    {
        DOTween.Sequence()
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                //FadeOutCards();
                rerollButton.interactable = false;
                continueButton.interactable = false;
                UnityEngine.UI.Image rerollButtonImage = rerollButton.GetComponent<UnityEngine.UI.Image>();
                UnityEngine.UI.Image continueButtonImage = continueButton.GetComponent<UnityEngine.UI.Image>();
                rerollButtonImage.DOFade(0f, 1f);
                continueButtonImage.DOFade(0f, 1f);
            });

        DOTween.Sequence()
            .AppendInterval(3f)
            .OnComplete(() =>
            {
                UICards[] cards = cardpackOpenPanel.GetComponentsInChildren<UICards>();

                foreach (var card in cards)
                {
                    if (card.transform != cardpackOpenPanel.transform)
                    {
                        card.transform.SetParent(cardsPanel.transform, false);
                        card.SetUIFlowRef(this);
                    }
                }
                cardpackPanel.SetActive(false);

                foreach (var card in cards)
                {
                    if (card.transform != cardsPanel.transform)
                    {
                        //UnityEngine.UI.Image cardImage = card.GetComponent<UnityEngine.UI.Image>();
                        //cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0f);
                        //cardImage.DOFade(1f, 1f).SetDelay(0.5f);
                    }
                }
            });

        DOTween.Sequence()
            .AppendInterval(3f)
            .OnComplete(() =>
            {
                gamePanel.SetActive(true);
            });

        
    }

    void SpawnCards()
    {
        foreach (var card in _currentCardPoolData.Cards)
        {
            GameObject gocard = Instantiate(cardPrefab, cardpackOpenPanel.transform);
            gocard.GetComponent<CardObject>().PopulateCard(card);
            gocard.name = "Card " + card.CardName;
        }

        HorizontalLayoutGroup layoutGroup = cardpackOpenPanel.GetComponent<HorizontalLayoutGroup>();
        if (layoutGroup != null)
        {
            layoutGroup.enabled = true;
        }
    }

    void DestroyCards()
    {
        Transform[] cards = cardpackOpenPanel.GetComponentsInChildren<Transform>();

        for (int i = 1; i < cards.Length; i++)
        {
            Destroy(cards[i].gameObject);
        }
    }

    void FadeInCards()
    {
        Transform[] cards = cardpackOpenPanel.GetComponentsInChildren<Transform>();

        foreach (Transform card in cards)
        {
            if (card != cardpackOpenPanel.transform)
            {
                //UnityEngine.UI.Image cardImage = card.GetComponent<UnityEngine.UI.Image>();
                //cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0f);
                //cardImage.DOFade(1f, 1f).SetDelay(0.5f);
            }
        }
    }

    void FadeOutCards()
    {
        Transform[] cards = cardpackOpenPanel.GetComponentsInChildren<Transform>();

        foreach (Transform card in cards)
        {
            if (card != cardpackOpenPanel.transform)
            {
                UnityEngine.UI.Image cardImage = card.GetComponent<UnityEngine.UI.Image>();
                cardImage.DOFade(0f, 1f).SetDelay(0.5f);
            }
        }
    }

    public void SetSelectedCard(UICards card)
    {
        if (currentClickedCard == card) return;

        if (currentClickedCard != null)
        {
            currentClickedCard.ResetPosition();
        }

        currentClickedCard = card;


    }
}
