using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Data.Changes
{
    abstract public class CTChange
    {
        public int Year { get; set; }
        abstract public void ApplyChange(ref CTTurnData _year);
    }
}
