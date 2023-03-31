using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Data container to store disaster timeline
    Used to store what disasters occur at what times
 */

public class DisasterTimelineData : MonoBehaviour
{
    public static DisasterTimelineData _INSTANCE;

    // [Year][Disaster]
    // DisasterTimelineData._INSTANCE.disaster_timeline[1997] = Disaster type (enum)
    public Dictionary<int, DisasterEnum.type> disaster_timeline;

    private void Awake()
    {
        if (_INSTANCE != null) {
            Debug.LogError("Multiple FactionBudgetData singleton instances!");
            Destroy(this.gameObject); }
        else { _INSTANCE = this; }
    }

    private void Start()
    {
        disaster_timeline = new Dictionary<int, DisasterEnum.type>();

        // Initialize dictionary with all disasters as NONE
        for (int i = 0; i < YearData._INSTANCE.GetYearRange(); i++)
        {
            int year = i + YearData._INSTANCE.earliest_year;
            disaster_timeline.Add(year, DisasterEnum.type.NONE);
        }
        Debug.Log("Initialised DisasterTimelineData");
    }

}
