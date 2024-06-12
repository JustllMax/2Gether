using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using Cysharp.Threading.Tasks;

public class VFX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    struct Combo
    {
        public RectTransform t;
        public Vector3 startPos;
        public bool isPointerOn;
        public Combo(RectTransform t, Vector3 startPos)
        {
            this.t = t;
            this.startPos = startPos;
            this.isPointerOn = false;
        }
        public void SetIsPointer(bool val)
        {
            isPointerOn = val;
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

        }

        background = transformChildren[0].GetComponent<Image>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        foreach (var child in assets)
        {
            //child.t.DOAnchorPosX(child.startPos.x + moveOffset, moveDuration);
            
            StartCoroutine(MoveForwardComponent(child));
            //float dest = child.t.anchoredPosition.x + moveOffset;
            //child.t.anchoredPosition = new Vector2(Mathf.Lerp(child.t.anchoredPosition.x, dest, Time.unscaledDeltaTime), child.t.anchoredPosition.y);
      
        }
        StartCoroutine(FadeComponent(fadeMax));


        //background.DOFade(fadeMax, fadeDuration);
    }

    private IEnumerator MoveForwardComponent(Combo child)
    {
        Vector2 startPosition = child.t.anchoredPosition;
        Vector2 endPosition = startPosition + new Vector2(moveOffset, 0);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            child.t.anchoredPosition = Vector2.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;

        }

        child.t.anchoredPosition = endPosition;

    }



    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        foreach (var child in assets)
        {
            
            StartCoroutine(MoveBackwardComponent(child));
        }

        StartCoroutine(FadeComponent(fadeMin));
        //background.DOFade(fadeMin, fadeDuration);
    }

    private IEnumerator MoveBackwardComponent(Combo child)
    {
        Vector2 startPosition = child.t.anchoredPosition;
        Vector2 endPosition = child.startPos;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            child.t.anchoredPosition = Vector2.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;

        }

        child.t.anchoredPosition = endPosition;

    }

    private IEnumerator FadeComponent(float fadeAmount)
    {
        Color startColor = background.color;
        Color endColor = startColor;
        endColor.a = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            endColor.a = Mathf.Lerp(startColor.a, fadeAmount, elapsedTime / moveDuration);
            background.color = endColor;
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;

        }

        endColor.a = fadeAmount;
        background.color = endColor;

    }

    private void OnEnable()
    {
        foreach (var child in assets)
        {
            child.t.anchoredPosition = child.startPos;
        }
            

        background.color = new Color (background.color.r, background.color.g, background.color.b, fadeMin);
    }
}
