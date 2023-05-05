using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class ScorecardUI : MonoBehaviour
{
    // Animation Variables
    public float fadetime = 5f;
    public CanvasGroup canvasGroup; 
    public RectTransform rectTransform;
    public List<GameObject> items = new List<GameObject>();


    // Point UI and Scorecard variables
    public List<TextMeshProUGUI> pointsUI = new List<TextMeshProUGUI>();
    // incremental counters for every field
    private float counter1 = 0.0f;
    private float counter2 = 0.0f;
    private float counter3 = 0.0f;
    private float counter4 = 0.0f;

    private float awarenessPoints=1000.0f;
    private float disasterPoints=25000.0f;
    private float nodePoints=20000f;
    private float yearPoints=22000f;

    private float incrementRate1 =10f;
    private float incrementRate2 = 10f;
    private float incrementRate3 = 10f;
    private float incrementRate4 = 10f;



    private void Start()
    {
        incrementRate1 = awarenessPoints / (50 * 4);
        incrementRate2 = disasterPoints / (50 * 4);
        incrementRate3 = nodePoints / (50 * 4);
        incrementRate4 = yearPoints / (50 * 4);
    }


    private void FixedUpdate()
    {

        IncrementPoints();
    }

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

    private void IncrementPoints()
    {
        

        if (counter1 < awarenessPoints)
        {
            counter1 += incrementRate1;
            pointsUI[0].text = counter1.ToString();
        }
       

        if (counter2 < disasterPoints)
        {
            counter2 += incrementRate2;
            pointsUI[1].text = counter2.ToString();
        }
        

        if (counter3 < nodePoints)
        {
            counter3 += incrementRate3;
            pointsUI[2].text = counter3.ToString();
        }
        

        if (counter4 < yearPoints)
        {
            counter4 += incrementRate4;
            pointsUI[3].text = counter4.ToString();
        }
       

    }





}
