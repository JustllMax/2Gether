using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VFX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Transform targetTransform;
    public Image targetBackground;


    public float moveDuration = 1f;
    public float fadeDuration = 1f;
    public float moveOffset = 2f;
    Vector3 startPos;


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.hovered[0].name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetTransform = eventData.hovered[0].transform;
        startPos = targetTransform.position;
        targetTransform.DOMoveX(targetTransform.position.x + moveOffset, moveDuration);
        targetBackground = targetTransform.GetComponentsInChildren<Image>()[1];
        targetBackground.DOFade(1f, fadeDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetTransform.DOMoveX(targetTransform.position.x - moveOffset, moveDuration);
        targetBackground.DOFade(0f, fadeDuration);
    }
}
