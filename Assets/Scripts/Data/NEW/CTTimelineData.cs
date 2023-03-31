using System.Collections.Generic;
using UnityEngine;

namespace CT.Data
{
    using Enumerations;
    using Technologies;
    using System;
    using Data.Changes;
    using System.CodeDom;
    using OpenCover.Framework.Model;

    public class CTTimelineData
    {
        public List<CTChange>[] user_changes;
        public List<CTChange>[] game_changes;
        CTYearData initial_year;

        public void Initialise(uint _start_year, uint _end_year)
        {
            if (_start_year >= _end_year)
                throw new ArgumentException("CTTimelineData.Initialise: Cannot assign the same start year and end year!");

            uint total_years = _end_year - _start_year;
            //Debug.Log($"Total years : {total_years}");

            user_changes = new List<CTChange>[total_years];
            for (uint year = 0; year < total_years; year++)
            {
                user_changes[year] = new List<CTChange>();
            }

            game_changes = new List<CTChange>[total_years];
            for (uint year = 0; year < total_years; year++)
            {
                game_changes[year] = new List<CTChange>();
            }

            // Set initial year with base DataSheet values
            initial_year = new CTYearData();
            initial_year.Initialise(
                DataSheet.starting_money,
                DataSheet.starting_science,
                DataSheet.starting_food,
                DataSheet.starting_population);
        }

        public CTYearData GetYearData(int _year)
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

                // Apply base upkeep costs from factions
                ret.ApplyCosts(DataSheet.worker_net * ret.Workers);
                ret.ApplyCosts(DataSheet.scientist_net * ret.Scientists);
                ret.ApplyCosts(DataSheet.farmers_net * ret.Farmers);
                ret.ApplyCosts(DataSheet.planners_net * ret.Planners);
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

        // Set faction distribution
        /// <summary>
        /// Takes floats for each faction percentage. 
        /// Floats should not total to greater than 1
        /// </summary>
        public void ChangePopulationDistribution(int _year, float _workers, float _scientists, float _farmers, float _planners)
        {
            if (_year < 0)
                throw new ArgumentException("Year cannot be zero or lower!");

            user_changes[_year].Add(new SetFactionDistribution(_workers, _scientists, _farmers, _planners));
        }

        public void ApplyDisasterEffect(int _year, CTDisasters _disaster)
        {
            if (_year < 0)
                throw new ArgumentException("Year cannot be zero or lower!");

            game_changes[_year].Add(new ApplyDisaster(_disaster));
        }

        #endregion


    }
}

