using UnityEngine;
using DG.Tweening;

public class UIShop : MonoBehaviour
{
    public GameObject shopPanel;
    private Vector3 originalPosition;
    private Vector3 centerPosition;

    private bool isShopOpen = false;

    void Start()
    {
        originalPosition = shopPanel.transform.localPosition;
        centerPosition = Vector3.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (isShopOpen)
            {
                CloseShop();
            }
            else
            {
                OpenShop();
            }
        }
    }

    void OpenShop()
    {
        shopPanel.transform.DOLocalMove(centerPosition, 0.5f);
        isShopOpen = true;
    }

    void CloseShop()
    {
        shopPanel.transform.DOLocalMove(originalPosition, 0.5f);
        isShopOpen = false;
    }
}
