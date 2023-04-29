using System.Collections.Generic;
using UnityEngine;
using System;

namespace CT.Data
{
    using Changes;
    using Enumerations;
    using Lookup;
    using Newtonsoft.Json.Schema;
    using static UnityEngine.Rendering.DebugUI;

    public class CTTurnData
    {

        public CTTurnData() { }

        public CTTurnData(CTTurnData _data) 
        {
            turn                    = _data.turn;
            active_technologues     =  new Dictionary<CTTechnologies, bool>(_data.active_technologues);
            applied_policies        = _data.applied_policies;
            revoked_policies        = _data.revoked_policies;
            Money                   = _data.Money;
            Science                 = _data.Science;
            Food                    = _data.Food;
            faction_distribution    = _data.faction_distribution;
            Population              = _data.Population;
            Awareness               = _data.Awareness;
        }
        
        // Debugging
        public uint turn;
        public bool failed_turn { get { return (Population <= 0); } }

        public Vector4 faction_distribution = new Vector4();

        // Technologies
        public Dictionary<CTTechnologies, bool> active_technologues = new Dictionary<CTTechnologies, bool>();

        // Policies
        public List<CTPolicyCard> applied_policies = new List<CTPolicyCard>();
        public List<CTPolicyCard> revoked_policies = new List<CTPolicyCard>();

        private float food_delta = 0;

        #region Resources

        #region Consumables
        // Money
        private float data_money;
        public int Money
        {
            get { return (int)data_money; }

            set
            {
                if (value == data_money)
                    return;

                if (value < 0)
                    data_money = 0;
                else
                    data_money = value;
            }
        }

        // Science
        private float data_science;
        public int Science
        {
            get { return (int)data_science; }

            set
            {
                if (value == data_science)
                    return;

                if (value < 0)
                    data_science = 0;
                else
                    data_science = value;
            }
        }

        // Food
        private float data_food;
        public int Food
        {
            get { return (int)data_food; }

            set
            {
                if (value == data_food)
                    return;

                if (value < 0)
                {
                    food_delta = value;
                    data_food = 0;
                }
                else
                    data_food = value;
            }
        }

        // Population 
        private float data_population = DataSheet.starting_population;
        public int Population
        {
            get { return (int)data_population; }
            set
            {
                if (value == data_population) 
                    return;

                // If value is less than assigned population is it implicitly less than total population
                // An error must be thrown if population is set to less than the assigned workers
                // Could cause issues when killing population?
                if (value <= 0)
                {
                    data_population = 0;
                    faction_distribution = new Vector4(0,0,0,0);
                    return;
                    //throw new ArgumentException("CTPopulation.resource_total.set: Total population cannot be negative!");
                }

                data_population = value;
            }
        }

        private float data_surplus_food;
        public int SurplusFood
        {
            get { return (int)data_surplus_food; }

            set
            {
                if (value < data_surplus_food)
                    return;

                Debug.Log(value);
                if (value < 0)
                {
                    data_surplus_food = 0;
                    throw new ArgumentException("Food cannot go below zero!");
                }

                data_food = value;
            }
        }
        #endregion


        #region Population Budget Readonly
        // Read only variables
        public float AssignedPopulation
        {
            get
            {
                return
                    Workers +
                    Farmers +
                    Scientists +
                    Planners;
            }
        }
        public int UnassignedPopulation
        {
            get { return (int)(Population - AssignedPopulation); }

        }
        #endregion


        #region Types of Population
        public int Workers
        {
            get { return (int)(Population * faction_distribution.x); }
        }

        public int Scientists
        {
            get { return (int)(Population * faction_distribution.y); }
        }

        public int Farmers
        {
            get { return (int)(Population * faction_distribution.z); }
        }

        public int Planners
        {
            get { return (int)(Population * faction_distribution.w); }
        }
        #endregion

        private float awareness;
        public float Awareness
        {
            get 
            {
                return awareness;
            }
            set 
            {
                if (value == awareness)
                    return;

                awareness = value;
            }
        }

        #endregion


        #region Methods
        public void Initialise(int _money, int _science, int _food, int _pop)
        {
            Money = _money;
            Science = _science;
            Food = _food;
            Population = _pop;

            // Initialise active_technologues and assign Keys for each Enum type in CTTechnologies
            active_technologues = new Dictionary<CTTechnologies, bool>();
            foreach (CTTechnologies tech in (CTTechnologies[])System.Enum.GetValues(typeof(CTTechnologies)))
            {
                // Default each technology to false (not owned)
                active_technologues[tech] = false;
            }
        }

        #endregion


        #region Actions
        public void ApplyCosts(CTCost _cost)
        {
            //Debug.Log("Money " + _cost.money);
            Money -= (int)_cost.money;

            //Debug.Log("Science " + _cost.science);
            Science -= (int)_cost.science;

            //Debug.Log("Food " + _cost.food);
            Food -= (int)_cost.food;

            //Debug.Log("Pop " + _cost.population);
            Population -= (int)_cost.population;
        }

        public void ApplyTechnology(CTTechnologies _tech)
        {
            active_technologues[_tech] = true;
        }

        #endregion


        #region Utility

        private float ScalePopulationStarvation(float _v, float _p)
        {
            float ret = (Mathf.Abs(_v) / _p);
            if (ret > 1.0f)
                ret = 1.0f;
            return ret;
        }

        public void GrowPopulation(int _i)
        {
            if (_i == 0)
                return;

            Population += (int)(Population * DataSheet.food_surplus_population_gain);
            return;
        }

        public void DecayPopulation()
        {
            // Calculate delta in required vs available food
            float delta_scale = ScalePopulationStarvation(food_delta, Population);

            Population -= (int)(Population * (DataSheet.starvation_rate * delta_scale) + 1);

            return;
        }

        #endregion
    }
}