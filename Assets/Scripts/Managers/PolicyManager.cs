using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyManager : MonoBehaviour
{
    public static PolicyManager _INSTANCE;

    [System.Serializable]
    public class Policy
    {
        public List<string> availableChoices = new List<string>();
        public string finalChoice;
        public int year;
        public int cost;
        public int publicFavour;
        public int awareness;
        public string resultOfEffect;
    }

    private int numOfChoices = 3;
    public string[] choices = { "Cut all trees", "Build a dam", "Increase taxes", "Kill john lennon", "Commit mass genocide", "Throw a party!",
    "Lower taxes", "Throw waste into the ocean", "Build more roads", "Build more factories", "Build more houses" };
    public List<Policy> policyList = new List<Policy>();

    private void Start()
    {
        for (int i = 0; i < UIController.Instance.pCardText.Length; i++)
        {
            //NewRandomPolicy(i);
        }
    }

    void NewRandomPolicy(int cardNumber)
    {
        Policy pol = new Policy();

        for (int i = 0; i < numOfChoices; i++)
        {
            string currentChoice = choices[Random.Range(0, choices.Length)];
            while (!pol.availableChoices.Contains(currentChoice))
            {
                pol.availableChoices.Add(currentChoice);
                numOfChoices--;
            }
            numOfChoices++;
        }

        pol.finalChoice = pol.availableChoices[Random.Range(0, pol.availableChoices.Count)];
        pol.year = YearData._INSTANCE.current_year;
        pol.cost = Random.Range(-10000, 10000);
        pol.publicFavour = Random.Range(-10000, 10000);
        pol.awareness = Random.Range(-10000, 10000);

        UIController.Instance.pCardText[cardNumber].text = pol.finalChoice + "\nCost: " + pol.cost +
            "\nPublic Favour: " + pol.publicFavour + "\nAwareness: " + pol.awareness;
        /*
        pol.resultOfEffect = "A policy of '" + pol.finalChoice + "' has been taken " 
            + ((pol.cost > 0) ? "which has cost you $" : "which has made you gain $") + Mathf.Abs(pol.cost) + ", "
            + ((pol.publicFavour > 0) ? "which has also gained the public's favour " : "which has also reduced the public's favour") + " by " + Mathf.Abs(pol.publicFavour) + " points and "
            + ((pol.awareness > 0) ? "has increased your awareness" : "has reduced your awareness") + " by " + Mathf.Abs(pol.awareness) + " points.";

        Debug.Log(pol.resultOfEffect);
        */
        policyList.Add(pol);
        var pol1 = pol;
        policyList.Sort(SortByYear);
    }

    private static int SortByYear(Policy pol1, Policy pol2)
    {
        return pol1.year.CompareTo(pol2.year);
    }
}
