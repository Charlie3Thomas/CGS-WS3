using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Data.Changes
{
    using Lookup;
    public class TrackAwareness : CTChange
    {

        public TrackAwareness() { }

        public TrackAwareness(float _ware)
        {
            value = _ware;
        }

        public float value;

        public override void ApplyChange(ref CTYearData _year)
        {
            _year.Awareness = value;
        }
    }
}
