using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    using Enumerations;
    using Lookup;

    public class GameManager : MonoBehaviour
    {
        #region Member Variables
        public static GameManager _INSTANCE;

        // Changes
        private ApplyDisaster[] disaster_timeline;
        private List<CTChange>[] game_changes;
        private List<CTChange>[] user_changes;
        private List<CTChange>[] awareness_changes;

        private static readonly CTTurnData initial_year = new CTTurnData();

        public CTTurnData turn_data = new CTTurnData();
        //private CTTimelineData prime_timeline;

        private uint current_turn = 0;
        public uint CurrentTurn { get { return current_turn; } }
        private int user_changes_in_turn;
        private Vector3 empty_turn_resource_expenditure;

        #endregion



        #region UnityFunctions
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

            //Debug.Log($"{current_turn}: Workers: {turn_data.GetFactionDistribution().y}, Scientists: {turn_data.GetFactionDistribution().x}, Farmers: {turn_data.GetFactionDistribution().z}, Planners: {turn_data.GetFactionDistribution().w}");
        }

        private void FixedUpdate()
        {
            UpdateResourceCounters();
        }
        #endregion


        #region Setup
        private void Initialise()
        {
            initial_year.Initialise(DataSheet.STARTING_MONEY, DataSheet.STARTING_SCIENCE, DataSheet.STARTING_FOOD, DataSheet.STARTING_POPULATION);

            // Initialise disaster timeline
            disaster_timeline = new ApplyDisaster[DataSheet.TURNS_NUMBER  + 1];
            for (int i = 0; i < disaster_timeline.Length; i++)
            {
                disaster_timeline[i] = null;
                //ApplyDisaster init = new ApplyDisaster();
                //init.disaster = CTDisasters.None;
                //init.intensity = -1.0f;
                //init.turn = i;
                //disaster_timeline[i] = init;
            }

            Debug.Log("Disaster Timeline initialised");

            // Initialise user changes list
            user_changes = new List<CTChange>[DataSheet.TURNS_NUMBER + 1];
            for (uint year = 0; year < user_changes.Length; year++)
                user_changes[year] = new List<CTChange>();

            // Initialise game changes list
            game_changes = new List<CTChange>[DataSheet.TURNS_NUMBER + 1];
            for (uint year = 0; year < game_changes.Length; year++)
                game_changes[year] = new List<CTChange>();

            // Initialise awareness changes list
            awareness_changes = new List<CTChange>[DataSheet.TURNS_NUMBER + 1];
            for (uint year = 0; year < awareness_changes.Length; year++)
                awareness_changes[year] = new List<CTChange>();

            empty_turn_resource_expenditure = new Vector3(0, 0, 0);
        }

        #endregion


        #region Data
        private CTTurnData GetYearData(uint _year)
        {
            CTTurnData ret = new CTTurnData(initial_year);

            ret.turn = _year;

            for (int i = 0; i <= _year; i++)
            {
                // Game Changes for year
                foreach (CTChange change in game_changes[i])
                    change.ApplyChange(ref ret);

                // User Changes for year
                foreach (CTChange change in user_changes[i])
                    change.ApplyChange(ref ret);

                // Get Total Modifiers for turn
                //ret.ApplyModifiers();                

                // Apply net resource worth of each assigned population member for each turn between zero and requested turn
                CTCost net_total = new CTCost(0, 0, 0, 0);
                net_total += (DataSheet.WORKER_NET * ret.Workers);
                net_total += (DataSheet.SCIENTIST_NET * ret.Scientists);
                net_total += (DataSheet.FARMERS_NET * ret.Farmers);
                net_total += (DataSheet.PLANNERS_NET * ret.Planners);
                net_total += (DataSheet.UNEMPLOYED_NET * ret.UnassignedPopulation);

                if (ret.Food >= ret.Population) { ret.GrowPopulation(i); }
                else { ret.DecayPopulation(net_total.food); }

                // Apply net faction produce/consumption
                ret.ApplyCosts(net_total, CTCostType.Upkeep);

                // Apply disaster events
                disaster_timeline[i]?.ApplyChange(ref ret);
            }

            return ret;
        }

        public void AIPlayFromTurn(uint _turn)
        {
            // Get Year Data after changes

            CTTurnData data = new CTTurnData(GetYearData(_turn));

            //For each turn
            for (uint t = _turn; t < DataSheet.TURNS_NUMBER - 1; t++)
            {
                //Debug.Log($"Loop {t} has {active_techs} active techs");

                // Generate a faction dist game change
                Vector4 dist = GenerateRandomFactionSpread(t);

                // If game_changes[t] does not contain a CTChange of type SetFactionDistribution
                if (!game_changes[t].Exists(x => x.GetType() == typeof(SetFactionDistribution)))
                    // Add change to game_changes
                    game_changes[t].Add(new SetFactionDistribution(dist.x, dist.y, dist.z, dist.w));

                // If there is already a PurchaseTechnology change in turn t
                if (game_changes[t].Exists(x => x.GetType() == typeof(PurchaseTechnology)))
                {
                    for (uint i = t; i < DataSheet.TURNS_NUMBER - 1; i++)
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

                //Debug.Log(data.Population);
            }
        }
        #endregion


        #region UI
        public void OnClickCheckoutYearButton(uint _requested_turn)
        {
            //Debug.Log("GameManager.OnClickCheckoutYearButton" + _year);
            AudioManager.Instance.StartDisasterAudio(CheckDisasterInTurn(), GetDisasterIntensityAtTurn(current_turn));
            // Don't allow user to checkout year if the requested turn is the current turn
            if (_requested_turn == current_turn)
                return;

            // Lock in changes to faction distribution
            SetFactionDistribution();

            // If player made changes to a turn
            if (WereChangesMadeInTurn())
            {
                // Adjust awareness
                awareness_changes[current_turn].Add(new TrackAwareness(DataSheet.YEAR_CHANGE_AWARENESS_RATE));

                // Recalculate AI turns
                AIPlayFromTurn(current_turn);
            }

            current_turn = _requested_turn;

            CheckAllUserTechPurchasesValid();

            //GetChangesAtTurn();
            turn_data = GetYearData(_requested_turn);
            empty_turn_resource_expenditure = new Vector3(0, 0, 0);
            UpdateResourceCounters();
            UpdateFactionDistributionPips();
            UpdatePipsWithCurrentTurnData();
            //CheckForTimelineConflicts();
            SetAwarenessUI();
            FindObjectOfType<TechTree>().GetComponent<TechTree>().ClearBuffs();
            FindObjectOfType<TechTree>().GetComponent<TechTree>().UpdateNodes();

            //Debug.Log($"{current_turn}: Workers: {turn_data.GetFactionDistribution().y}, Scientists: {turn_data.GetFactionDistribution().x}, Farmers: {turn_data.GetFactionDistribution().z}, Planners: {turn_data.GetFactionDistribution().w}");

            //Debug.Log(turn_data.GetSafetyFactor());

            //turn_data.Logs();

            //LogChangesInCurrentTurn();

            Debug.Log($"{turn_data.turn} Planners ratio: {turn_data.GetFactionDistribution().w} SafetyFactor: {turn_data.GetSafetyFactor()}");

            AudioManager.Instance.StartDisasterAudio(CheckDisasterInTurn());
            

            
        }

        private void UpdateFactionDistributionPips()
        {
            float workers = turn_data.GetFactionDistribution().x;
            float scientists = turn_data.GetFactionDistribution().y;
            float farmers = turn_data.GetFactionDistribution().z;
            float planners = turn_data.GetFactionDistribution().w;

            // This does not work as expected
            ComputerController.Instance.pointSelectors[0].pointValue = scientists; // Scientist
            ComputerController.Instance.pointSelectors[1].pointValue = planners; // Planner
            ComputerController.Instance.pointSelectors[2].pointValue = farmers; // Farmer
            ComputerController.Instance.pointSelectors[3].pointValue = workers; // Worker
        }

        private void UpdateResourceCounters()
        {
            ComputerController.Instance.currencyText.text = (turn_data.Money - (int)empty_turn_resource_expenditure.x).ToString();
            ComputerController.Instance.rpText.text = (turn_data.Science - (int)empty_turn_resource_expenditure.y).ToString();
            ComputerController.Instance.foodText.text = (turn_data.Food - (int)empty_turn_resource_expenditure.z).ToString();

            ComputerController.Instance.populationText.text = turn_data.Population.ToString();

            SetAwarenessUI();
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

        public void SetAwarenessUI()
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
            Vector4 requested_ratios = GetFactionDistribution();

            Vector4 current_ratios = turn_data.GetFactionDistribution();

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
                user_changes[current_turn].Add(new SetFactionDistribution(requested_ratios.x, requested_ratios.y, requested_ratios.z, requested_ratios.w));
                //prime_timeline.ChangePopulationDistribution();
                awareness_changes[current_turn].Add(new TrackAwareness(DataSheet.YEAR_CHANGE_AWARENESS_RATE));
                Debug.Log("Choices for faction distribution locked in!");
            }
        }

        public Vector4 GetFactionDistribution()
        {
            float work_abs = ComputerController.Instance.pointSelectors[2].pointValue; // Worker
            float sci_abs = ComputerController.Instance.pointSelectors[0].pointValue; // Scientist
            float farm_abs = ComputerController.Instance.pointSelectors[3].pointValue; // Farmer
            float plan_abs = ComputerController.Instance.pointSelectors[1].pointValue; // Planner

            float req_work_ratio = RAUtility.Remap(work_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
            float req_sci_ratio = RAUtility.Remap(sci_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
            float req_farm_ratio = RAUtility.Remap(farm_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
            float req_plan_ratio = RAUtility.Remap(plan_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);

            return new Vector4(req_work_ratio, req_sci_ratio, req_farm_ratio, req_plan_ratio);
        }

        public bool PurchaseTechnology(CTTechnologies _t, uint _turn)
        {
            // Check if you can afford tech
            if (DataSheet.technology_price[_t] <= GetResourceTotals())
            {
                turn_data.technologies[_t] = true;
                user_changes[_turn].Add(new PurchaseTechnology(_t));
                empty_turn_resource_expenditure += new Vector3(
                    DataSheet.technology_price[_t].money,   // x
                    DataSheet.technology_price[_t].science, // y
                    DataSheet.technology_price[_t].food);   // z

                return true;
            }
            return false;
        }

        public void ResetAwareness()
        {
            awareness_changes = new List<CTChange>[DataSheet.TURNS_NUMBER];
            for (uint year = 0; year < DataSheet.TURNS_NUMBER; year++)
            {
                awareness_changes[year] = new List<CTChange>();
            }
        }

        public void AddDisastersToGameChanges(List<Disaster> _disaster)
        {
            //for (int i = 0; i < _disaster.Count; i++)
            //{
            //    disaster_timeline[i] = new ApplyDisaster(_disaster[i]);
            //}

            foreach (Disaster d in _disaster)
            {
                disaster_timeline[d.turn] = new ApplyDisaster(d);
            }

            // Take generated disasters from the disaster manager and insert them into the game changes list
            //game_changes[_disaster.turn].Add(new ApplyDisaster(_disaster));
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

                            foreach (TechNode n in required_nodes)
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

        #endregion


        #region Utility

        public RAUtility.Vector4List GetResourcesAcrossYears()
        {
            CTTurnData data = new CTTurnData(initial_year);
            List<float> moneys = new List<float>();
            List<float> sciences = new List<float>();
            List<float> foods = new List<float>();
            List<float> populations = new List<float>();

            for (uint i = 0; i < DataSheet.TURNS_NUMBER - 1; i++)
            {
                data = GetYearData(i);
                moneys.Add(data.Money);
                sciences.Add(data.Science);
                foods.Add(data.Food);
                populations.Add(data.Population);
                //Debug.Log($"Year {i} has {data.Money} money, {data.Science} science, {data.Food} food, {data.Population} population");
            }

            return new RAUtility.Vector4List(moneys, sciences, foods, populations);
        }

        private float GetFactionDistribtion(CTFaction _faction, CTTurnData _turn)
        {
            switch (_faction)
            {
                case CTFaction.Scientist:
                    return (turn_data.GetFactionDistribution().y);

                case CTFaction.Planner:
                    return (turn_data.GetFactionDistribution().w);

                case CTFaction.Farmer:
                    return (turn_data.GetFactionDistribution().z);

                case CTFaction.Worker:
                    return (turn_data.GetFactionDistribution().x);

                // Default at impossible error value
                default:
                    Debug.LogError("GameManager.GetFactionDistribution requested faction type is not implemented!");
                    return -1.0f;
            }
        }

        private Vector4 GenerateRandomFactionSpread(uint _turn)
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

        #region Getters
        public CTTurnData GetTurn()
        {
            return turn_data;
        }

        private CTResourceTotals GetResourceTotals()
        {
            return new CTResourceTotals(
                turn_data.Money - (int)empty_turn_resource_expenditure.x,
                turn_data.Science - (int)empty_turn_resource_expenditure.y,
                turn_data.Food - (int)empty_turn_resource_expenditure.z,
                turn_data.Population);
        }
        public bool IsTechnologyOwned(CTTechnologies _tech)
        {
            return true;
        }

        private int GetTurnChanges()
        {
            return user_changes[current_turn].Count();
        }

        private bool WereChangesMadeInTurn()
        {
            return !(GetTurnChanges() == user_changes_in_turn);
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

        public CTDisasters CheckDisasterInTurn()
        {
            foreach (CTChange change in game_changes[current_turn])
            {
                if (change.GetType() == typeof(ApplyDisaster))
                {
                    ApplyDisaster ret = (ApplyDisaster)change;
                    return ret.disaster;
                }
            }

            return CTDisasters.None;
        }

        /// <summary>
        /// Returns an ApplyDisaster object if a disaster was applied in the given turn, otherwise returns null.
        /// To get current turn, pass in GameManager._INSTANCE.current_turn
        /// </summary>
        public ApplyDisaster GetDisasterDataAtTurn(uint _turn)
        {
            foreach (CTChange change in game_changes[_turn])
            {
                if (change.GetType() == typeof(ApplyDisaster))
                {
                    ApplyDisaster ret = (ApplyDisaster)change;

                    if (ret.disaster == CTDisasters.None)
                        return null;

                    return ret;
                }
            }

            return null;
        }

        public float GetDisasterIntensityAtTurn(uint _turn)
        {
            ApplyDisaster ret = GetDisasterDataAtTurn(_turn);
            
            return ret?.intensity ?? -1.0f; // Return error value
        }

        private void LogChangesInCurrentTurn()
        {
            string ret = $"\n";

            foreach (CTChange c in game_changes[current_turn])
            {
                // For each disaster
                if (c.GetType() == typeof(ApplyDisaster))
                {
                    ApplyDisaster ad = (ApplyDisaster)c;
                    ret = $"{ret} Apply disaster :{ad.disaster} applied in game changes\n";
                }

                // For each set faction distribution
                if (c.GetType() == typeof(SetFactionDistribution))
                {
                    SetFactionDistribution ad = (SetFactionDistribution)c;
                    ret = $"{ret} SetFactionDistribution: {new Vector4(ad.worker_percentage, ad.farmer_percentage, ad.scientist_percentage, ad.planner_percentage)} applied in game changes\n";
                }

                // For each purchase technology
                if (c.GetType() == typeof(PurchaseTechnology))
                {
                    PurchaseTechnology ad = (PurchaseTechnology)c;
                    ret = $"{ret} PurchaseTechnology: {ad.tech} applied in game changes\n";
                }
            }

            foreach (CTChange c in user_changes[current_turn])
            {
                // For each set faction distribution
                if (c.GetType() == typeof(SetFactionDistribution))
                {
                    SetFactionDistribution ad = (SetFactionDistribution)c;
                    ret = $"{ret} SetFactionDistribution applied in user changes\n";
                }

                // For each purchase technology
                if (c.GetType() == typeof(PurchaseTechnology))
                {
                    PurchaseTechnology ad = (PurchaseTechnology)c;
                    ret = $"{ret} PurchaseTechnology: {ad.tech} applied in user changes\n";
                }

                // For each set policy
                if (c.GetType() == typeof(SetPolicy))
                {
                    SetPolicy ad = (SetPolicy)c;
                    ret = $"{ret} SetPolicy: {ad.policy.ID} applied in user changes\n";
                }
            }

            Debug.Log(ret);
        }

        #endregion
        #endregion
    }
}
