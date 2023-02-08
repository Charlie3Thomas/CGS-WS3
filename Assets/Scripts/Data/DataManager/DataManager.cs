using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager _INSTANCE;

    private void Awake()
    {
        if (_INSTANCE != null)
        {
            Debug.LogError("Multiple DataManager singleton instances!");
            Destroy(this.gameObject);
        }
        else { _INSTANCE = this; }
    }

    enum DataType
    {
        MONEY,
        SCIENCE,
        FOOD,
        POPULATION
    }

    #region MONEY
    // MONEY 
    // GAIN MONEY
    private void AddMoney(int _year, int _value)
    {
        int initial_money = MoneyData._INSTANCE.money[_year];
        int updated_money = initial_money + _value;
        PropagateChanges(_year, DataType.MONEY, updated_money);
    }

    // SPEND MONEY
    private void SpendMoney(int _year, int _value)
    {
        int initial_money = MoneyData._INSTANCE.money[_year];
        int updated_money = initial_money - _value;
        PropagateChanges(_year, DataType.MONEY, updated_money);
    }

    #endregion


    #region SCIENCE
    // SCIENCE
    // GAIN SCIENCE
    private void AddScience(int _year, int _value)
    {
        int initial_science = ScienceData._INSTANCE.science[_year];
        int updated_science = initial_science + _value;
        PropagateChanges(_year, DataType.SCIENCE, updated_science);
    }

    // SPEND SCIENCE
    private void SpendScience(int _year, int _value)
    {
        int initial_science = ScienceData._INSTANCE.science[_year];
        int updated_science = initial_science - _value;
        PropagateChanges(_year, DataType.SCIENCE, updated_science);
    }
    #endregion


    #region FOOD
    // FOOD
    // GAIN FOOD
    private void AddFood(int _year, int _value)
    {
        int initial_food = FoodData._INSTANCE.food[_year];
        int updated_food = initial_food + _value;
        PropagateChanges(_year, DataType.FOOD, updated_food);
    }


    // SPEND FOOD
    private void SpendFood(int _year, int _value)
    {
        int initial_food = FoodData._INSTANCE.food[_year];
        int updated_food = initial_food - _value;
        PropagateChanges(_year, DataType.FOOD, updated_food);
    }
    #endregion


    #region POPULATION
    // POPULATION
    // POPULATION GROWTH
    private void AddPopulation(int _year, int _value)
    {
        //int initial_population = 
    }

    // POPULATION UPKEEP
    #endregion

    // Update Changes through entire timeline for specific data containers
    private void PropagateChanges(int _year, DataType _type, int _value)
    {
        switch (_type)
        {
            case DataType.MONEY:
                // Update Changed Year

                MoneyData._INSTANCE.money[_year] = _value;

                // For each year in timeline from changed point
                for (int i = _year + 1; i <= MoneyData._INSTANCE.latest_year; i++)
                {
                    int money_last_year = MoneyData._INSTANCE.money[i - 1]; // Get previous year value
                    int growth_funds = (int)(money_last_year * MoneyData._INSTANCE.money_growth_rate); // Add growth value to previous value
                    MoneyData._INSTANCE.money[i] = growth_funds; // Update data point for current year with growth value
                }

                break;

            case DataType.SCIENCE:
                // Update Changed Year

                ScienceData._INSTANCE.science[_year] = _value;

                // For each year in timeline from changed point
                for (int i = _year + 1; i <= ScienceData._INSTANCE.latest_year; i++)
                {
                    int science_last_year = ScienceData._INSTANCE.science[i - 1]; // Get previous year value
                    int growth_science = (int)(science_last_year + ScienceData._INSTANCE.science_production); // Add growth value to previous value
                    ScienceData._INSTANCE.science[i] = growth_science; // Update data point for current year with growth value
                }

                break;

            case DataType.FOOD:
                // Update Changed Year

                FoodData._INSTANCE.food[_year] = _value;

                // For each year in timeline from changed point
                for (int i = _year + 1; i <= FoodData._INSTANCE.latest_year; i++)
                {
                    int food_last_year = FoodData._INSTANCE.food[i - 1]; // Get previous year value
                    int growth_food = (int)(food_last_year + FoodData._INSTANCE.food_production); // Add growth value to previous value
                    FoodData._INSTANCE.food[i] = growth_food; // Update data point for current year with growth value
                }

                break;

            case DataType.POPULATION:
                break;
        }

    }

    /*
     
    bool CHECK VALID CHANGE ()

    Look at current change request

    Evaluate against existing timeline changes

    Check if proposed change prevents future changes from being possible

    return true if valid

    else return false 
     
     
     */

}