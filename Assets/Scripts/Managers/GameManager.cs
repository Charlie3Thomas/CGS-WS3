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
        private ApplyDisaster[] disaster_timeline;//Sam: changed access modifier so i can read it out of this script (don't worry cant write to it outside of this script)
        private List<CTChange>[] game_changes;
        private List<CTChange>[] user_changes;
        private List<CTChange>[] awareness_changes;

        public List<CTChange>[] UserChanges { get { return user_changes; } }
        public List<CTChange>[] GameChanges { get { return game_changes; } }
        public List<CTChange>[] AwarenessChanges { get { return awareness_changes; } }

        // Prime timeline
        private static readonly CTTurnData initial_year = new CTTurnData();

        // Current timeline
        public CTTurnData turn_data = new CTTurnData();

        public uint current_turn { get; private set; }
        public uint CurrentTurn { get { return current_turn; } }
        private int stored_changes_in_turn;
        private Vector3 empty_turn_resource_expenditure;

        #endregion



        #region UnityFunctions
        private void Awake()
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }


        private void Start()
        {
        }

        private void FixedUpdate()
        {
            //UpdateResourceCounters();
        }
        #endregion


        #region Setup

        public void Setup()
        {
            // Setup data containers
            Initialise();

            // Generate disasters and populate into data containers
            DisasterManager.instance.Generate();

            // AI generate turn data
            AIPlayFromTurn(0);

            // Set current turn data to year zero
            turn_data = new CTTurnData(GetYearData(0));

            // Setup UI
            UpdateResourceCounters();
            UpdateFactionDistributionPips();
            UpdatePipsWithCurrentTurnData();

            // Setup Tech Tree
            FindObjectOfType<TechTree>().GetComponent<TechTree>().ClearBuffs();
            FindObjectOfType<TechTree>().GetComponent<TechTree>().UpdateNodes();

            //Debug.Log($"{current_turn}: Workers: {turn_data.GetFactionDistribution().y}, Scientists: {turn_data.GetFactionDistribution().x}, Farmers: {turn_data.GetFactionDistribution().z}, Planners: {turn_data.GetFactionDistribution().w}");

        }

        private void Initialise()
        {
            current_turn = 0;

            initial_year.Initialise(DataSheet.STARTING_MONEY, DataSheet.STARTING_SCIENCE, DataSheet.STARTING_FOOD, DataSheet.STARTING_POPULATION);

            // Initialise disaster timeline
            disaster_timeline = new ApplyDisaster[DataSheet.TURNS_NUMBER  + 1];
            for (int i = 0; i < disaster_timeline.Length; i++)
            {
                disaster_timeline[i] = null;
            }

            Debug.Log("Disaster Timeline initialised");

            // Initialise user changes list
            user_changes = new List<CTChange>[DataSheet.TURNS_NUMBER + 1];
            for (uint year = 0; year < user_changes.Length; year++)
                user_changes[year] = new List<CTChange>();

            user_changes[0].Add(new PurchaseTechnology(CTTechnologies.Banking));
            user_changes[0].Add(new PurchaseTechnology(CTTechnologies.Laboratory));
            user_changes[0].Add(new PurchaseTechnology(CTTechnologies.TownPlanning));
            user_changes[0].Add(new PurchaseTechnology(CTTechnologies.Granary));

            // Initialise game changes list
            game_changes = new List<CTChange>[DataSheet.TURNS_NUMBER + 1];
            for (uint year = 0; year < game_changes.Length; year++)
                game_changes[year] = new List<CTChange>();

            // Initialise awareness changes list
            awareness_changes = new List<CTChange>[DataSheet.TURNS_NUMBER + 1];
            for (uint year = 0; year < awareness_changes.Length; year++)
                awareness_changes[year] = new List<CTChange>();

            stored_changes_in_turn = 0;

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
                ret.ApplyBuffNerfs();

                // Grow population check
                if (ret.Food >= ret.Population) { ret.GrowPopulation(i); }

                // Apply modifiers to base faction produce/consumption
                Vector4 net_mods = ret.GetFactionNetModifiers();
                CTCost mod_workers = new CTCost(DataSheet.WORKER_NET);
                mod_workers.money *= net_mods.x;
                CTCost mod_scientists = new CTCost(DataSheet.SCIENTIST_NET);
                mod_scientists.science *= net_mods.y;
                CTCost mod_farmers = new CTCost(DataSheet.FARMERS_NET);
                mod_farmers.food *= net_mods.z;

                // Apply net costs for each faction to a net total
                CTCost net_total = new CTCost(0, 0, 0, 0);
                net_total += (mod_workers               * ret.Workers);
                net_total += (mod_scientists            * ret.Scientists);
                net_total += (mod_farmers               * ret.Farmers);
                net_total += (DataSheet.PLANNERS_NET    * net_mods.w * ret.Planners);
                net_total += (DataSheet.UNEMPLOYED_NET  * ret.UnassignedPopulation);

                // Decay population check
                if (ret.Food < ret.Population) { ret.DecayPopulation(net_total.food); }

                // Apply net faction produce/consumption
                ret.ApplyCosts(net_total, CTCostType.Upkeep);

                // Apply disaster events
                disaster_timeline[i]?.ApplyChange(ref ret);

                //Debug.Log($"Turn {i} mods: {net_mods}");
            }

            return ret;
        }

        public List<CTTurnData> GetTimelineData() 
        {
            List<CTTurnData> ret = new List<CTTurnData>();

            CTTurnData init = new CTTurnData(initial_year);

            for (int i = 0; i <= DataSheet.TURNS_NUMBER; i++)
            {
                init.turn = (uint)i;

                // Game Changes for year
                foreach (CTChange change in game_changes[i])
                    change.ApplyChange(ref init);

                // User Changes for year
                foreach (CTChange change in user_changes[i])
                    change.ApplyChange(ref init);

                // Get Total Modifiers for turn
                init.ApplyBuffNerfs();

                // Grow population check
                if (init.Food >= init.Population) { init.GrowPopulation(i); }

                // Apply modifiers to base faction produce/consumption
                Vector4 net_mods = init.GetFactionNetModifiers();
                CTCost mod_workers = new CTCost(DataSheet.WORKER_NET);
                mod_workers.money *= net_mods.x;
                CTCost mod_scientists = new CTCost(DataSheet.SCIENTIST_NET);
                mod_scientists.science *= net_mods.y;
                CTCost mod_farmers = new CTCost(DataSheet.FARMERS_NET);
                mod_farmers.food *= net_mods.z;

                // Apply net resource worth of each assigned population member for each turn between zero and requested turn
                CTCost net_total = new CTCost(0, 0, 0, 0);
                net_total += (mod_workers       * init.Workers);
                net_total += (mod_scientists    * init.Scientists);
                net_total += (mod_farmers       * init.Farmers);
                net_total += (DataSheet.PLANNERS_NET * net_mods.w * init.Planners);
                net_total += (DataSheet.UNEMPLOYED_NET * init.UnassignedPopulation);

                // Decay population check
                if (init.Food < init.Population) { init.DecayPopulation(net_total.food); }

                // Apply net faction produce/consumption
                init.ApplyCosts(net_total, CTCostType.Upkeep);

                // Apply disaster events
                disaster_timeline[i]?.ApplyChange(ref init);

                CTTurnData data = new CTTurnData(init);

                ret.Add(data);
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

                //CheckAllUserTechPurchasesValid();

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
            // Don't allow user to checkout year if the requested turn is the current turn
            //if (_requested_turn == current_turn)
            //    return;

            // Clear any disaster effects
            if (DisasterEffectManager.instance != null)
                DisasterEffectManager.instance.ClearDisasterEffects();

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

            stored_changes_in_turn = 0;

            current_turn = _requested_turn;

            CheckAllUserTechPurchasesValid();

            turn_data = new CTTurnData(GetYearData(_requested_turn));

            if (DisasterEffectManager.instance != null && disaster_timeline[current_turn]?.disaster != null)
                //DisasterEffectManager.instance.ShowDisasterEffect(disaster_timeline[current_turn].disaster, disaster_timeline[current_turn].intensity);

            DisasterSeqenceManager.Instance.StartDisasterWarningSequence(); //Sam: abstracted my sequence functionality

            PolicyManager.instance.LoadPoliciesAtCurrentScope(current_turn);

            empty_turn_resource_expenditure = new Vector3(0, 0, 0);

            UpdateResourceCounters();

            UpdateFactionDistributionPips();

            UpdatePipsWithCurrentTurnData();

            SetAwarenessUI();

            FindObjectOfType<TechTree>().GetComponent<TechTree>().ClearBuffs();

            FindObjectOfType<TechTree>().GetComponent<TechTree>().UpdateNodes();

            DisasterManager.instance.WriteDisastersInJournal();
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

        public void UpdateResourceCounters()
        {
            ComputerController.Instance.currencyText.text = (turn_data.Money - (int)empty_turn_resource_expenditure.x).ToString();
            ComputerController.Instance.rpText.text = (turn_data.Science - (int)empty_turn_resource_expenditure.y).ToString();
            ComputerController.Instance.foodText.text = (turn_data.Food - (int)empty_turn_resource_expenditure.z).ToString();

            ComputerController.Instance.populationText.text = turn_data.Population.ToString();
            SetAwarenessUI();
            UpdateCity();
        }

        private void UpdateCity()
        {
            CityBuildingManager.Instance.UpdatePopulation(turn_data.Population);
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
            stored_changes_in_turn++;
        }

        public void RevokePolicy(int _year, CTPolicyCard _policy)
        {
            if (_year < 0)
                throw new ArgumentException("Year cannot be zero or lower!");

            user_changes[_year].Add(new RevokePolicy(_policy));
            stored_changes_in_turn++;
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
                stored_changes_in_turn++;
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
                stored_changes_in_turn++;
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
            foreach (Disaster d in _disaster)
            {
                disaster_timeline[d.turn] = new ApplyDisaster(d);
            }
        }


        private void CheckAllUserTechPurchasesValid()
        {

            TechTree tt = FindObjectOfType<TechTree>().GetComponent<TechTree>();
            List<TechNode> nodes = new List<TechNode>();
            // Loop through all turns
            for (uint t = current_turn; t < user_changes.Length; t++)
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

        public void BigRedButton()
        {
            // Call a policy manager function to clear the lists of active policies

            // Re-Initialise user changes list
            user_changes = new List<CTChange>[DataSheet.TURNS_NUMBER + 1];
            for (uint year = 0; year < user_changes.Length; year++)
                user_changes[year] = new List<CTChange>();

            // Re-Initialise awareness changes and set to 0.3f
            awareness_changes = new List<CTChange>[DataSheet.TURNS_NUMBER + 1];
            for (uint year = 0; year < awareness_changes.Length; year++)
                awareness_changes[year] = new List<CTChange>();
            awareness_changes[current_turn].Add(new TrackAwareness(0.3f));

            // Recalculate AI turns
            AIPlayFromTurn(current_turn);

            // Re-CheckoutTurn
            stored_changes_in_turn = 0;
            CheckAllUserTechPurchasesValid();
            turn_data = new CTTurnData(GetYearData(current_turn));
            AudioManager.Instance.StartDisasterAudio(CheckDisasterInTurn(), GetDisasterIntensityAtTurn(current_turn));
            PolicyManager.instance.LoadPoliciesAtCurrentScope(current_turn);
            empty_turn_resource_expenditure = new Vector3(0, 0, 0);
            UpdateResourceCounters();
            UpdateFactionDistributionPips();
            UpdatePipsWithCurrentTurnData();
            SetAwarenessUI();
            FindObjectOfType<TechTree>().GetComponent<TechTree>().ClearBuffs();
            FindObjectOfType<TechTree>().GetComponent<TechTree>().UpdateNodes();

            // Update graph
            ComputerController.Instance.RefreshGraph();

            // Update notepad
            DisasterManager.instance.WriteDisastersInJournal();

            // Policies
            PolicyManager.instance.Initialise();
        }

        #endregion


        #region Utility

        public RAUtility.Vector4List GetResourcesAcrossYears()
        {
            List<CTTurnData> turns = GetTimelineData();
            List<float> moneys = new List<float>();
            List<float> sciences = new List<float>();
            List<float> foods = new List<float>();
            List<float> populations = new List<float>();

            ComputerController.Instance.turns = turns;

            for (int i = 0; i < DataSheet.TURNS_NUMBER; i++)
            {
                moneys.Add(turns[i].Money / 10f);
                sciences.Add(turns[i].Science / 10f);
                foods.Add(turns[i].Food / 10f);
                populations.Add(turns[i].Population);
                //Debug.Log($"Year {turns[i].turn} has {turns[i].Money} money, {turns[i].Science} science, {turns[i].Food} food, {turns[i].Population} population");
            }

            return new RAUtility.Vector4List(moneys, sciences, foods, populations);
        }



        public float GetFactionDistribtion(CTFaction _faction, CTTurnData _turn)
        {
            switch (_faction)
            {
                case CTFaction.Scientist:
                    return (_turn.GetFactionDistribution().y);

                case CTFaction.Planner:
                    return (_turn.GetFactionDistribution().w);

                case CTFaction.Farmer:
                    return (_turn.GetFactionDistribution().z);

                case CTFaction.Worker:
                    return (_turn.GetFactionDistribution().x);

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

        public CTTurnData PGetYearData(uint _turn)
        {
            return GetYearData(_turn); 
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

        private bool WereChangesMadeInTurn()
        {
            // This is flawed as it returns true even if the user did not make changes in the current year checkout, so long as there are user changes for the current year
            //return !(GetTurnChanges() == user_changes_in_turn);

            // New implementation:
            if (stored_changes_in_turn == 0) // If the stored number of changes for this turn == the current changes in this turn
                return false; // return false;

            return true;

            
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
            return disaster_timeline[current_turn]?.disaster ?? CTDisasters.None;
        }

        /// <summary>
        /// Returns an ApplyDisaster object if a disaster was applied in the given turn, otherwise returns null.
        /// To get current turn, pass in GameManager._INSTANCE.current_turn
        /// </summary>
        public ApplyDisaster GetDisasterDataAtTurn(uint _turn)
        {
            return disaster_timeline[current_turn];
        }

        public float GetDisasterIntensityAtTurn(uint _turn)
        {
            ApplyDisaster ret = GetDisasterDataAtTurn(_turn);
            
            return ret?.intensity ?? -1.0f; // Return error value
        }

        /// <summary>
        /// Returns a list of set policies between the start of the game and the current turn
        /// </summary>
        /// <returns></returns>
        public Dictionary<SetPolicy, int> GetAllSetPoliciesInScope()
        {
            Dictionary<SetPolicy, int> ret = new Dictionary<SetPolicy, int>();

            for (int i = 0; i <= current_turn; i++)
            {
                foreach (CTChange c in user_changes[i])
                {
                    if (c.GetType() == typeof(SetPolicy))
                    {
                        SetPolicy sp = (SetPolicy)c;
                        ret[sp] = i;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Returns a list of revoked policies between the start of the game and the current turn
        /// </summary>
        /// <returns></returns>
        public Dictionary<RevokePolicy, int> GetAllRevokedPoliciesInScope()
        {
            Dictionary<RevokePolicy, int> ret = new Dictionary<RevokePolicy, int>();

            for (int i = 0; i <= current_turn; i++)
            {
                foreach (CTChange c in user_changes[i])
                {
                    if (c.GetType() == typeof(RevokePolicy))
                    {
                        RevokePolicy rp = (RevokePolicy)c;
                        ret[rp] = i;
                    }
                }
            }

            return ret;
        }

        public int GetActiveTechnologyTotal(uint _turn)
        {
            CTTurnData data = new CTTurnData(GetYearData(_turn));

            int ret = 0;

            foreach (KeyValuePair<CTTechnologies, bool> kvp in data.technologies)
            {
                if (kvp.Value == true)
                    ret++;
            }

            return ret;
        }

        /// <summary>
        /// Can be called in any turn, and will return the total awareness 
        /// </summary>
        /// <returns></returns>
        /// 
        // Scorecard element - Awareness bonus
        public float GetAwareness()
        {
            float ret = 0.0f;

            for (int i = 0; i < awareness_changes.Length; i++)
            {
                foreach (TrackAwareness blob in awareness_changes[i])
                {
                    ret += blob.value;
                }
            }

            return ret;
        }
        // Scorecard element - Disasters survived
        public int GetLastSurvivedTurn()
        {
            for (int i = 0; i < DataSheet.TURNS_NUMBER + 1; i++)
            {
                CTTurnData data = GetYearData((uint)i);

                if (data.Population == 0)
                {
                    return i;
                }
            }

            return (int)DataSheet.TURNS_NUMBER;
        }
        // Scorecard element - Technology Nodes unlocked
        public int GetTechnologiesUnlockedTotal()
        {
            CTTurnData data = GetYearData((uint)DataSheet.TURNS_NUMBER);

            int ret = 0;

            foreach (KeyValuePair<CTTechnologies, bool> kvp in data.technologies)
            {
                if (kvp.Value == true)
                    ret++;
            }

            return ret;
        }
        // Scorecard Element - Population Survived
        public int GetLastTurnPopulation()
        {
            CTTurnData data = GetYearData((uint)DataSheet.TURNS_NUMBER);

            return data.Population;
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

        #region Load
        public void LoadData(List<CTChange>[] _userChanges, List<CTChange>[] _gameChanges, List<CTChange>[] _awarenessChanges)
        {
            user_changes = _userChanges;
            game_changes = _gameChanges;
            awareness_changes = _awarenessChanges;
        }
        #endregion
    }
}
