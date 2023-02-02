using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Policy
{
    public string finalChoice;
    public int year;
    public float effect;
    public AllocType effectType;
    public int Requirement;
    public FactionEnum.type requiredFaction;
    public int cost;
    public AllocType resourceCost;
    //public int publicFavour;
    //public int awareness;
}

public class PolicyManager : MonoBehaviour
{
    public static PolicyManager instance;

    private string[] choices = { "Cut all trees", "Build a dam", "Increase taxes", "Kill john lennon", "Commit mass genocide", "Throw a party!",
    "Lower taxes", "Throw waste into the ocean", "Build more roads", "Build more factories", "Build more houses" };
    private List<Policy> policyList = new List<Policy>();
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
                pol.effect = Mathf.Round(Random.Range(0f, 1f) * 100f) / 100f;
                pol.effectType = (AllocType)Random.Range(0, 3);
                pol.requiredFaction = (FactionEnum.type)Random.Range(0, 4);
                pol.Requirement = Random.Range(0, 10000);
                pol.resourceCost = (AllocType)Random.Range(0, 3);
                pol.cost = Random.Range(0, 10000);
                //pol.publicFavour = Random.Range(-10000, 10000);
                //pol.awareness = Random.Range(-10000, 10000);
                policyList.Add(pol);
                policyList.Sort(SortByYear);
                ComputerController.Instance.policyCards[policyCount].GetComponent<PolicyCard>().policy = pol;
                ComputerController.Instance.pCardTexts[policyCount].text = pol.finalChoice + "\nIncrease in " + pol.effectType.ToString().ToLower() +
                    " gain by " + pol.effect * 100 + "%" + "\nRequirement: " + pol.Requirement + " " + pol.requiredFaction.ToString().ToLower() + "s" +
                        "\nCost: " + pol.cost + " " + pol.resourceCost.ToString().ToLower() + "s";
                policyCount++;
            }
        }
    }

    private static int SortByYear(Policy pol1, Policy pol2)
    {
        return pol1.year.CompareTo(pol2.year);
    }
}
