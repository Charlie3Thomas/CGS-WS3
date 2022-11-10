using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Data container to store faction quantity values per year, per faction
    Used to store absolute values of faction quantities after calculation
 */

public class FactionNumberData : MonoBehaviour
{
    public static FactionNumberData _INSTANCE;
    private FactionEnum faction_type;

    // [Year][Faction][Quantity]
    // FactionNumberData._INSTANCE.number_data[1997][Workers] = Quantity
    public Dictionary<int,
           Dictionary<FactionEnum.Faction, int>> number_data;

    private void Start()
    {
        number_data = new Dictionary<int, Dictionary<FactionEnum.Faction, int>>();

        // Initialize dictionary with all faction numbers at zero
        for (int i = 0; i < YearData._INSTANCE.GetYearRange(); i++)
        {
            int year = i + YearData._INSTANCE.earliest_year;
            number_data.Add(year, new Dictionary<FactionEnum.Faction, int>());

            number_data[year].Add(FactionEnum.Faction.WORKER, 0);
            number_data[year].Add(FactionEnum.Faction.FARMER, 0);
            number_data[year].Add(FactionEnum.Faction.SCIENTIST, 0);
            number_data[year].Add(FactionEnum.Faction.ES_WORKER, 0);
        }
    }

    private void Awake()
    {
        if (_INSTANCE != null) 
        { 
            Debug.LogError("Multiple FactionNumberData singleton instances!");
            Destroy(this.gameObject);
        }
        else { _INSTANCE = this; }
    }
}
