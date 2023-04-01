using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Policy
{
    public string finalTitle;
    public int year;
    public List<BuffsNerfs> buffs_nerfs = new List<BuffsNerfs>();
    public int Requirement;
    //public FactionEnum.type requiredFaction;
    [SerializeReference]
    public Resource cost = new Resource();
}

public class PolicyManager : MonoBehaviour
{
    public static PolicyManager instance;

    private string[] titles = { "Landmass Zoning", "Will-to-Drill", "Supreme Aid", "Basic Education", "Specialized Education", "Sign Language",
    "Lower taxes", "Throw waste into the ocean", "Build more roads", "Build more factories", "Kill john lennon" };
    public List<Policy> currentPolicies = new List<Policy>();
    public Policy currentSelectedPolicy;
    private int policyCardIndex = 0;
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

    private void Update()
    {
        if (currentPolicies.Count > 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                policyCardIndex = (policyCardIndex + 1) % currentPolicies.Count;
                currentSelectedPolicy = currentPolicies[policyCardIndex];
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                policyCardIndex--;
                if (policyCardIndex < 0)
                    policyCardIndex = currentPolicies.Count - 1;

                currentSelectedPolicy = currentPolicies[policyCardIndex];
            }
        }
    }

    public void NewPolicySet()
    {
        finalChoices.Clear();
        policyList.Clear();
        int policyCount = 0;
        //while (policyCount < numOfPolicies)
        //{
        //    Policy pol = new Policy();
        //    pol.finalTitle = titles[Random.Range(0, titles.Length)];
        //    if (finalChoices.Contains(pol.finalTitle))
        //    {
        //        continue;
        //    }
        //    else
        //    {
        //        finalChoices.Add(pol.finalTitle);
        //        //pol.year = YearData._INSTANCE.current_year;

        //        for (int i = 0; i < Random.Range(1, 3); i++)
        //        {
        //            pol.buffs_nerfs.Add(new BuffsNerfs((BuffsNerfsType)Random.Range(0, System.Enum.GetValues(typeof(BuffsNerfsType)).Length),
        //                Mathf.Round(Random.Range(-1f, 1f) * 100f) / 100f));
        //        }

        //        //pol.requiredFaction = (FactionEnum.type)Random.Range(0, 4);
        //        pol.Requirement = Random.Range(0, 10000);
        //        pol.cost.allocType = (AllocType)Random.Range(0, 3);
        //        pol.cost.amount = Random.Range(0, 10000);
        //        policyList.Add(pol);
        //        policyList.Sort(SortByYear);
        //        ComputerController.Instance.policyCards[policyCount].GetComponent<PolicyCard>().policy = pol;
        //        string effect = "";
        //        foreach (BuffsNerfs bns in pol.buffs_nerfs)
        //        {
        //            effect += ((bns.amount > 0) ? "Increase in " : "Decrease in ") + bns.type.ToString().ToLower() + " by " + (Mathf.Abs(bns.amount) * 100) + "%\n";
        //        }
        //        //ComputerController.Instance.pCardTexts[policyCount].text = pol.finalTitle + "\n" + effect + "Requirement: " + pol.Requirement + " " + pol.requiredFaction.ToString().ToLower() + "s" +
        //                //"\nCost: " + pol.cost.amount + " " + pol.cost.allocType.ToString().ToLower() + "s";
        //        policyCount++;
        //    }
        //}
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

        //while (true)
        //{
        //    pol.finalTitle = titles[Random.Range(0, titles.Length)];
        //    if (!finalChoices.Contains(pol.finalTitle))
        //    {
        //        break;
        //    }
        //}

        finalChoices.Add(pol.finalTitle);
        //pol.year = YearData._INSTANCE.current_year;

        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            pol.buffs_nerfs.Add(new BuffsNerfs((BuffsNerfsType)Random.Range(0, System.Enum.GetValues(typeof(BuffsNerfsType)).Length),
                Mathf.Round(Random.Range(-1f, 1f) * 100f) / 100f));
        }

        //pol.requiredFaction = (FactionEnum.type)Random.Range(0, 4);
        pol.Requirement = Random.Range(0, 10000);
        pol.cost.allocType = (AllocType)Random.Range(0, 3);
        pol.cost.amount = Random.Range(0, 10000);
        policyList.Add(pol);
        policyList.Sort(SortByYear);
        ComputerController.Instance.policyCards[missingPolicyCard - 1] = pc;
        pc.GetComponent<PolicyCard>().policy = pol;
        ComputerController.Instance.pCardAnims[missingPolicyCard - 1] = pc.GetComponent<Animator>();
        ComputerController.Instance.pCardTexts[missingPolicyCard - 1] = pc.transform.GetChild(0).GetComponent<TMP_Text>();

        string effect = "";
        foreach (BuffsNerfs bns in pol.buffs_nerfs)
        {
            effect += ((bns.amount > 0) ? "Increase in " : "Decrease in ")  + bns.type.ToString().ToLower() + " by " + (Mathf.Abs(bns.amount) * 100) + "%\n";
        }

        //ComputerController.Instance.pCardTexts[missingPolicyCard - 1].text = pol.finalTitle + "\n" + effect + "Requirement: " + pol.Requirement + " " + pol.requiredFaction.ToString().ToLower() + "s" +
                //"\nCost: " + pol.cost.amount + " " + pol.cost.allocType.ToString().ToLower() + "s";

        yield return null;
    }

    private static int SortByYear(Policy pol1, Policy pol2)
    {
        return pol1.year.CompareTo(pol2.year);
    }
}
