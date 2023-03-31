using CT.Data;
using CT.Enumerations;
using CT.Technologies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OingoBoingo : MonoBehaviour
{
    CT.Data.CTTimelineData timeline;

    private CTYearData year;

    public int year_range;

    // Start is called before the first frame update
    void Start()
    {
        timeline = new CT.Data.CTTimelineData();
        timeline.Initialise(1, DataSheet.turns_number);

        timeline.BuyTech(0, CT.Technologies.CTTechnologies.Spoons);
        timeline.ChangePopulationDistribution(0, 0.1f, 0.1f, 0.8f, 0.0f);
        timeline.ApplyDisasterEffect(0, CT.Technologies.CTDisasters.NoCoffee);

        year = timeline.GetYearData(10);
    }
}
