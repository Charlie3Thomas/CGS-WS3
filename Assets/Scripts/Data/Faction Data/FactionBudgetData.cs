using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Data container to store faction budget values per year, per faction
    Used to store absolute values of budget products after calculation
 */

public class FactionBudgetData : MonoBehaviour
{
    public static FactionBudgetData _INSTANCE;

    // [Year][Faction][Budget]
    // FactionBudgetData._INSTANCE.budget_data[1997][Workers] = Budget
    public Dictionary<int,
           Dictionary<FactionEnum.type, int>> budget_data;

    private void Awake()
    {
        if (_INSTANCE != null) { 
            Debug.LogError("Multiple FactionBudgetData singleton instances!");
            Destroy(this.gameObject); }
        else { _INSTANCE = this; }
    }

    private void Start()
    {
        budget_data = new Dictionary<int, Dictionary<FactionEnum.type, int>>();

        // Initialize dictionary with all faction budgets at zero
        for (int i = 0; i < YearData._INSTANCE.GetYearRange(); i++)
        {
            int year = i + YearData._INSTANCE.earliest_year;
            budget_data.Add(year, new Dictionary<FactionEnum.type, int>());

            budget_data[year].Add(FactionEnum.type.WORKER, 0);
            budget_data[year].Add(FactionEnum.type.FARMER, 0);
            budget_data[year].Add(FactionEnum.type.SCIENTIST, 0);
            budget_data[year].Add(FactionEnum.type.ES_WORKER, 0);
        }

        Debug.Log("Initialised FactionBudgetData");
    }

}
