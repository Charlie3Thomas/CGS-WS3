using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Data.Changes
{
    public class PopulationGrowth : CTChange
    {
        public PopulationGrowth(uint _delta) 
        { 
            this.delta = _delta;
        }

        public uint delta;

        public override void ApplyChange(ref CTYearData _year)
        {
            _year.Population += (int)this.delta;
        }
    }

}
