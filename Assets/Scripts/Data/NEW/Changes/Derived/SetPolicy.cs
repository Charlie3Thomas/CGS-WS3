using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Data.Changes
{
    using Lookup;
    public class SetPolicy : CTChange
    {

        public SetPolicy() { }

        public SetPolicy(CTPolicies _policy)
        {
            this.policy = _policy;
        }

        public CTPolicies policy;

        public override void ApplyChange(ref CTYearData _year)
        {
            _year.active_policies[policy] = true;
        }
    }
}
