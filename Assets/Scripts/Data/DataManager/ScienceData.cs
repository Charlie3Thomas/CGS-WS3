using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceData : MonoBehaviour
{
    public static ScienceData _INSTANCE;

    // [Year][Changed]
    // YearData._INSTANCE.changed_years[1997] = Changed (true/false)
    public Dictionary<int, int> science;

    public int earliest_year = 0;
    public int latest_year = 0;

    public int initial_science = 0;

    public int science_production = 0;

    // [Year][Funds]
    private void OnEnable()
    {
        earliest_year = 1900;
        latest_year = 2100;

        science = new Dictionary<int, int>();

        // Initialize dictionary with all years set to unchanged
        for (int i = 0; i <= GetYearRange(); i++)
        {
            int year = i + earliest_year;
            if (i == 0) // Use starting funds
                science.Add(year, initial_science);
        }

        Debug.Log("Initialised ScienceData");
    }

    private void Awake()
    {
        if (_INSTANCE != null)
        {
            Debug.LogError("Multiple ScienceData singleton instances!");
            Destroy(this.gameObject);
        }
        else { _INSTANCE = this; }
    }

    public int GetYearRange()
    {
        return latest_year - earliest_year;
    }

    public int GetScienceForYear(int _year)
    {
        return ScienceData._INSTANCE.science[_year];
    }
}
