
namespace CT.Data.Changes
{
    public class SetPolicy : CTChange
    {
        public SetPolicy(CTPolicyCard _policy)
        {
            this.policy = _policy;
        }

        public CTPolicyCard policy;

        public override void ApplyChange(ref CTYearData _year)
        {
            _year.applied_policies.Add(policy);
            //_year.OingoBoingo();
        }
    }
}
