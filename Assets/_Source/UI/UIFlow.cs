using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    void Start()
    {
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
                FadeOutCards();
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
                Transform[] cards = cardpackOpenPanel.GetComponentsInChildren<Transform>();

                foreach (Transform card in cards)
                {
                    if (card != cardpackOpenPanel.transform)
                    {
                        card.SetParent(cardsPanel.transform, false);
                    }
                }
                cardpackPanel.SetActive(false);

                foreach (Transform card in cards)
                {
                    if (card != cardsPanel.transform)
                    {
                        UnityEngine.UI.Image cardImage = card.GetComponent<UnityEngine.UI.Image>();
                        cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0f);
                        cardImage.DOFade(1f, 1f).SetDelay(0.5f);
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
        for (int i = 0; i < 5; i++)
        {
            GameObject card = Instantiate(cardPrefab, cardpackOpenPanel.transform); // U¿ycie transform do rodzica
            card.name = "Card " + (i + 1);
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
                UnityEngine.UI.Image cardImage = card.GetComponent<UnityEngine.UI.Image>();
                cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0f);
                cardImage.DOFade(1f, 1f).SetDelay(0.5f);
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
}
