using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private void Start()
    {
        GameStartDataLoader();
    }

    private void GameStartDataLoader()
    {
        Debug.Log("1997 pre-change = " + YearData._INSTANCE.changed_years[1997]);
        YearData._INSTANCE.changed_years[1997] = true;
        Debug.Log("1997 post-change = " + YearData._INSTANCE.changed_years[1997]);

        // Perform all calcualtions to populate Data Containers with information
        // Faction number data using FactionNumberData._INSTANCE
        // Faction number data using FactionBudgetData._INSTANCE
        // Faction happiness data using FactionHappinessData._INSTANCE
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) { return; }
        // Year Data
        Handles.Label(transform.position + new Vector3(0, 0, -1), "Year:");
        Handles.Label(transform.position + new Vector3(0, 1, -1), "Changed:");

        int i = 0;
        foreach (KeyValuePair<int, bool> entry in YearData._INSTANCE.changed_years)
        {
            //Debug.Log((entry.Value) + " " + (entry.Key));
            Handles.Label(transform.position + new Vector3(0, 0, i), (entry.Key).ToString());
            Handles.Label(transform.position + new Vector3(0, 1, i), (entry.Value).ToString());
            i++;
        }

    }
}