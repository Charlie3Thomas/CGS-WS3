using System.Collections.Generic;
using UnityEngine;
using System;

namespace CT.Data
{
    using Changes;
    using Enumerations;
    using Lookup;

    public class CTTurnData
    {

        public CTTurnData() { }

        public CTTurnData(CTTurnData _data) 
        { 
            turn                    = _data.turn;
            active_technologues     = _data.active_technologues;
            applied_policies        = _data.applied_policies;
            revoked_policies        = _data.revoked_policies;
            modifiers               = _data.modifiers;
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

        public Vector4 faction_distribution;

        // Technologies
        public Dictionary<CTTechnologies, bool> active_technologues;

        // Policies
        public List<CTPolicyCard> applied_policies = new List<CTPolicyCard>();
        public List<CTPolicyCard> revoked_policies = new List<CTPolicyCard>();

        public CTModifiers modifiers;


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

                //Debug.Log(value);
                if (value < 0)
                {
                    data_money = 0;
                    //throw new ArgumentException("Money cannot go below zero!");
                }

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
                if (value < data_science)
                    return;
                //Debug.Log(value);
                if (value < 0)
                {
                    data_science = 0;
                    //throw new ArgumentException("Science cannot go below zero!");
                }
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

                // Growth
                if (value > Population)
                {
                    Population += (int)(Population * DataSheet.food_surplus_population_gain);
                    //Debug.Log("Population growth!");
                }

                // Decay
                if (value <= 0)
                {
                    if (value == 0)
                    { // If set to exactly zero, there was JUST enough food
                        data_food = value;
                        return;
                    }
                    else
                    { // If set to less than zero, there was not enough food
                        data_food = 0;

                        // Calculate delta in required vs available food
                        float delta_scale = ScalePopulationStarvation(value, Population);

                        Population -= (int)(Population * (DataSheet.starvation_rate * delta_scale) + 1);

                        return;
                    }                    
                }

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
        private float assigned_workers;
        public int Workers
        {
            get { return (int)(Population * faction_distribution.x); }
            //set
            //{
            //    // If the increase in assigned is greater than the unassigned throw exception
            //    if (value > UnassignedPopulation + assigned_workers)
            //    {
            //        Debug.Log($"Value is {value}, Total available is is {UnassignedPopulation + assigned_workers} in turn {turn}");
            //        throw new ArgumentException("CTPopulation.assigned_workers.set: Assigned workers cannot be greater than unassigned population!");
            //    }
            //    assigned_workers = value;
            //}
        }

        private float assigned_scientists;
        public int Scientists
        {
            get { return (int)(Population * faction_distribution.y); }
            //set
            //{
            //    // If the increase in assigned is greater than the unassigned throw exception
            //    if (value > UnassignedPopulation + assigned_scientists)
            //    {
            //        Debug.Log($"Value is {value}, Total available is is {UnassignedPopulation + assigned_scientists} in turn {turn}");
            //        throw new ArgumentException("CTPopulation.assigned_scientists.set: Assigned scientists cannot be greater than unassigned population!");
            //    }
            //    assigned_scientists = value;
            //}
        }

        private float assigned_farmers;
        public int Farmers
        {
            get { return (int)(Population * faction_distribution.z); }
            //set
            //{
            //    // If the increase in assigned is greater than the unassigned throw exception
            //    if (value > UnassignedPopulation + assigned_farmers)
            //    {
            //        Debug.Log($"Value is {value}, Total available is is {UnassignedPopulation + assigned_farmers}");
            //        throw new ArgumentException("CTPopulation.assigned_farmers.set: Assigned farmers cannot be greater than unassigned population!");
            //    }
            //    assigned_farmers = value;
            //}
        }

        private float assigned_emergency;
        public int Planners
        {
            get { return (int)(Population * faction_distribution.w); }
            //set
            //{
            //    // If the increase in assigned is greater than the unassigned throw exception
            //    if (value > UnassignedPopulation + assigned_emergency)
            //    {
            //        Debug.Log($"Value is {value}, Total available is is {UnassignedPopulation + assigned_emergency}");
            //        throw new ArgumentException("CTPopulation.assigned_emergency.set: Assigned emergency cannot be greater than unassigned population!");
            //    }
            //    assigned_emergency = value;
            //}
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

            //Workers     = (int)(Population * faction_distribution.x);
            //Scientists  = (int)(Population * faction_distribution.y);
            //Farmers     = (int)(Population * faction_distribution.z);
            //Planners    = (int)(Population * faction_distribution.w);

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
            Money -= _cost.money;

            //Debug.Log("Science " + _cost.science);
            Science -= _cost.science;

            //Debug.Log("Food " + _cost.food);
            Food -= _cost.food;

            //Debug.Log("Pop " + _cost.population);
            Population -= _cost.population;
        }

        #endregion


        #region Utility
        //private float GetFactionRatio(CTFaction _type)
        //{
        //    switch (_type)
        //    {
        //        case CTFaction.Scientist:
        //            return (float)Scientists / (float)Population;
        //        case CTFaction.Worker:
        //            return (float)Workers / (float)Population;
        //        case CTFaction.Planner:
        //            return (float)Planners / (float)Population;
        //        case CTFaction.Farmer:
        //            return (float)Farmers / (float)Population;
        //        default:
        //            return -1.0f;
        //    }
        //}

        private float ScalePopulationStarvation(float _v, float _p)
        {
            float ret = (Mathf.Abs(_v) / _p);
            if (ret > 1.0f)
                ret = 1.0f;
            return ret;
        }

        #endregion
    }
}