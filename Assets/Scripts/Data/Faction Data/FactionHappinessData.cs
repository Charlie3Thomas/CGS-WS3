using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Data container to store faction happiness values per year, per faction
    Used to store absolute values of faction happiness after calculation
 */

public class FactionHappinessData : MonoBehaviour
{
    public static FactionHappinessData _INSTANCE;
    private FactionEnum faction_type;

    // [Year][Faction][Happiness]
    // FactionHappinessData._INSTANCE.happiness_data[1997][Workers] = Happiness
    public Dictionary<int,
           Dictionary<FactionEnum.Faction, int>> happiness_data;

    private void Start()
    {
        happiness_data = new Dictionary<int, Dictionary<FactionEnum.Faction, int>>();

        // Initialize dictionary with all faction happiness at zero
        for (int i = 0; i < YearData._INSTANCE.GetYearRange(); i++)
        {
            int year = i + YearData._INSTANCE.earliest_year;
            happiness_data.Add(year, new Dictionary<FactionEnum.Faction, int>());

            happiness_data[year].Add(FactionEnum.Faction.WORKER, 0);
            happiness_data[year].Add(FactionEnum.Faction.FARMER, 0);
            happiness_data[year].Add(FactionEnum.Faction.SCIENTIST, 0);
            happiness_data[year].Add(FactionEnum.Faction.ES_WORKER, 0);
        }
    }

    private void Awake()
    {
        if (_INSTANCE != null) 
        { 
            Debug.LogError("Multiple FactionHappinessData singleton instances!");
            Destroy(this.gameObject);
        }
        else { _INSTANCE = this; }
    }
}
