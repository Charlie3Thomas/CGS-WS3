using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Data.Resources
{
    /// <summary>
    /// Money, Science, Food, Population
    /// </summary>
    public class CTResourceTotals
    {
        public CTResourceTotals(int _m, int _s, int _f, int _p)
        {
            money = _m;
            science = _s;
            food = _f;
            population = _p;
        }

        public int money;
        public int science;
        public int food;
        public int population;
    }
}