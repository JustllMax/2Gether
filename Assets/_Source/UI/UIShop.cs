using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UIShop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject shopPanel;
    public Button closeButton;

    private Vector3 originalPosition;
    private Vector3 targetPosition;

    private bool isShopOpen = false;
    private bool keepShopOpen = false;

    void Start()
    {
        originalPosition = shopPanel.transform.localPosition;
        targetPosition = originalPosition + new Vector3(925f, 0f, 0f);

        closeButton.gameObject.SetActive(false);
        closeButton.onClick.AddListener(CloseShopOnClickButton);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OpenShop();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!keepShopOpen)
        {
            CloseShop();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isShopOpen)
        {
            OpenShop();
            keepShopOpen = true;
        }
        else
        {
            keepShopOpen = !keepShopOpen;
        }
    }

    void OpenShop()
    {
        closeButton.gameObject.SetActive(true);
        shopPanel.transform.DOLocalMove(targetPosition, 0.5f);
        isShopOpen = true;
    }

    public void CloseShop()
    {
        closeButton.gameObject.SetActive(false);
        shopPanel.transform.DOKill();
        shopPanel.transform.DOLocalMove(originalPosition, 0.5f);
        isShopOpen = false;
        keepShopOpen = false;
    }

    public void CloseShopOnClickButton()
    {
        CloseShop();
    }
}
