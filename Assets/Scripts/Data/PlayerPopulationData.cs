using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPopulationData : MonoBehaviour
{
    public static PlayerPopulationData _INSTANCE;

    // [Year][Changed]
    // YearData._INSTANCE.changed_years[1997] = Changed (true/false)
    public Dictionary<int, int> population;

    public int earliest_year = 0;
    public int latest_year = 0;

    public int starting_population = 69420;
    public float pop_growth_rate = 1.01f;

    // [Year][Funds]
    private void OnEnable()
    {
        earliest_year = 1900;
        latest_year = 2100;

        population = new Dictionary<int, int>();

        // Initialize dictionary with all years set to unchanged
        for (int i = 0; i <= GetYearRange(); i++)
        {
            int year = i + earliest_year;
            if (i == 0) // Use starting funds
                population.Add(year, starting_population);
            else // Scale funds by economy growth rate
            {
                int funds_last_year = population[year - 1];
                funds_last_year = (int)(funds_last_year * pop_growth_rate);
                population.Add(year, funds_last_year);
            }
        }

        Debug.Log("Initialised PlayerPopulationData");
    }

    private void Awake()
    {
        if (_INSTANCE != null)
        {
            Debug.LogError("Multiple PlayerEconData singleton instances!");
            Destroy(this.gameObject);
        }
        else { _INSTANCE = this; }
    }

    public int GetYearRange()
    {
        return latest_year - earliest_year;
    }

    public int GetPopulationForYear(int _year)
    {
        return PlayerPopulationData._INSTANCE.population[_year];
    }

    private void Start()
    {
        //int test_year = 1997;
        //Debug.Log("City population for the year " + test_year + " = " + PlayerPopulationData._INSTANCE.GetPopulationForYear(test_year));
    }
}
