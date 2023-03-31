using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Data.Changes
{
    using Technologies;
    public class PurchaseTechnology : CTChange
    {

        public PurchaseTechnology() { }

        public PurchaseTechnology(CTTechnologies _tech)
        {
            this.tech = _tech;
        }

        public CTTechnologies tech;

        public override void ApplyChange(ref CTYearData _year)
        {
            _year.active_technologues[tech] = true;
            _year.ApplyCosts(DataSheet.GetTechPrice(tech, _year));
        }
    }
}
