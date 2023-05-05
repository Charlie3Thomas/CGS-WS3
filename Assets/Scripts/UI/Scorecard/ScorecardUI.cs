using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScorecardUI : MonoBehaviour
{

    public float fadetime = 5f;
    public CanvasGroup canvasGroup; 
    public RectTransform rectTransform;
    public List<GameObject> items = new List<GameObject>();


    public void FadeInAnimation()
    {
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition = new Vector3(0f, -1000f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadetime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1, fadetime);

        StartCoroutine("PopupAnimation");

    }

    IEnumerator PopupAnimation()
    {
        foreach (var item in items)
        {
            item.transform.localScale = Vector3.zero;
        }

        foreach (var item in items)
        {
            item.transform.DOScale(1f, fadetime).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(1f);

        }
    }


}
