using System.Collections.Generic;
using UnityEngine;
using System;

namespace CT.Data
{
    using CT.Enumerations;
    using Lookup;

    public class CTTurnData
    {

        public CTTurnData() { }

        public CTTurnData(CTTurnData _data) 
        {
            turn                    = _data.turn;
            Money                   = _data.Money;
            Science                 = _data.Science;
            Food                    = _data.Food;
            Population              = _data.Population;
            Awareness               = _data.Awareness;

            applied_policies        =  new List<CTPolicyCard>(_data.applied_policies);
            revoked_policies        =  new List<CTPolicyCard>(_data.revoked_policies);

            technologies            =  new Dictionary<CTTechnologies, bool>(_data.technologies);

            faction_distribution    =  new Vector4( _data.faction_distribution.x, 
                                                    _data.faction_distribution.y, 
                                                    _data.faction_distribution.z, 
                                                    _data.faction_distribution.w);
        }
        
        // Debugging
        public uint turn;
        public bool failed_turn { get { return (Population <= 0); } }

        private Vector4 faction_distribution = new Vector4();

        // Technologies
        public Dictionary<CTTechnologies, bool> technologies = new Dictionary<CTTechnologies, bool>();

        // Policies
        public List<CTPolicyCard> applied_policies = new List<CTPolicyCard>();
        public List<CTPolicyCard> revoked_policies = new List<CTPolicyCard>();

        private Vector4 cost_modifier_totals = new Vector4(1, 1, 1, 1);

        private bool modified = false;

