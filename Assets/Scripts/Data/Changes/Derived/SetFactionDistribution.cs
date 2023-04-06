using CT.Data;
using CT.Data.Changes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFactionDistribution : CTChange
{
    public SetFactionDistribution() { }

    /// <summary>
    /// Takes floats for each faction percentage. 
    /// Floats should not total to greater than 1
    /// </summary>
    public SetFactionDistribution(float _workers, float _scientists, float _farmers, float _planners)
    {
        float total_assigned = _workers + _scientists + _farmers + _planners;

        if (total_assigned > 1)
        {
            throw new ArgumentException("Cannot assign more than 100% of population!");
        }

        this.worker_percentage = _workers;
        this.scientist_percentage = _scientists;
        this.farmer_percentage = _farmers;
        this.planner_percentage = _planners;
    }

    public float worker_percentage;
    public float scientist_percentage;
    public float farmer_percentage;
    public float planner_percentage;

    public override void ApplyChange(ref CTYearData _year)
    {
        ResetDistribution(ref _year);
        AssignNewDistribution(ref _year);
    }

    private void ResetDistribution(ref CTYearData _year)
    {
        // Reset distribution for year
        _year.Workers = 0;
        _year.Scientists = 0;
        _year.Farmers = 0;
        _year.Planners = 0;
    }

    private void AssignNewDistribution(ref CTYearData _year)
    {
        // Asign new distribution
        _year.Workers = (int)(_year.Population * worker_percentage);
        _year.Scientists = (int)(_year.Population * scientist_percentage);
        _year.Farmers = (int)(_year.Population * farmer_percentage);
        _year.Planners = (int)(_year.Population * planner_percentage);
    }
}