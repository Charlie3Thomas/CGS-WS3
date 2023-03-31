using CT.Data;
using CT.Technologies;
using UnityEngine;

public class OingoBoingo : MonoBehaviour
{
    CTTimelineData timeline;

    private CTYearData year;

    public int year_range;

    // Start is called before the first frame update
    void Start()
    {
        timeline = new CTTimelineData();
        timeline.Initialise(1, DataSheet.turns_number);

        timeline.ApplyPolicy(0, CTPolicies.DrinkMoreWater);
        timeline.BuyTech(0, CTTechnologies.Banking);
        timeline.ChangePopulationDistribution(0, 0.1f, 0.1f, 0.8f, 0.0f);
        timeline.ApplyDisasterEffect(0, CTDisasters.NoCoffee);

        year = timeline.GetYearData(10);
    }
}
