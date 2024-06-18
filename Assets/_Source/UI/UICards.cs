using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICards : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public CardObject CardData;

    private Vector3 upOffset = new Vector3(0f, 280f, 0f);
    private Vector3 originalScale;
    private bool isClicked = false;
    bool isHovered = false;
    RectTransform rectTransform;
    private UIFlow _flowRef;
    float scaleModifier = 1.25f;
    public void SetUIFlowRef(UIFlow flowRef)
        { this._flowRef = flowRef; }


    private void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked && transform.parent != null && transform.parent.name == "cardsPanel")
        {
            MaximizeCard();
            isHovered = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (!isClicked && transform.parent != null && transform.parent.name == "cardsPanel")
        {
            MinimizeCard();
            isHovered = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_flowRef.HasCardSelected() && eventData.button == PointerEventData.InputButton.Left)
        {
            _flowRef.DeselectCurrentlyHeldCard();
        }


        if (!isClicked && transform.parent != null && transform.parent.name == "cardsPanel")
        {
            ResetPosition();
            isClicked = true;
            _flowRef.SetSelectedCard(this);



        }
    }

    public void ResetPosition()
    {
        isClicked = false;
        MinimizeCard();
    }

    public void MinimizeCard()
    {
        rectTransform.localScale = originalScale;
        rectTransform.localPosition -= upOffset;
    }

    public void MaximizeCard()
    {
        rectTransform.localPosition += upOffset;
        rectTransform.localScale = originalScale * scaleModifier;
    }


    public void SelectCard()
    {
        isClicked = true;
        if(isHovered == false)
        {
            MaximizeCard();
        } 
    }

}