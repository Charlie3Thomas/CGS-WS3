using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

/*
    SELECTOR ORDER:
    1. Scientist
    2. Planner
    3. Farmer
    4. Workers
 
 */

namespace CT
{
    using Data;
    using Data.Changes;
    using Data.Resources;
    using Lookup;
    using Enumerations;
    using UnityEngine.UIElements;
    using UnityEditor.Experimental.GraphView;
    using System.Linq.Expressions;

    public class GameManager : MonoBehaviour
    {
        public static GameManager _INSTANCE;

        // Changes
        private List<CTChange>[] game_changes;
        private List<CTChange>[] user_changes;
        private List<CTChange>[] awareness_changes;

        private static readonly CTTurnData initial_year = new CTTurnData();

        public CTTurnData turn_data = new CTTurnData();
        //private CTTimelineData prime_timeline;

        private uint current_turn = 0;
        public uint CurrentTurn { get { return current_turn; } }
        private int user_changes_in_turn;
        private Vector3 current_turn_resource_expenditure;

        private void Awake()
        {
            if (_INSTANCE != null)
            {
                Destroy(this.gameObject);
                Debug.LogError("Multiple GameManager Instances!");
            }
            else
                _INSTANCE = this;

            DontDestroyOnLoad(gameObject);
        }


        private void Start()
        {
            // Setup data containers
            Initialise();

            // Generate disasters and populate into data containers
            DisasterManager.instance.Generate();

            // AI generate turn data
            AIPlayFromTurn(0);

            // Set current turn data to year zero
            turn_data = GetYearData(0);

            // Setup UI
            UpdateResourceCounters();

            UpdateFactionDistributionPips();

            UpdatePipsWithCurrentTurnData();

            // Setup Tech Tree
            FindObjectOfType<TechTree>().GetComponent<TechTree>().ClearBuffs();

            FindObjectOfType<TechTree>().GetComponent<TechTree>().UpdateNodes();
        }

        private void FixedUpdate()
        {
            UpdateResourceCounters();
        }

        private void Initialise()
        {
            initial_year.Initialise(DataSheet.starting_money, DataSheet.starting_science, DataSheet.starting_food, DataSheet.starting_population);

            // Initialise user changes list
            user_changes = new List<CTChange>[DataSheet.turns_number + 1];
            for (uint year = 0; year < user_changes.Length; year++)
                user_changes[year] = new List<CTChange>();

            // Initialise game changes list
            game_changes = new List<CTChange>[DataSheet.turns_number + 1];
            for (uint year = 0; year < game_changes.Length; year++)
                game_changes[year] = new List<CTChange>();

            // Initialise awareness changes list
            awareness_changes = new List<CTChange>[DataSheet.turns_number + 1];
            for(uint year = 0; year < awareness_changes.Length; year++)
                awareness_changes[year] = new List<CTChange>();

            current_turn_resource_expenditure = new Vector3(0, 0, 0);
        }

        private CTTurnData GetYearData(uint _year)
        {
            CTTurnData ret = new CTTurnData(initial_year);

            ret.turn = _year;

            for (int i = 0; i <= _year; i++)
            {
                // Disaster instances for year
                foreach (CTChange change in game_changes[i])
                    change.ApplyChange(ref ret);

                // Technology changes for year
                foreach (CTChange change in user_changes[i])
                    change.ApplyChange(ref ret);

                // Get Total Modifiers for turn
                ret.ApplyModifiers();

                // Apply net resource worth of each assigned population member for each turn between zero and requested turn
                CTCost net_total = new CTCost(0, 0, 0, 0);
                net_total += (DataSheet.worker_net * ret.Workers);
                net_total += (DataSheet.scientist_net * ret.Scientists);
                net_total += (DataSheet.farmers_net * ret.Farmers);
                net_total += (DataSheet.planners_net * ret.Planners);
                net_total += (DataSheet.unemployed_net * ret.UnassignedPopulation);

                if (ret.Food >= ret.Population) { ret.GrowPopulation(i); }
                else { ret.DecayPopulation(); }

                ret.ApplyCosts(net_total);
            }

            return ret;
        }

