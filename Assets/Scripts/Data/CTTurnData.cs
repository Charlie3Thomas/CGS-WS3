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
            technologies     =  new Dictionary<CTTechnologies, bool>(_data.technologies);
            applied_policies        =  new List<CTPolicyCard>(_data.applied_policies);
            revoked_policies        =  new List<CTPolicyCard>(_data.revoked_policies);
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
        public Dictionary<CTTechnologies, bool> technologies = new Dictionary<CTTechnologies, bool>();

        // Policies
        public List<CTPolicyCard> applied_policies = new List<CTPolicyCard>();
        public List<CTPolicyCard> revoked_policies = new List<CTPolicyCard>();

        private Vector4 cost_modifier_totals = new Vector4(0, 0, 0, 0);

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
            technologies = new Dictionary<CTTechnologies, bool>();
            foreach (CTTechnologies tech in (CTTechnologies[])System.Enum.GetValues(typeof(CTTechnologies)))
            {
                // Default each technology to false (not owned)
                technologies[tech] = false;
            }
        }

        #endregion


        #region Actions
        public void ApplyCosts(CTCost _cost)
        {
            if (Population == 0) return;

            Money       -= (int)(_cost.money        * (1 + cost_modifier_totals.x));
            Science     -= (int)(_cost.science      * (1 + cost_modifier_totals.y));
            Food        -= (int)(_cost.food         * (1 + cost_modifier_totals.z));
            Population  -= (int)(_cost.population   * (1 + cost_modifier_totals.w));
        }

        public void ApplyTechnology(CTTechnologies _tech)
        {
            technologies[_tech] = true;
        }

        public void ApplyModifiers()
        {
            // Technologies
            foreach (KeyValuePair<CTTechnologies, bool> kvp in technologies)
            {
                if (kvp.Value) // If you have the technology this turn
                {
                    // Get the buff/nerf associated with the technology
                    BuffsNerfs bn = DataSheet.technology_buffs[kvp.Key];

                    // Loop through buff/nerf type
                    for (int bnt = 0; bnt < bn.type.Count; bnt++)
                    {
                        OingoBoingo(bn.type[bnt], bn.amount[bnt]);
                    }
                }
            }

            // Policies
            foreach (CTPolicyCard pc_app in applied_policies)
            {
                bool pc_app_active = true;
                foreach (CTPolicyCard pc_rev in revoked_policies)
                {
                    // Check if IDs match, and if so don't apply buffs
                    if (pc_app.ID ==  pc_rev.ID)
                    {
                        pc_app_active = false;
                        break;
                    }
                }

                SetFactionDistribution current = new SetFactionDistribution(faction_distribution.x,
                                                                            faction_distribution.y,
                                                                            faction_distribution.z,
                                                                            faction_distribution.w);

                if (!(current >= pc_app.fdist))
                    return;

                if (pc_app_active)
                {
                    // For each buff
                    foreach (KeyValuePair<BuffsNerfsType, bool> kvp in  pc_app.buffs)
                    {
                        if (kvp.Value)
                            OingoBoingo(kvp.Key, pc_app.buff_nerf_scale[kvp.Key]);
                    }

                    // For each nerf
                    foreach (KeyValuePair<BuffsNerfsType, bool> kvp in pc_app.debuffs)
                    {
                        if (kvp.Value)
                            OingoBoingo(kvp.Key, pc_app.buff_nerf_scale[kvp.Key]);
                    }
                }
            }
        }

        private void OingoBoingo(BuffsNerfsType _t, float _degree)
        {
            switch (_t)
            {
                case BuffsNerfsType.MONEY_GAIN:
                    ApplyCTCostModifer(CTModifierType.Money, _degree);
                    break;

                case BuffsNerfsType.FOOD_GAIN:
                    ApplyCTCostModifer(CTModifierType.Food, _degree);
                    break;

                case BuffsNerfsType.SCIENCE_GAIN:
                    ApplyCTCostModifer(CTModifierType.Science, _degree);
                    break;   
                    
                case BuffsNerfsType.MONEY_UPKEEP:
                    ApplyCTCostModifer(CTModifierType.Money, _degree);
                    break;    
                    
                case BuffsNerfsType.FOOD_UPKEEP:
                    ApplyCTCostModifer(CTModifierType.Food, _degree);
                    break;    
                    
                case BuffsNerfsType.SCIENCE_UPKEEP:
                    ApplyCTCostModifer(CTModifierType.Science, _degree);
                    break;   
                    
                case BuffsNerfsType.SAFETY_FACTOR:
                    break;  
                    
                case BuffsNerfsType.MONEY_BONUS:
                    ApplyFlatValue(CTModifierType.Money, _degree);
                    break; 
                    
                case BuffsNerfsType.SCIENCE_BONUS:
                    ApplyFlatValue(CTModifierType.Science, _degree);
                    break; 
                    
                case BuffsNerfsType.AWARENESS_FACTOR:
                    break;
            }
        }

        // Apply flat value
        private void ApplyFlatValue(CTModifierType _t, float _value)
        {
            if (Population == 0) return;

            switch (_t)
            {
                case CTModifierType.Money:
                    Money += (int)_value;
                    break;

                case CTModifierType.Science:
                    Science += (int)_value;
                    break;

                case CTModifierType.Food:
                    Food += (int)_value;
                    break;

                case CTModifierType.Population:
                    Population += (int)_value;
                    break;

                case CTModifierType.Awareness:
                    throw new NotImplementedException();

                case CTModifierType.Safety:
                    throw new NotImplementedException();

                default:
                    Debug.LogError($"CTTurnData.ApplyFlatValue({_t}, {_value}) is not an implemented flat value modifier type!");
                    break;
            }
        }

        // Apply modifier to costs
        private void ApplyCTCostModifer(CTModifierType _t, float _value)
        {
            switch (_t)
            {
                case CTModifierType.Money:
                    cost_modifier_totals.x += _value;
                    break;

                case CTModifierType.Science:
                    cost_modifier_totals.y += _value;
                    break;

                case CTModifierType.Food:
                    cost_modifier_totals.z += _value;
                    break;

                case CTModifierType.Population:
                    cost_modifier_totals.w += _value;
                    break;

                default:
                    Debug.LogError($"CTTurnData.ApplyFlatValue({_t}, {_value}) is not an implemented CTCost modifier type!");
                    break;
            }
        }

        // Modify disaster impact

        // Modify awareness

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