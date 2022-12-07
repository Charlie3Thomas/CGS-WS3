using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEconomy : MonoBehaviour
{
    private void TestFunction(int _year, float _multiplier)
    {
        int econ_base_value = PlayerEconData._INSTANCE.economy[_year];
        int econ_modified_value = (int)(econ_base_value * _multiplier);
        PropagateChanges(_year, econ_modified_value);
    }

    public void PropagateChanges(int _year, int _value)
    {
        // Update Changed Year
        PlayerEconData._INSTANCE.economy[_year] = _value;

        // Update Economy future from changed value
        for (int i = _year + 1; i <= PlayerEconData._INSTANCE.latest_year; i++)
        {
            PlayerEconData._INSTANCE.economy[i] = (int)(_value * PlayerEconData._INSTANCE.econ_growth_rate);
        }
    }

    private void Start()
    {
        TestFunction(1997, 2.0f);
    }
}
