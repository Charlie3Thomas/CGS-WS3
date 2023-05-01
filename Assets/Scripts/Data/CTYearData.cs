using System.Collections.Generic;
using UnityEngine;
using System;

namespace CT.Data
{
    using Enumerations;
    using Lookup;

    public class CTYearData
    {
        // Debugging
        public uint turn;
        public bool year_changed_by_player = false;

        // Technologies
        public Dictionary<CTTechnologies, bool> active_technologues;

        // Policies
        public List<CTPolicyCard> applied_policies;
        public List<CTPolicyCard> revoked_policies;

        public CTModifiers modifiers;


        #region Resources

        #region Consumables
        // Money
        private int money;
        public int Money
        {
            get { return money; }

            set
            {
                if (value == money)
                    return;

                //Debug.Log(value);
                if (value < 0)
                {
                    money = 0;
                    //throw new ArgumentException("Money cannot go below zero!");
                }

                money = value;
            }
        }

        // Science
        private int science;
        public int Science
        {
            get { return science; }

            set
            {
                if (value < science)
                    return;
                //Debug.Log(value);
                if (value < 0)
                {
                    science = 0;
                    //throw new ArgumentException("Science cannot go below zero!");
                }
                else
                    science = value;

            }
        }

        // Food
        private int food;
        public int Food
        {
            get { return food; }

            set
            {
                if (value == food)
                    return;
                // Starvation
                if (value < 0)
                {
                    food = 0;
                    float adjusted = ScalePopulationStarvation(value, Population);
                    Population -= (int)(Population * (DataSheet.starvation_rate *  adjusted));
                    Debug.Log("Population is starving!");
                    //throw new ArgumentException("Food cannot go below zero!");
                }
                else
                {
                    food = value;
                }

                // Growth
                if (value > Population)
                {
                    Population += (int)(Population * DataSheet.food_surplus_population_gain);
                    Debug.Log("Population growth!");
                }
            }
        }

        private int surplus_food;
        public int SurplusFood
        {
            get { return surplus_food; }

            set
            {
                if (value < surplus_food)
                    return;

                Debug.Log(value);
                if (value < 0)
                {
                    surplus_food = 0;
                    throw new ArgumentException("Food cannot go below zero!");
                }

                food = value;
            }
        }

        // Population 
        private int population = DataSheet.starting_population;
        public int Population
        {
            get { return population; }
            set
            {
                if (value == population) 
                    return;

                // If value is less than assigned population is it implicitly less than total population
                // An error must be thrown if population is set to less than the assigned workers
                // Could cause issues when killing population?
                if (value < 0)
                {
                    throw new ArgumentException("CTPopulation.resource_total.set: Total population cannot be negative!");
                }

                if (value < AssignedPopulation)
                {
                    float planner_ratio = GetFactionRatio(CTFaction.Planner);
                    float farmer_ratio = GetFactionRatio(CTFaction.Farmer);
                    float worker_ratio = GetFactionRatio(CTFaction.Worker);
                    float scientist_ratio = GetFactionRatio(CTFaction.Scientist);

                    Planners = 0;
                    Farmers = 0;
                    Workers = 0;
                    Scientists = 0;

                    Population = value;

                    Planners = (int)(Population * planner_ratio);
                    Farmers = (int)(Population * farmer_ratio);
                    Workers = (int)(Population * worker_ratio);
                    Scientists = (int)(Population * scientist_ratio);

                    Population -= UnassignedPopulation;

                    /*
                    while (value < AssignedPopulation)
                    {
                        int oingo = UnityEngine.Random.Range(1, 5);

                        try
                        {
                            switch (oingo)
                            {
                                case 1:
                                    if (Workers > 0)
                                        Workers--;
                                    else
                                        continue;
                                    break;

                                case 2:
                                    if (Scientists > 0)
                                        Scientists--;
                                    else
                                        continue;
                                    break;

                                case 3:
                                    if (Farmers > 0)
                                        Farmers--;
                                    else
                                        continue;
                                    break;

                                case 4:
                                    if (Planners > 0)
                                        Planners--;
                                    else
                                        continue;
                                    break;

                                default:
                                    break;
                            }
                        }
                        catch 
                        {
                            continue;
                        }
                        population--;
                    }
                    */
                }
                else
                {
                    if (value == 0)
                        return;

                    float planner_ratio = GetFactionRatio(CTFaction.Planner);
                    float farmer_ratio = GetFactionRatio(CTFaction.Farmer);
                    float worker_ratio = GetFactionRatio(CTFaction.Worker);
                    float scientist_ratio = GetFactionRatio(CTFaction.Scientist);

                    Planners = 0;
                    Farmers = 0;
                    Workers = 0;
                    Scientists = 0;

                    this.population = value;

                    Planners = (int)(Population * planner_ratio);
                    Farmers = (int)(Population * farmer_ratio);
                    Workers = (int)(Population * worker_ratio);
                    Scientists = (int)(Population * scientist_ratio);
                }
            }
        }
        #endregion


