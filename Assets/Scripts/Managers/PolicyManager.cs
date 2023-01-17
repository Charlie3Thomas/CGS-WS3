using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyManager : MonoBehaviour
{
    public static PolicyManager instance;
    [System.Serializable]
    public class Policy
    {
        public string finalChoice;
        public int year;
        public int cost;
        public int publicFavour;
        public int awareness;
        public string resultOfEffect;
    }

    private string[] choices = { "Cut all trees", "Build a dam", "Increase taxes", "Kill john lennon", "Commit mass genocide", "Throw a party!",
    "Lower taxes", "Throw waste into the ocean", "Build more roads", "Build more factories", "Build more houses" };
    private List<Policy> policyList = new List<Policy>();
    HashSet<string> finalChoices;
    private int numOfPolicies = 7;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void NewPolicySet()
    {
        int policyCount = 0;
        HashSet<string> finalChoices = new HashSet<string>();
        while (policyCount < numOfPolicies)
        {
            Policy pol = new Policy();
            pol.finalChoice = choices[Random.Range(0, choices.Length)];
            if (finalChoices.Contains(pol.finalChoice))
            {
                continue;
            }
            else
            {
                finalChoices.Add(pol.finalChoice);
                pol.year = YearData._INSTANCE.current_year;
                pol.cost = Random.Range(-10000, 10000);
                pol.publicFavour = Random.Range(-10000, 10000);
                pol.awareness = Random.Range(-10000, 10000);
                policyList.Add(pol);
                policyList.Sort(SortByYear);
                UIController.Instance.pCardText[policyCount].text = pol.finalChoice + "\nCost: " + pol.cost +
                "\nPublic Favour: " + pol.publicFavour + "\nAwareness: " + pol.awareness;
                policyCount++;
            }
        }
    }

    private static int SortByYear(Policy pol1, Policy pol2)
    {
        return pol1.year.CompareTo(pol2.year);
    }
}
