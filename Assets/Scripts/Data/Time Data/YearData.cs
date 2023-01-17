using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Data Container to store whether the player has made changes to a year
    Used to determine whether the "Mayor" or player decisions will be used in calculations 
 */

public class YearData : MonoBehaviour
{
    public static YearData _INSTANCE;

    // [Year][Changed]
    // YearData._INSTANCE.changed_years[1997] = Changed (true/false)
    public Dictionary<int, bool> changed_years;

    public int earliest_year;
    public int latest_year;
    public int current_year;

    private void OnEnable() // Must be done on enable as other faction data initialisation is reliant on this
    {
        earliest_year = 1900;
        latest_year = 2100;
        current_year = Random.Range(earliest_year, latest_year);
        changed_years = new Dictionary<int, bool>();

        // Initialize dictionary with all years set to unchanged
        for (int i = 0; i <= GetYearRange(); i++)
        {
            int year = i + earliest_year;
            changed_years.Add(year, false);
        }

        Debug.Log("Initialised YearData");
    }

    // [Year][Changed]
    // FactionNumberData._INSTANCE.changed_years[1997] = Changed (true/false)
    private void Awake()
    {
        if (_INSTANCE != null) 
        { 
            Debug.LogError("Multiple YearData singleton instances!");
            Destroy(this.gameObject);
        }
        else { _INSTANCE = this; }
    }

    public void YearUp()
    {
        if(current_year < latest_year)
            current_year++;
    }

    public void YearDown()
    {
        if(current_year > earliest_year)
            current_year--;
    }

    public int GetYearRange()
    {
        return latest_year - earliest_year;
    }
}