        #region Population Budget Readonly
        // Read only variables
        public int AssignedPopulation
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
            get { return population - AssignedPopulation; }

        }
        #endregion


        #region Types of Population
        private int assigned_workers;
        public int Workers
        {
            get { return assigned_workers; }
            set
            {
                // If the increase in assigned is greater than the unassigned throw exception
                if (value > UnassignedPopulation + assigned_workers)
                {
                    Debug.Log($"Value is {value}, Total available is is {UnassignedPopulation + assigned_workers}");
                    throw new ArgumentException("CTPopulation.assigned_workers.set: Assigned workers cannot be greater than unassigned population!");
                }
                assigned_workers = value;
            }
        }

        private int assigned_farmers;
        public int Farmers
        {
            get { return assigned_farmers; }
            set
            {
                // If the increase in assigned is greater than the unassigned throw exception
                if (value > UnassignedPopulation + assigned_farmers)
                {
                    Debug.Log($"Value is {value}, Total available is is {UnassignedPopulation + assigned_farmers}");
                    throw new ArgumentException("CTPopulation.assigned_farmers.set: Assigned farmers cannot be greater than unassigned population!");
                }
                assigned_farmers = value;
            }
        }

        private int assigned_scientists;
        public int Scientists
        {
            get { return assigned_scientists; }
            set
            {
                // If the increase in assigned is greater than the unassigned throw exception
                if (value > UnassignedPopulation + assigned_scientists)
                {
                    Debug.Log($"Value is {value}, Total available is is {UnassignedPopulation + assigned_scientists}");
                    throw new ArgumentException("CTPopulation.assigned_scientists.set: Assigned scientists cannot be greater than unassigned population!");
                }
                assigned_scientists = value;
            }
        }

        private int assigned_emergency;
        public int Planners
        {
            get { return assigned_emergency; }
            set
            {
                // If the increase in assigned is greater than the unassigned throw exception
                if (value > UnassignedPopulation + assigned_emergency)
                {
                    Debug.Log($"Value is {value}, Total available is is {UnassignedPopulation + assigned_emergency}");
                    throw new ArgumentException("CTPopulation.assigned_emergency.set: Assigned emergency cannot be greater than unassigned population!");
                }
                assigned_emergency = value;
            }
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

            if (turn == 0)
            {
                //Debug.Log($"Setting base faction distribution for turn {turn}");
                Workers = (int)(_pop * DataSheet.starting_workers);
                Scientists = (int)(_pop * DataSheet.starting_scientists);
                Farmers = (int)(_pop * DataSheet.starting_farmers);
                Planners = (int)(_pop * DataSheet.starting_planners);
            }


            //Debug.Log(Population);

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
        private float GetFactionRatio(CTFaction _type)
        {
            switch (_type)
            {
                case CTFaction.Scientist:
                    return (float)Scientists / (float)Population;
                case CTFaction.Worker:
                    return (float)Workers / (float)Population;
                case CTFaction.Planner:
                    return (float)Planners / (float)Population;
                case CTFaction.Farmer:
                    return (float)Farmers / (float)Population;
                default:
                    return -1.0f;
            }
        }

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