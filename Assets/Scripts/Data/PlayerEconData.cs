using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEconData : MonoBehaviour
{
    public static PlayerEconData _INSTANCE;

    // [Year][Changed]
    // YearData._INSTANCE.changed_years[1997] = Changed (true/false)
    public Dictionary<int, int> economy;

    public int earliest_year = 0;
    public int latest_year = 0;

    public int starting_funds = 1337;
    public float econ_growth_rate = 1.01f;

    // [Year][Funds]
    private void OnEnable()
    {
        earliest_year = 1900;
        latest_year = 2100;

        economy = new Dictionary<int, int>();

        // Initialize dictionary with all years set to unchanged
        for (int i = 0; i <= GetYearRange(); i++)
        {
            int year = i + earliest_year;
            if (i == 0) // Use starting funds
                economy.Add(year, starting_funds);
            else // Scale funds by economy growth rate
            {
                int funds_last_year = economy[year - 1];
                int growth_funds = (int)(funds_last_year * econ_growth_rate);
                economy.Add(year, growth_funds);
            }
        }

        Debug.Log("Initialised PlayerEconData");
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

    public int GetFundsForYear(int _year)
    {
        return PlayerEconData._INSTANCE.economy[_year];
    }    

    private void Start()
    {
        //int test_year = 1997;
        //Debug.Log("City funds for the year " + test_year + " = " + PlayerEconData._INSTANCE.GetFundsForYear(test_year));
    }
}
