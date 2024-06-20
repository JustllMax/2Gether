using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UICards : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public CardObject CardData;
    [SerializeField]
    private Vector2 upOffset = new Vector2(0f, 200f);
    [SerializeField]
    private bool isSelected = false;
    [SerializeField]
    bool isHovered = false;
    [SerializeField]
    private float originalY;
    private Vector3 originalScale;
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
        if (!isSelected && transform.parent != null && transform.parent.name == "cardsPanel")
        {
            MaximizeCard();
            
        }
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (!isSelected && transform.parent != null && transform.parent.name == "cardsPanel")
        {
            MinimizeCard();
            
        }
        isHovered = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {


        if (_flowRef.HasCardSelected() && eventData.button == PointerEventData.InputButton.Left)
        {
            if (_flowRef.GetCurrentlyHeldCard() == this)
            {
                _flowRef.DeselectCurrentlyHeldCard();
                return;
            }
            _flowRef.DeselectCurrentlyHeldCard();

        }


        if (!isSelected && transform.parent != null && transform.parent.name == "cardsPanel")
        {
            Debug.Log(this + " was selected");
            isSelected = true;
            _flowRef.SetSelectedCard(this);



        }
    }

    public void ResetPosition()
    {
        isSelected = false;
        if(isHovered == false)
            MinimizeCard();
    }

    public void MinimizeCard()
    {
        rectTransform.anchoredPosition -= upOffset;
        rectTransform.localScale = originalScale;
    }

    public void MaximizeCard()
    {
        rectTransform.localScale = originalScale * scaleModifier;
        rectTransform.anchoredPosition += upOffset;
    }


    public void SelectCard()
    {
        isSelected = true;
        if(isHovered == false)
        {
            MaximizeCard();
        } 
    }

    public void SetInitialYPosition()
    {
        originalY = rectTransform.anchoredPosition.y;
    }
}