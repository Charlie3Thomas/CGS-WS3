using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using CT;


public class ScorecardUI : MonoBehaviour
{
    // Animation Variables
    public float fadetime = 1f;
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
    private float counter5 = 0.0f;

    private float awarenessPoints = 0.0f;
    private float disasterPoints  = 0.0f;
    private float nodePoints      = 0.0f;
    private float populationPoints= 0.0f;
    private float totalPoints     = 0.0f;

    private float incrementRate1 = 10f;
    private float incrementRate2 = 10f;
    private float incrementRate3 = 10f;
    private float incrementRate4 = 10f;
    private float incrementRate5 = 10f;



    private void Awake()
    {

        awarenessPoints  = (10000 * (1- GameManager._INSTANCE.GetAwareness()));
        disasterPoints = GameManager._INSTANCE.GetLastSurvivedTurn();
        nodePoints = 1000 * GameManager._INSTANCE.GetTechnologiesUnlockedTotal();
        populationPoints = 10 * GameManager._INSTANCE.GetLastTurnPopulation();
        Debug.Log("awarenessPoints : " + GameManager._INSTANCE.GetAwareness() + "disasterPoints : " + GameManager._INSTANCE.GetLastSurvivedTurn() + "nodePoints : " + GameManager._INSTANCE.GetTechnologiesUnlockedTotal() + "populationPoints : " + GameManager._INSTANCE.GetLastTurnPopulation());

        totalPoints = awarenessPoints + disasterPoints + nodePoints + populationPoints;

        incrementRate1 = awarenessPoints / (50 * 4);
        incrementRate2 = disasterPoints / (50 * 4);
        incrementRate3 = nodePoints / (50 * 4);
        incrementRate4 = populationPoints / (50 * 4);
        incrementRate5 = totalPoints / (50 * 4);



    }
    
    void Start()
    {
        FadeInAnimation();
        ScoreBoardAudio.Instance.PlayScoreRiseAudio();

    }


    private void FixedUpdate()
    {

        IncrementPoints();
    }


    public void BackToMainMenuAndReset()
    {
        DOTween.Clear(true); // Clear animation cache
       
        SceneManager.LoadScene(0);

        
        
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
            ScoreBoardAudio.Instance.PlayShowLeaderboardAudio();
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
        

        if (counter4 < populationPoints)
        {
            counter4 += incrementRate4;
            pointsUI[3].text = counter4.ToString();
        }

        if (counter5 < totalPoints)
        {
            counter5 += incrementRate5;
            pointsUI[4].text = counter5.ToString();
        }

    }

}