        private CTTurnData GetNextTurnData(CTTurnData _current_turn)
        {
            CTTurnData ret = new CTTurnData(_current_turn);

            ret.turn++;

            // Disaster instances for year
            foreach (CTChange change in game_changes[ret.turn])
                change.ApplyChange(ref ret);

            // Technology changes for year
            foreach (CTChange change in user_changes[ret.turn])
                change.ApplyChange(ref ret);

            // Apply net resource worth of each assigned population member for each turn between zero and requested turn
            CTCost net_total = new CTCost(0, 0, 0, 0);
            net_total += (DataSheet.worker_net * ret.Workers);
            net_total += (DataSheet.scientist_net * ret.Scientists);
            net_total += (DataSheet.farmers_net * ret.Farmers);
            net_total += (DataSheet.planners_net * ret.Planners);
            net_total += (DataSheet.unemployed_net * ret.UnassignedPopulation);

            ret.ApplyCosts(net_total);

            return ret;
        }

        public void OnClickCheckoutYearButton(uint _requested_turn)
        {
            //Debug.Log("GameManager.OnClickCheckoutYearButton" + _year);

            // Don't allow user to checkout year if the requested turn is the current turn
            if (_requested_turn == current_turn)
                return;

            // Lock in changes to faction distribution
            SetFactionDistribution();

            // If player made changes to a turn
            if (WereChangesMadeInTurn())
            {
                // Adjust awareness
                awareness_changes[current_turn].Add(new TrackAwareness(DataSheet.year_change_awareness_rate));

                // Recalculate AI turns
                AIPlayFromTurn(current_turn);
            }

            current_turn = _requested_turn;

            CheckAllUserTechPurchasesValid();

            //GetChangesAtTurn();
            turn_data = GetYearData(_requested_turn);
            current_turn_resource_expenditure = new Vector3(0, 0, 0);
            UpdateResourceCounters();
            UpdateFactionDistributionPips();
            UpdatePipsWithCurrentTurnData();
            //CheckForTimelineConflicts();
            GetTimelineAwareness();
            FindObjectOfType<TechTree>().GetComponent<TechTree>().ClearBuffs();
            FindObjectOfType<TechTree>().GetComponent<TechTree>().UpdateNodes();
        }

        public void AddDisastersToGameChanges(Disaster _disaster)
        {
            // Take generated disasters from the disaster manager and insert them into the game changes list
            game_changes[_disaster.turn].Add(new ApplyDisaster(_disaster));            
        }

