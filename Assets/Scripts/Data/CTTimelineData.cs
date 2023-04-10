using System.Collections.Generic;

namespace CT.Data
{
    using Lookup;
    using System;
    using Data.Changes;
    using FMOD;

    public class CTTimelineData
    {
        public List<CTChange>[] user_changes;
        public List<CTChange>[] game_changes;
        CTYearData initial_year;

        private CTTimelineData() { }

        public CTTimelineData(uint _start_year, uint _end_year, List<CTChange>[] _user_changes, List<CTChange>[] _game_changes)
        {
            if (_start_year >= _end_year)
                throw new ArgumentException("CTTimelineData.Initialise: Cannot assign the same start year and end year!");

            uint total_years = _end_year - _start_year;

            user_changes = _user_changes;
            game_changes = _game_changes;

            // Set initial year with base DataSheet values
            initial_year = new CTYearData();
            initial_year.Initialise(
                DataSheet.starting_money,
                DataSheet.starting_science,
                DataSheet.starting_food,
                DataSheet.starting_population);
        }

        public CTYearData GetYearData(uint _year)
        {
            CTYearData ret = initial_year;

            for (int i = 0; i < _year; i++)
            {
                // Disaster instances for year
                foreach (CTChange change in game_changes[i])
                {
                    change.ApplyChange(ref ret);
                }
                // Technology changes for year
                foreach (CTChange change in user_changes[i])
                {
                    change.ApplyChange(ref ret);
                }

                // Apply net resource worth of each assigned population member for each turn between zero and requested turn
                CTCost net_total = new CTCost(0, 0, 0, 0);
                net_total += (DataSheet.farmers_net * ret.Farmers);
                net_total += (DataSheet.worker_net * ret.Workers);
                net_total += (DataSheet.scientist_net * ret.Scientists);
                net_total += (DataSheet.planners_net * ret.Planners);
                net_total += (DataSheet.unemployed_net * ret.UnassignedPopulation);

                ret.ApplyCosts(net_total);

                //ret.ApplyCosts(DataSheet.farmers_net * ret.Farmers);
                //ret.ApplyCosts(DataSheet.worker_net * ret.Workers);
                //ret.ApplyCosts(DataSheet.scientist_net * ret.Scientists);
                //ret.ApplyCosts(DataSheet.planners_net * ret.Planners);
                //ret.ApplyCosts(DataSheet.unemployed_net * ret.UnassignedPopulation);
            }

            return ret;
        }


        #region Actions

        // Buy technology
        public void BuyTech(int _year, CTTechnologies _tech)
        {
            if (_year < 0)
                throw new ArgumentException("Year cannot be zero or lower!");

            user_changes[_year].Add(new PurchaseTechnology(_tech));
        }

        // Set Policy
        public void ApplyPolicy(int _year, CTPolicies _policy)
        {
            if (_year < 0)
                throw new ArgumentException("Year cannot be zero or lower!");

            user_changes[_year].Add(new SetPolicy(_policy));
        }

        public void RevokePolicy(int _year, CTPolicies _policy)
        {
            if (_year < 0)
                throw new ArgumentException("Year cannot be zero or lower!");

            user_changes[_year].Remove(new SetPolicy(_policy));
        }

        // Set faction distribution
        /// <summary>
        /// Takes floats for each faction percentage. 
        /// Floats should not total to greater than 1
        /// </summary>
        public void ChangePopulationDistribution(uint _year, float _workers, float _scientists, float _farmers, float _planners)
        {
            if (_year < 0)
                throw new ArgumentException("Year cannot be zero or lower!");

            user_changes[_year].Add(new SetFactionDistribution(_workers, _scientists, _farmers, _planners));
        }

        public void ApplyDisasterEffect(int _year, Disaster _disaster)
        {
            if (_year < 0)
                throw new ArgumentException("Year cannot be zero or lower!");

            game_changes[_year].Add(new ApplyDisaster(_disaster));
        }

        #endregion


    }
}

