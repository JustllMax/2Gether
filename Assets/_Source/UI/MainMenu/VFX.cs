using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class VFX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    struct Combo
    {
        public RectTransform t;
        public Vector3 startPos;

        public Combo(RectTransform t, Vector3 startPos)
        {
            this.t = t;
            this.startPos = startPos;
        }
    }

    [Header("Offset")]
    public float moveDuration = 1f;
    public float moveOffset = -100f;
    [Header("Fade")]
    public float fadeDuration = 1f;
    public float fadeMin = 0.2f;
    public float fadeMax = 1f;

    List<Combo> assets;
    Image background;
    private void Awake()
    {
        assets = new List<Combo>();
        List<RectTransform> transformChildren = transform.GetComponentsInChildren<RectTransform>().ToList();
        transformChildren.RemoveAt(0);
        foreach(RectTransform c in transformChildren)
        {
            assets.Add(new Combo(c, c.anchoredPosition));
            Debug.Log(c.position);
        }

        background = transformChildren[0].GetComponent<Image>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach(var child in assets)
            child.t.DOAnchorPosX(child.startPos.x + moveOffset, moveDuration);
        
        background.DOFade(fadeMax, fadeDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var child in assets)
            child.t.DOAnchorPosX(child.startPos.x, moveDuration);

        background.DOFade(fadeMin, fadeDuration);
    }
}
