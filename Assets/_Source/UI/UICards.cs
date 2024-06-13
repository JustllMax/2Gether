using UnityEngine;
using UnityEngine.EventSystems;

public class UICards : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public CardObject CardData;

    private Vector3 upOffset = new Vector3(0f, 200f, 0f);
    private Vector3 originalScale;
    private bool isClicked = false;

    private UIFlow _flowRef;

    public void SetUIFlowRef(UIFlow flowRef)
        { this._flowRef = flowRef; }


    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked && transform.parent != null && transform.parent.name == "cardsPanel")
        {
            transform.localPosition += upOffset;
            transform.localScale = originalScale * 1.5f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClicked && transform.parent != null && transform.parent.name == "cardsPanel")
        {
            transform.localPosition -= upOffset;
            transform.localScale = originalScale;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isClicked && transform.parent != null && transform.parent.name == "cardsPanel")
        {
            _flowRef.SetSelectedCard(this);
            //if (currentClickedCard != null)
            //{
            //    currentClickedCard.ResetPosition();
            //}
            isClicked = true;
            //currentClickedCard = this;


        }
    }

    public void ResetPosition()
    {
        isClicked = false;
        transform.localPosition -= upOffset;
        transform.localScale = originalScale;
    }
}