using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionManager : MonoBehaviour
{
    public static DecisionManager _INSTANCE;

    [System.Serializable]
    public class Decision
    {
        public string[] choices = { "Cut all trees", "Build a dam", "Increase taxes", "Kill john lennon", "Commit mass genocide", "Throw a party!" };
        public List<string> availableChoices = new List<string>();
        public string finalChoice;
        public int year;
        public int cost;
        public int publicFavour;
        public int awareness;
        public string resultOfEffect;
    }

    private int numOfChoices = 3;
    public List<Decision> decisionList = new List<Decision>();

    private void Start()
    {
        NewRandomDecision();
    }

    void NewRandomDecision()
    {
        Decision dec = new Decision();

        for (int i = 0; i < numOfChoices; i++)
        {
            dec.availableChoices.Add(dec.choices[Random.Range(0, dec.choices.Length)]);
        }

        dec.finalChoice = dec.availableChoices[Random.Range(0, dec.availableChoices.Count)];
        dec.year = YearData._INSTANCE.current_year;
        dec.cost = Random.Range(-10000, 10000);
        dec.publicFavour = Random.Range(-10000, 10000);
        dec.awareness = Random.Range(-10000, 10000);
        dec.resultOfEffect = "A decision of '" + dec.finalChoice + "' has been taken " 
            + ((dec.cost > 0) ? "which has cost you $" : "which has made you gain $ ") + Mathf.Abs(dec.cost) + ", "
            + ((dec.publicFavour > 0) ? "which has also gained the public's favour " : "which has also reduced the public's favour") + " by " + Mathf.Abs(dec.publicFavour) + " points and "
            + ((dec.awareness > 0) ? "has increased your awareness" : "has reduced your awareness") + " by " + Mathf.Abs(dec.awareness) + " points.";

        Debug.Log(dec.resultOfEffect);
        decisionList.Add(dec);
        var dec1 = dec;
        decisionList.Sort(SortByYear);
    }

    private static int SortByYear(Decision dec1, Decision dec2)
    {
        return dec1.year.CompareTo(dec2.year);
    }
}