        private void CheckAllUserTechPurchasesValid()
        {
            TechTree tt = FindObjectOfType<TechTree>().GetComponent<TechTree>();
            List<TechNode> nodes = new List<TechNode>();
            // Loop through all turns
            for (uint t = 0; t < user_changes.Length; t++)
            {
                CTTurnData data = GetYearData(t);

                // If turn contains user change of type PurchaseTechnology
                if (user_changes[t].Exists(x => x.GetType() == typeof(PurchaseTechnology)))
                {
                    // Loop through all user_changes and check if of type PurchaseTechnology
                    for (int i = 0; i < user_changes[t].Count; i++)
                    {
                        // If user change is of type PurchaseTechnology
                        if (user_changes[t][i].GetType() == typeof(PurchaseTechnology))
                        {
                            PurchaseTechnology change = (PurchaseTechnology)user_changes[t][i];
                            // Check if prereqs for tech are unlocked
                            TechNode node = tt.GetNodeOfType(change.tech);
                            List<TechNode> required_nodes = node.GetRequiredTechs();

                            foreach(TechNode n in required_nodes)
                            {
                                if (data.technologies[n.tech] == false)
                                {
                                    // PurchaseTechnology is invalid
                                    user_changes[t].Remove(user_changes[t][i]);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void AIPlayFromTurn(uint _turn)
        {
            // Get Year Data after changes

            CTTurnData data = new CTTurnData(GetYearData(_turn));

            //For each turn
            for (uint t = _turn; t < DataSheet.turns_number - 1; t++)
            {
                //Debug.Log($"Loop {t} has {active_techs} active techs");

                // Generate a faction dist game change
                Vector4 dist = GetRandomFactionSpread(t);

                // If game_changes[t] does not contain a CTChange of type SetFactionDistribution
                if (!game_changes[t].Exists(x => x.GetType() == typeof(SetFactionDistribution)))
                    // Add change to game_changes
                    game_changes[t].Add(new SetFactionDistribution(dist.x, dist.y, dist.z, dist.w));

                // If there is already a PurchaseTechnology change in turn t
                if (game_changes[t].Exists(x => x.GetType() == typeof(PurchaseTechnology)))
                {
                    for (uint i = t; i < DataSheet.turns_number - 1; i++)
                    {
                        // Find index in game_changes[t] of type PurchaseTechnology
                        int index = game_changes[i].FindIndex(change => change is PurchaseTechnology);

                        if (index != -1) // Remove only if found
                            game_changes[i].Remove(game_changes[i][index]);
                    }
                }

                data = new CTTurnData(GetYearData(t));

                List<CTTechnologies> available_techs = new List<CTTechnologies>();
                TechTree tt = FindObjectOfType<TechTree>().GetComponent<TechTree>();
                for (int i = tt.nodes.Count - 1; i >= 0; i--)
                {
                    // Try to buy tech

                    // Do we have pre-req?
                    List<TechNode> req = tt.nodes[i].GetRequiredTechs();

                    // All The nodes we don't already own
                    if (req.Count == 0 && data.technologies[tt.nodes[i].tech] == false)
                    {
                        available_techs.Add(tt.nodes[i].tech);
                        //Debug.Log($"WE CAN BUY {tt.nodes[i].tech}!");

                    }

                    // Check if we own all the prereqs for node
                    int total = 0;
                    foreach (TechNode n in req)
                    {
                        // If do not own the required node
                        if (data.technologies[n.tech] == false)
                            break;
                        else
                            total++;
                    }

                    // All the nodes for which we already own all prereqs
                    if (total == req.Count)
                    {
                        if (data.technologies[tt.nodes[i].tech] == false)
                        {
                            available_techs.Add(tt.nodes[i].tech);
                            //Debug.Log($"WE CAN BUY {tt.nodes[i].tech}!");
                        }
                    }
                }

                // Pick a valid technology to purchase
                int avt_index = CTSeed.RandFromSeed(t, "tech").Next(available_techs.Count);

                // Can we afford it?
                CTResourceTotals rt = new CTResourceTotals(data.Money, data.Science, data.Food, data.Population);

                if (available_techs.Count != 0)
                {
                    if (DataSheet.GetTechPrice(available_techs[avt_index]) < rt)
                    {
                        // Add PurchaceTechnology change in turn t
                        game_changes[t].Add(new PurchaseTechnology(available_techs[avt_index]));
                    }
                    else
                    {
                        //Debug.Log($"Could not purchase technology {available_techs[avt_index]} in turn {t}, resources were: {rt}");
                    }
                }

                CheckAllUserTechPurchasesValid();

                data = new CTTurnData(GetYearData(data.turn + 1));

                // Check if turn failed
                if (data.failed_turn)
                {
                    Debug.LogError($"Failed turn bozo {t}");
                    return;
                }

                Debug.Log(data.Population);
            }
        }

        public bool IsTechnologyOwned(CTTechnologies _tech)
        {
            return true;
        }


        #region Actions
        // Set Policy
        public void ApplyPolicy(int _year, CTPolicyCard _policy)
        {
            if (_year < 0)
                throw new ArgumentException("Year cannot be zero or lower!");

            user_changes[_year].Add(new SetPolicy(_policy));
        }

        public void RevokePolicy(int _year, CTPolicyCard _policy)
        {
            if (_year < 0)
                throw new ArgumentException("Year cannot be zero or lower!");

            user_changes[_year].Add(new RevokePolicy(_policy));
        }

        private void SetFactionDistribution()
        {
            float work_abs  = ComputerController.Instance.pointSelectors[2].pointValue; // Worker
            float sci_abs   = ComputerController.Instance.pointSelectors[0].pointValue; // Scientist
            float farm_abs  = ComputerController.Instance.pointSelectors[3].pointValue; // Farmer
            float plan_abs  = ComputerController.Instance.pointSelectors[1].pointValue; // Planner

            float req_work_ratio    = RAUtility.Remap(work_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
            float req_sci_ratio     = RAUtility.Remap(sci_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
            float req_farm_ratio    = RAUtility.Remap(farm_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
            float req_plan_ratio    = RAUtility.Remap(plan_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
            //Vector4 requested_ratios = new Vector4(req_sci_ratio, req_plan_ratio, req_farm_ratio, req_work_ratio);
            Vector4 requested_ratios = new Vector4(req_work_ratio, req_sci_ratio, req_farm_ratio, req_plan_ratio);

            //float current_sci_ratio = (float)turn.Scientists / (float)turn.Population;
            //float current_plan_ratio = (float)turn.Planners / (float)turn.Population;
            //float current_farm_ratio = (float)turn.Farmers / (float)turn.Population;
            //float current_work_ratio = (float)turn.Workers / (float)turn.Population;
            Vector4 current_ratios = turn_data.faction_distribution;

            //Should make sure that faction spread is not the same as the start of the turn before applying it as a "Change"
            Vector4 a = CTVector4Round(requested_ratios, 10);
            Vector4 b = CTVector4Round(current_ratios, 10);
            if (a == b)
            {
                //Debug.Log("Requested choice is identical to base distribution!");
                return;
            }
            else
            {
                user_changes[current_turn].Add(new SetFactionDistribution(req_work_ratio, req_sci_ratio, req_farm_ratio, req_plan_ratio));
                //prime_timeline.ChangePopulationDistribution();
                awareness_changes[current_turn].Add(new TrackAwareness(DataSheet.year_change_awareness_rate));
                Debug.Log("Choices for faction distribution locked in!");
            }
        }

        public bool PurchaseTechnology(CTTechnologies _t, uint _turn)
        {
            // Check if you can afford tech
            if (DataSheet.technology_price[_t] <= GetResourceTotals())
            {
                turn_data.technologies[_t] = true;
                user_changes[_turn].Add(new PurchaseTechnology(_t));
                current_turn_resource_expenditure += new Vector3(
                    DataSheet.technology_price[_t].money,   // x
                    DataSheet.technology_price[_t].science, // y
                    DataSheet.technology_price[_t].food);   // z

                return true;
            }
            return false;
        }

        public void ResetAwareness()
        {
            awareness_changes = new List<CTChange>[DataSheet.turns_number];
            for (uint year = 0; year < DataSheet.turns_number; year++)
            {
                awareness_changes[year] = new List<CTChange>();
            }
        }

        #endregion


        #region Utility
        private void ProjectNetResource()
        {
            // Look at current faction distribution at current turn 

            // Apply faction distribtion to population at current turn

            // Get numbers for scientists, planners, farmers, workers

            // Multiply scientists, planners, farmers, workers by DataSheet net values

            // Put post-multiplcation net into the projection boxes

            throw new NotImplementedException();
        }

        private void UpdateFactionDistributionPips()
        {
            float workers       = turn_data.faction_distribution.x;
            float scientists    = turn_data.faction_distribution.y;
            float farmers       = turn_data.faction_distribution.z;
            float planners      = turn_data.faction_distribution.w;

            // This does not work as expected
            ComputerController.Instance.pointSelectors[0].pointValue = scientists; // Scientist
            ComputerController.Instance.pointSelectors[1].pointValue = planners; // Planner
            ComputerController.Instance.pointSelectors[2].pointValue = farmers; // Farmer
            ComputerController.Instance.pointSelectors[3].pointValue = workers; // Worker
        }

        private void UpdateResourceCounters()
        {
            ComputerController.Instance.currencyText.text = (turn_data.Money - (int)current_turn_resource_expenditure.x).ToString();
            ComputerController.Instance.rpText.text = (turn_data.Science - (int)current_turn_resource_expenditure.y).ToString();
            ComputerController.Instance.foodText.text = (turn_data.Food - (int)current_turn_resource_expenditure.z).ToString();
            ComputerController.Instance.populationText.text = turn_data.Population.ToString();

            GetTimelineAwareness();
        }

        private void UpdatePipsWithCurrentTurnData()
        {
            // Sci
            ComputerController.Instance.pointSelectors[0].SetPoints(GetFactionDistribtion(CTFaction.Scientist, turn_data) * 10);
            // Plan
            ComputerController.Instance.pointSelectors[1].SetPoints(GetFactionDistribtion(CTFaction.Planner, turn_data) * 10);
            // Farmer
            ComputerController.Instance.pointSelectors[3].SetPoints(GetFactionDistribtion(CTFaction.Farmer, turn_data) * 10);
            // Worker
            ComputerController.Instance.pointSelectors[2].SetPoints(GetFactionDistribtion(CTFaction.Worker, turn_data) * 10);
        }

        private float GetFactionDistribtion(CTFaction _faction, CTTurnData _turn)
        {
            switch (_faction)
            {
                case CTFaction.Scientist:
                    return (turn_data.faction_distribution.y);

                case CTFaction.Planner:
                    return (turn_data.faction_distribution.w);

                case CTFaction.Farmer:
                    return (turn_data.faction_distribution.z);

                case CTFaction.Worker:
                    return (turn_data.faction_distribution.x);

                // Default at impossible error value
                default:
                    Debug.LogError("GameManager.GetFactionDistribution requested faction type is not implemented!");
                    return -1.0f;
            }
        }

        private void DebugDisasterChanges()
        {


            for (int i = 0; i < 40; i++)
            {
                if (game_changes[i].Count != 0)
                {
                    Debug.Log($"Game Changes Turn {i} Size = {game_changes[i].Count}");
                }
            }
        }

        private Vector4 GetRandomFactionSpread(uint _turn)
        {
            System.Random rand = new System.Random();

            float[] floats = { 0, 0, 0, 0 };

            floats[0] = (float)CTSeed.RandFromSeed(_turn, "0").NextDouble();
            floats[1] = (float)CTSeed.RandFromSeed(_turn, "1").NextDouble();
            floats[2] = (float)CTSeed.RandFromSeed(_turn, "2").NextDouble();
            floats[3] = (float)CTSeed.RandFromSeed(_turn, "3").NextDouble();

            float sum = floats[0] + floats[1] + floats[2] + floats[3];

            floats[0] = Mathf.Round(floats[0] / sum * 10f) / 10f;
            floats[1] = Mathf.Round(floats[1] / sum * 10f) / 10f;
            floats[2] = Mathf.Round(floats[2] / sum * 10f) / 10f;
            floats[3] = Mathf.Round(floats[3] / sum * 10f) / 10f;

            float adjusted_sum = floats[0] + floats[1] + floats[2] + floats[3];

            float adjustment = (adjusted_sum > 1.0f) ? -0.1f : 0.1f;

            if (adjusted_sum != 1.0f)
            {
                // If sum is less than 1.0f
                if (adjusted_sum < 1.0f)
                {
                    float lowest = floats.Min();
                    for (int i = 0; i < floats.Length; i++)
                    {
                        if (floats[i] == lowest)
                        {
                            floats[i] += 0.1f;
                            break;
                        }
                    }
                }
                else // If sum is greater than 1.0f
                {
                    float highest = floats.Max();
                    for (int i = 0; i < floats.Length; i++)
                    {
                        if (floats[i] == highest)
                        {
                            floats[i] -= 0.1f;
                            break;
                        }
                    }
                }

                adjusted_sum = floats[0] + floats[1] + floats[2] + floats[3];

            }
            return new Vector4(floats[0], floats[1], floats[2], floats[3]);
        }

        public CTTurnData GetTurn()
        {
            return turn_data;
        }

        private CTResourceTotals GetResourceTotals()
        {
            return new CTResourceTotals(
                turn_data.Money - (int)current_turn_resource_expenditure.x, 
                turn_data.Science - (int)current_turn_resource_expenditure.y, 
                turn_data.Food - (int)current_turn_resource_expenditure.z, 
                turn_data.Population);
        }

        public List<CTTechnologies> GetUnlockedTechnologiesInTurn()
        {
            List<CTTechnologies> ret = new List<CTTechnologies>();

            //Debug.Log($"{}");

            foreach (KeyValuePair<CTTechnologies, bool> kvp in turn_data.technologies)
            {
                if (turn_data.technologies[kvp.Key] == true)
                {
                    ret.Add(kvp.Key);
                }
            }

            //Debug.Log($"GameManager.GetUnlockedTechnologiesInTurn: {ret.Count}, {turn_data.turn}");

            return ret;
        }

        private void GetChangesAtTurn()
        {
            if (current_turn == 40) { return; }
            user_changes_in_turn = user_changes[current_turn].Count();
        }

        private int GetTurnChanges()
        {
            return user_changes[current_turn].Count();
        }

        private bool WereChangesMadeInTurn()
        {
            return !(GetTurnChanges() == user_changes_in_turn);
        }

        public static Vector4 CTVector4Round(Vector4 _value, int _precision)
        {
            Vector4 ret = new Vector4(
                (Mathf.Round(_value.x * _precision) / _precision),
                (Mathf.Round(_value.y * _precision) / _precision),
                (Mathf.Round(_value.z * _precision) / _precision),
                (Mathf.Round(_value.w * _precision) / _precision));

            //float ret = Mathf.Round(_value * _precision) / _precision;
            return ret;
        }

        public void GetTimelineAwareness()
        {
            float ret = 0.0f;

            for (int i = 0; i < awareness_changes.Length; i++)
            {
                foreach (TrackAwareness blob in awareness_changes[i])
                {
                    ret += blob.value;
                }
            }

            ComputerController.Instance.mat_awareness.SetFloat("_FillAmount", ret);
        }
        #endregion
    }
}
