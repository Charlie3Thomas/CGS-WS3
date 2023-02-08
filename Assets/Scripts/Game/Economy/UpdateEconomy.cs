using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEconomy : MonoBehaviour
{
    private void TestFunction(int _year, float _multiplier)
    {
        int econ_base_value = MoneyData._INSTANCE.money[_year];
        int econ_modified_value = (int)(econ_base_value * _multiplier);
        PropagateChanges(_year, econ_modified_value);
    }

    public void PropagateChanges(int _year, int _value)
    {
        // Update Changed Year
        MoneyData._INSTANCE.money[_year] = _value;

        // Update Economy future from changed value
        for (int i = _year + 1; i <= MoneyData._INSTANCE.latest_year; i++)
        {
            int funds_last_year = MoneyData._INSTANCE.money[i - 1]; // Year [i - 1] funds
            int growth_funds = (int)(funds_last_year * MoneyData._INSTANCE.money_growth_rate); // Multiply year [i - 1] funds by growth rate
            MoneyData._INSTANCE.money[i] = growth_funds; // Set year i funds to growth funds
        }
    }

    private void Update()
    {
        //// Debug
        //if (Input.GetKeyDown("k"))
        //{
        //    Debug.Log("PRESSED K");
        //    TestFunction(1997, 2.0f);
        //}
    }
}
