using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodData : MonoBehaviour
{
    public static FoodData _INSTANCE;

    // [Year][Changed]
    // YearData._INSTANCE.changed_years[1997] = Changed (true/false)
    public Dictionary<int, int> food;

    public int earliest_year = 0;
    public int latest_year = 0;

    public int initial_food = 1337;

    public int food_production = 0;

    // [Year][Funds]
    private void OnEnable()
    {
        earliest_year = 1900;
        latest_year = 2100;

        food = new Dictionary<int, int>();

        // Initialize dictionary with all years set to unchanged
        for (int i = 0; i <= GetYearRange(); i++)
        {
            int year = i + earliest_year;
            if (i == 0) // Use starting funds
                food.Add(year, initial_food);
        }

        Debug.Log("Initialised FoodData");
    }

    private void Awake()
    {
        if (_INSTANCE != null)
        {
            Debug.LogError("Multiple FoodData singleton instances!");
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
        return FoodData._INSTANCE.food[_year];
    }
}
