
namespace CT.Data.Changes
{
    public class RevokePolicy : CTChange
    {
        public RevokePolicy(CTPolicyCard _policy)
        {
            this.policy = _policy;
        }

        public CTPolicyCard policy;

        public override void ApplyChange(ref CTTurnData _year)
        {
            _year.revoked_policies.Add(policy);
        }
    }
}
