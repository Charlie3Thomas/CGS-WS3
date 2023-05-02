
namespace CT.Data.Changes
{
    using Lookup;

    public class SetPolicy : CTChange
    {
        public SetPolicy(CTPolicyCard _policy)
        {
            this.policy = _policy;
        }

        public CTPolicyCard policy;

        public override void ApplyChange(ref CTTurnData _year)
        {
            _year.applied_policies.Add(policy);
            _year.ApplyCosts(policy.cost, CTCostType.Purchase);
        }
    }
}
