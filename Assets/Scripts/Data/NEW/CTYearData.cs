using System.Collections.Generic;
using UnityEngine;

namespace CT.Data
{
    using System;
    using Lookup;

    public class CTYearData
    {
        // Debugging
        //public int year;
        public bool year_changed_by_player = false;

        // Technologies
        public Dictionary<CTTechnologies, bool> active_technologues;

        // Policies
        public Dictionary<CTPolicies, bool> active_policies;

        #region Resources

        // Money
        private int money;
        public int Money
        {
            get { return money; }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Money cannot go below zero!");
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
                if (value < 0)
                {
                    throw new ArgumentException("Science cannot go below zero!");
                }

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
                if (value < 0)
                {
                    throw new ArgumentException("Food cannot go below zero!");
                }

                food = value;
            }
        }

        private int surplus_food;
        public int SurplusFood
        {
            get { return food; }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Food cannot go below zero!");
                }

                food = value;
            }
        }

        // Population 
        private int population;
        public int Population
        {
            get { return population; }
            set
            {
                // If value is less than assigned population is it implicitly less than total population
                // An error must be thrown if population is set to less than the assigned workers
                // Could cause issues when killing population?
                if (value < 0)
                {
                    throw new ArgumentException("CTPopulation.resource_total.set: Total population cannot be negative!");
                }

                if (value < AssignedPopulation)
                {
                    while(value < AssignedPopulation)
                    {
                        int oingo = UnityEngine.Random.Range(1, 5);

                        try
                        {
                            switch (oingo)
                            {
                                case 1:
                                    Workers--;
                                    break;
                                case 2:
                                    Scientists--;
                                    break;
                                case 3:
                                    Farmers--;
                                    break;
                                case 4:
                                    Planners--;
                                    break;
                            }
                        }
                        catch 
                        {
                            continue;
                        }
                        population--;
                    }
                }
                else
                {
                    this.population = value;
                }
            }
        }


        #region Population Budget Readonly
        // Read only variables
        private int AssignedPopulation
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
        private int UnassignedPopulation
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

        #endregion



        #region Methods
        public void Initialise(int _money, int _science, int _food, int _pop)
        {
            Money = _money;
            Science = _science;
            Food = _food;
            Population = _pop;
            Workers = _pop / 4;
            Scientists = _pop / 4;
            Farmers = _pop / 4;
            Planners = _pop / 4;

            Debug.Log(Population);

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
            Money -= _cost.money;

            Science -= _cost.science;

            Food -= _cost.food;

            Population -= _cost.population;
        }
        #endregion
    }
}