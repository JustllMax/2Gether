using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes;
using System;
using UnityEngine.PlayerLoop;

public class UIFlow : MonoBehaviour
{
    public static UIFlow Instance;

    public Button boosterPackButton;
    public Button rerollButton;
    public Button continueButton;

    [SerializeField]
    private Button _startNightButton;

    public GameObject cardPrefab;
    public GameObject cardpackPanel;
    public GameObject cardpackOpenPanel;
    public GameObject gamePanel;
    public GameObject cardsPanel;

    [SerializeField]
    private UICards currentClickedCard = null;

    [SerializeField, ReadOnly]
    private List<Card> _currentCardPool;

    [SerializeField, ReadOnly]
    List<UICards> _cards = new List<UICards>();

    [SerializeField]
    private BuildingDetailHandler _buildingDetailHandler;

    void Awake()
    {
        Instance = this;

        gamePanel.SetActive(false);
        cardpackPanel.SetActive(false);
        rerollButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        boosterPackButton.onClick.AddListener(OpenBoosterPack);
        rerollButton.onClick.AddListener(Reroll);
        continueButton.onClick.AddListener(Continue);
    }

    private void OnEnable()
    {
        DayNightCycleManager.DayBegin += OnDayStart;
    }

    private void OnDisable()
    {
        DayNightCycleManager.DayBegin -= OnDayStart;
    }

    

    public void ShowPanel(List<Card> pd)
    {
        _currentCardPool = new List<Card>();
        _currentCardPool = pd;

        gamePanel.SetActive(false);
        cardpackPanel.SetActive(true);
        boosterPackButton.gameObject.SetActive(true);
        rerollButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

    }

    public bool IsPlacingCard()
    {
        return currentClickedCard != null;
    }

    void OpenBoosterPack()
    {
        boosterPackButton.gameObject.SetActive(false);

        SpawnCards();
        FadeInCards();

        // TODO: add it back later
        rerollButton.gameObject.SetActive(false);
        rerollButton.interactable = true;
        continueButton.gameObject.SetActive(true);
        continueButton.interactable = true;
        //rerollButton.transform.DOScale(0, 0);
        //continueButton.transform.DOScale(0, 0);
        //rerollButton.transform.DOScale(1, 1);
        //continueButton.transform.DOScale(1, 1);
    }

    void Reroll()
    {
        FadeOutCards();
        rerollButton.interactable = false;
        continueButton.interactable = false;
        UnityEngine.UI.Image rerollButtonImage = rerollButton.GetComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Image continueButtonImage = continueButton.GetComponent<UnityEngine.UI.Image>();
        rerollButtonImage.gameObject.SetActive(true);
        continueButtonImage.gameObject.SetActive(true);

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
    //    DOTween.Sequence()
    //        .AppendInterval(1f)
    //        .OnComplete(() =>
    //        {
    //            //FadeOutCards();
    //            rerollButton.interactable = false;
    //            continueButton.interactable = false;
    //            UnityEngine.UI.Image rerollButtonImage = rerollButton.GetComponent<UnityEngine.UI.Image>();
    //            UnityEngine.UI.Image continueButtonImage = continueButton.GetComponent<UnityEngine.UI.Image>();
    //            rerollButtonImage.gameObject.SetActive(false);
    //            continueButtonImage.gameObject.SetActive(false);
    //        });

        rerollButton.interactable = false;
        continueButton.interactable = false;
        UnityEngine.UI.Image rerollButtonImage = rerollButton.GetComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Image continueButtonImage = continueButton.GetComponent<UnityEngine.UI.Image>();
        rerollButtonImage.gameObject.SetActive(false);
        continueButtonImage.gameObject.SetActive(false);

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
                        //  card.SetUIFlowRef(this);
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
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                gamePanel.SetActive(true);
            });

        
    }

    private void Update()
    {
        HandleInput();

        if (currentClickedCard != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                currentClickedCard.CardData.EndExecute();
                currentClickedCard.ResetPosition();
                currentClickedCard = null;
            }
        }

        
    }

    void SpawnCards()
    {
        foreach (var card in _currentCardPool)
        {
            GameObject gocard = Instantiate(cardPrefab, cardpackOpenPanel.transform);
            var cardObject = gocard.GetComponent<CardObject>();
            cardObject.PopulateCard(card);
            gocard.name = "Card " + card.CardName;

            _cards.Add(gocard.GetComponent<UICards>());
            _cards[_cards.Count - 1].SetUIFlowRef(this);
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

        cards = cardsPanel.GetComponentsInChildren<Transform>();

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
        _startNightButton.interactable = false;

        if (card == null)
            _startNightButton.interactable = true;

        if (currentClickedCard == card)
        {   
            DeselectCurrentlyHeldCard();
            return;
        }

        _buildingDetailHandler.CloseDetailPanel();

        if (currentClickedCard != null)
        {
            currentClickedCard.ResetPosition();
            currentClickedCard.CardData.EndExecute();
        }

        currentClickedCard = card;
        currentClickedCard.SelectCard();
        currentClickedCard.CardData.Execute();
    }

    public void DeselectCurrentlyHeldCard()
    {
        _startNightButton.interactable = true;

        currentClickedCard.ResetPosition();
        currentClickedCard.CardData.EndExecute();
        _buildingDetailHandler.CloseDetailPanel();
        currentClickedCard = null;
    }

    public void DiscardCard(Card buildingCard)
    {
        _startNightButton.interactable = true;

        Destroy(currentClickedCard.gameObject);
        currentClickedCard = null;
    }

    public bool HasCardSelected()
    {
        return currentClickedCard != null;
    }

    public void ContinueToNight()
    {
        DayNightCycleManager.Instance.EndDayCycle();
        Cleanup();
    }

    private void Cleanup()
    {
        DestroyCards();
        _cards.Clear();

        gamePanel.SetActive(false);
        cardpackPanel.SetActive(false);
        rerollButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }

    void OnDayStart()
    {
        ShowPanel(UICardManager.Instance.GetRandomCards(5));
    }

    //THIS IS TEMP 
    void HandleInput()
    {
       
        
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (_cards[0] != null)
                    SetSelectedCard(_cards[0]);
                    
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (_cards[1] != null)
                    SetSelectedCard(_cards[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (_cards[2] != null)
                    SetSelectedCard(_cards[2]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (_cards[3] != null)
                    SetSelectedCard(_cards[3]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (_cards[4] != null)
                    SetSelectedCard(_cards[4]);
            }
        
    }

}
