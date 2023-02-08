using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyData : MonoBehaviour
{
    public static MoneyData _INSTANCE;

    // [Year][Changed]
    // YearData._INSTANCE.changed_years[1997] = Changed (true/false)
    public Dictionary<int, int> money;

    public int earliest_year = 0;
    public int latest_year = 0;

    public int initial_money = 1337;
    public float money_growth_rate = 1.01f;

    // [Year][Funds]
    private void OnEnable()
    {
        earliest_year = 1900;
        latest_year = 2100;

        money = new Dictionary<int, int>();

        // Initialize dictionary with all years set to unchanged
        for (int i = 0; i <= GetYearRange(); i++)
        {
            int year = i + earliest_year;
            if (i == 0) // Use starting funds
                money.Add(year, initial_money);
            else // Scale funds by economy growth rate
            {
                int money_last_year = money[year - 1];
                int growth_funds = (int)(money_last_year * money_growth_rate);
                money.Add(year, growth_funds);
            }
        }

        Debug.Log("Initialised MoneyData");
    }

    private void Awake()
    {
        if (_INSTANCE != null)
        {
            Debug.LogError("Multiple MoneyData singleton instances!");
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
        return MoneyData._INSTANCE.money[_year];
    }
}
