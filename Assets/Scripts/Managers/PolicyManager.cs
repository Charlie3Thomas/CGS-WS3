using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public List<Policy> policyList = new List<Policy>();
    private int numOfPolicies = 7;
    public GameObject policyCardPrefab;
    public HashSet<string> finalChoices = new HashSet<string>();

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

    public void ReplacePolicyCard()
    {
        StartCoroutine(Replace());
    }

    private IEnumerator Replace()
    {
        if (policyCardPrefab == null)
            yield return null;

        int missingPolicyCard = 1;

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < numOfPolicies; i++)
        {
            if(GameObject.Find("CardHolder" + (i + 1)).transform.childCount == 0)
            {
                missingPolicyCard = i + 1;
                Debug.Log("CardHolder" + missingPolicyCard);
            }
        }

        GameObject pc = Instantiate(policyCardPrefab, Vector3.zero, Quaternion.Euler(-5f, 180, 0), GameObject.Find("CardHolder" + (missingPolicyCard).ToString()).transform);
        pc.name = policyCardPrefab.name + (missingPolicyCard).ToString();
        Policy pol = new Policy();

        while (true)
        {
            pol.finalChoice = choices[Random.Range(0, choices.Length)];
            if (!finalChoices.Contains(pol.finalChoice))
            {
                break;
            }
        }

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
        ComputerController.Instance.policyCards[missingPolicyCard - 1] = pc;
        pc.GetComponent<PolicyCard>().policy = pol;
        ComputerController.Instance.pCardAnims[missingPolicyCard - 1] = pc.GetComponent<Animator>();
        ComputerController.Instance.pCardTexts[missingPolicyCard - 1] = pc.transform.GetChild(0).GetComponent<TMP_Text>();
        ComputerController.Instance.pCardTexts[missingPolicyCard - 1].text = pol.finalChoice + "\nIncrease in " + pol.effectType.ToString().ToLower() +
            " gain by " + pol.effect * 100 + "%" + "\nRequirement: " + pol.Requirement + " " + pol.requiredFaction.ToString().ToLower() + "s" +
                "\nCost: " + pol.cost + " " + pol.resourceCost.ToString().ToLower() + "s";

        yield return null;
    }

    private static int SortByYear(Policy pol1, Policy pol2)
    {
        return pol1.year.CompareTo(pol2.year);
    }
}