        //private float safety_factor = 1.0f;

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
                    data_food = 0;
                else
                    data_food = value;
            }
        }

        // Population 
        private float data_population = DataSheet.STARTING_POPULATION;
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
        public void ApplyCosts(CTCost _cost, CTCostType _type)
        {
            if (Population == 0) return;

            switch (_type)
            {
                // Upkeep
                // Apply costs with modifiers x, y, z

                case CTCostType.Upkeep:
                    Money       -= (int)(_cost.money        * (cost_modifier_totals.x));
                    Science     -= (int)(_cost.science      * (cost_modifier_totals.y));
                    Food        -= (int)(_cost.food         * (cost_modifier_totals.z));
                    Population  -= (int)_cost.population;
                    break;

                // Purchase
                // Apply costs with no modifiers
                case CTCostType.Purchase:
                    Money       -= (int)_cost.money;
                    Science     -= (int)_cost.science;
                    Food        -= (int)_cost.food;
                    Population  -= (int)_cost.population;
                    break;

                // Disaster
                // Apply costs with all modifiers
                case CTCostType.Disaster:
                    float sf = Mathf.Abs(GetSafetyFactor());

                    Money       -= (int)((Money *        _cost.money)        * (sf));
                    Science     -= (int)((Science *      _cost.science)      * (sf));
                    Food        -= (int)((Food *         _cost.food)         * (sf));
                    Population  -= (int)((Population *   _cost.population)   * (sf));
                    break;

                default:
                    Debug.LogError("CTTurnData.ApplyCosts: Invalid CTCostType");
                    break;
            }
        }

        public void ApplyTechnology(CTTechnologies _tech)
        {
            technologies[_tech] = true;
        }

        public void SetFactionDistribution(Vector4 _dist)
        {
            faction_distribution = _dist;
        }

        public void ApplyModifiers()
        {
            //Debug.Log("CTTurnData.ApplyModifiers");

            // Apply base safety modifier based on planner total
            //safety_factor = (Population * faction_distribution.w) * DataSheet.PLANNER_SAFETY_FACTOR;
            //Debug.Log(safety_factor);

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

            // Prevent overflow or costs become boons / free
            if (cost_modifier_totals.x < DataSheet.MAX_MODIFIER_REDUCTION) cost_modifier_totals.x = DataSheet.MAX_MODIFIER_REDUCTION; // Prevent money modifier becoming zero cost / bonus
            if (cost_modifier_totals.y < DataSheet.MAX_MODIFIER_REDUCTION) cost_modifier_totals.y = DataSheet.MAX_MODIFIER_REDUCTION; // Prevent science modifier becoming zero cost / bonus
            if (cost_modifier_totals.z < DataSheet.MAX_MODIFIER_REDUCTION) cost_modifier_totals.z = DataSheet.MAX_MODIFIER_REDUCTION; // Prevent food modifier becoming zero cost / bonus
            if (cost_modifier_totals.w < DataSheet.MAX_MODIFIER_REDUCTION) cost_modifier_totals.w = DataSheet.MAX_MODIFIER_REDUCTION; // Prevent safety modifier becoming zero cost / bonus

            //// If planners do not have enough science, half their effectiveness
            //if (Science < (Population * faction_distribution.w) * DataSheet.PLANNERS_NET.science)
            //    safety_factor *= 0.5f;

            modified = true;
        }

        private void OingoBoingo(BuffsNerfsType _t, float _degree)
        {
            //if (_degree == 0)
            //    return;

            //Debug.Log(_degree);

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
                    cost_modifier_totals.x += 1;
                    break;

                case CTModifierType.Science:
                    cost_modifier_totals.y += 1;
                    break;

                case CTModifierType.Food:
                    cost_modifier_totals.z += 1;
                    break;

                case CTModifierType.Population:
                    cost_modifier_totals.w += 1;
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

            Population += (int)(Population * DataSheet.FOOD_SURPLUS_POPULATION_GAIN);
            return;
        }

        public void DecayPopulation(float _required)
        {
            // Calculate delta in required vs available food
            float delta = Food - _required;
            float delta_scale = ScalePopulationStarvation(delta, Population);

            Population -= (int)(Population * (DataSheet.STARVATION_RATE * delta_scale) + 1);

            return;
        }

        public void SwapResources(CTResources _lhs, float _lhs_ratio, CTResources _rhs, float _rhs_ratio)
        {
            int lhs = -1;
            int rhs = -1;

            Vector4 temps = new Vector4(Money, Science, Food, Population);

            switch (_lhs)
            {
                case CTResources.Money:
                    lhs = 0;
                    break;
                case CTResources.Science:
                    lhs = 1;
                    break;
                case CTResources.Food:
                    lhs = 2;
                    break;
                case CTResources.Population:
                    lhs = 3;
                    break;
                default:
                    break;
            }

            switch (_rhs)
            {
                case CTResources.Money:
                    rhs = 0;
                    break;
                case CTResources.Science:
                    rhs = 1;
                    break;
                case CTResources.Food:
                    rhs = 2;
                    break;
                case CTResources.Population:
                    rhs = 3;
                    break;
                default:
                    break;
            }

            if (lhs == -1 || rhs == -1)
            {
                Debug.LogError("CTTurnData.SwapResources: Invalid resource type input!");
                return;
            }


            Vector4 ret = new Vector4(temps.x, temps.y, temps.z, temps.w);

            ret[lhs] = temps[rhs] * _rhs_ratio;
            ret[rhs] = temps[lhs] * _lhs_ratio;

            Money       = (int)ret.x;
            Science     = (int)ret.y;
            Food        = (int)ret.z;
            Population  = (int)ret.w;
        }

        public Vector4 GetFactionDistribution()
        {
            return faction_distribution;
        }

        public float GetSafetyFactor()
        {
            float ret = faction_distribution.w;

            if (Science < (Population * faction_distribution.w) * DataSheet.PLANNERS_NET.science)
                ret *= 0.5f;

            if (ret >= 0.95f)
                ret = 0.95f;

            return 1.0f - ret;
        }

        public void Logs()
        {
            //Debug.Log($"Cost Modifier: {cost_modifier_totals}");
            //Debug.Log($"Safety Factor: {safety_factor}");
        }

        #endregion
    }
}