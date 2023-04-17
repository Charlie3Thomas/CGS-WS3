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
    using Lookup;
    using Enumerations;
    using Unity.VisualScripting;
    using CT.Data.Resources;

    public class GameManager : MonoBehaviour
    {
        public static GameManager _INSTANCE;

        private List<CTChange>[] user_changes;
        private List<CTChange>[] game_changes;
        private CTYearData turn = new CTYearData();
        private CTTimelineData prime_timeline;

        public uint current_turn = 0;

        [SerializeField] private TextMeshProUGUI Planners;
        [SerializeField] private TextMeshProUGUI Workers;
        [SerializeField] private TextMeshProUGUI Farmers;
        [SerializeField] private TextMeshProUGUI Scientists;

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
            Initialise();
            SetBaseFactionSpreadPerTurn();
            //Invoke("DebugDisasterChanges", 1.0f);
        }

        private void Update()
        {
            //// Values here are correct
            //float workers = (float)turn.Workers / (float)turn.Population;
            //float scientists = (float)turn.Scientists / (float)turn.Population;
            //float farmers = (float)turn.Farmers / (float)turn.Population;
            //float planners = (float)turn.Planners / (float)turn.Population;

            //Planners.text = planners.ToString();
            //Workers.text = workers.ToString();
            //Farmers.text = farmers.ToString();
            //Scientists.text = scientists.ToString();
        }

        private void Initialise()
        {
            // Initialise user changes list
            user_changes = new List<CTChange>[DataSheet.turns_number];
            for (uint year = 0; year < DataSheet.turns_number; year++)
            {
                user_changes[year] = new List<CTChange>();
            }

            // Initialise game changes list
            game_changes = new List<CTChange>[DataSheet.turns_number];
            for (uint year = 0; year < DataSheet.turns_number; year++)
            {
                game_changes[year] = new List<CTChange>();
            }

            // Initialise prime timeline
            prime_timeline = new CTTimelineData(0, DataSheet.turns_number, user_changes, game_changes);

            turn = prime_timeline.GetYearData(0);

            UpdateResourceCounters();
            UpdateFactionDistributionSliders();

            user_changes = prime_timeline.user_changes;
            game_changes = prime_timeline.game_changes;

            UpdatePipsWithCurrentTurnData();

            //ApplyPopulationGrowthChange(0, GetPopulationGrowth());
        }

        public void OnClickCheckoutYearButton(uint _turn)
        {
            // Don't allow user to checkout year if the requested turn is the current turn
            if (_turn == current_turn)
                return;

            // Lock in changes to faction distribution
            ConfirmFactionDistribution();

            current_turn = _turn;

            // Construct new timeline using stored changes
            prime_timeline = new CTTimelineData(_turn, DataSheet.turns_number, user_changes, game_changes);

            // store timeline turn data
            turn = prime_timeline.GetYearData(_turn);

            // Set resource counters to values at turn
            UpdateResourceCounters();

            // Set faction distribution sliders to values at turn
            UpdateFactionDistributionSliders();

            // Draw pips with turn data
            UpdatePipsWithCurrentTurnData();

            // Propogate population changes to all future turns
            //ApplyPopulationGrowthChange(_turn, GetPopulationGrowth());
            game_changes[_turn].Add(new SetFactionDistribution(0.25f, 0.25f, 0.25f, 0.25f));

            //if (game_changes[_turn][0].GetType() == typeof(SetFactionDistribution))
            //{
            //    // Get variables from game_changes[_turn][0] if of type SetFactionDistribution
            //    //SetFactionDistribution change = (SetFactionDistribution)game_changes[_turn][0];

            //    //Debug.Log(
            //    //    $"Planners: {change.planner_percentage},  " +
            //    //    $"Workers: {change.worker_percentage}, " +
            //    //    $"Farmers: {change.farmer_percentage}, " +
            //    //    $"Scientists: {change.scientist_percentage}");
            //}

            CheckForTimelineConflicts();

            FindObjectOfType<TechTree>().GetComponent<TechTree>().ClearBuffs();
            FindObjectOfType<TechTree>().GetComponent<TechTree>().UpdateNodes();
        }

        private void UpdateResourceCounters()
        {
            ComputerController.Instance.foodText.text = turn.Food.ToString();
            ComputerController.Instance.rpText.text = turn.Science.ToString();
            ComputerController.Instance.currencyText.text = turn.Money.ToString();
            ComputerController.Instance.populationText.text = turn.Population.ToString();
        }

        private void UpdateFactionDistributionSliders()
        {
            // Values here are correct
            float workers = (float)turn.Workers / (float)turn.Population;
            float scientists = (float)turn.Scientists / (float)turn.Population;
            float farmers = (float)turn.Farmers / (float)turn.Population;
            float planners = (float)turn.Planners / (float)turn.Population;

            Planners.text = planners.ToString();
            Workers.text = workers.ToString();
            Farmers.text = farmers.ToString();
            Scientists.text = scientists.ToString();

            // This does not work as expected
            ComputerController.Instance.pointSelectors[0].pointValue = scientists; // Scientist
            ComputerController.Instance.pointSelectors[1].pointValue = planners; // Planner
            ComputerController.Instance.pointSelectors[2].pointValue = farmers; // Farmer
            ComputerController.Instance.pointSelectors[3].pointValue = workers; // Worker
        }

        private void ProjectNetResource()
        {
            // Look at current faction distribution at current turn 

            // Apply faction distribtion to population at current turn

            // Get numbers for scientists, planners, farmers, workers

            // Multiply scientists, planners, farmers, workers by DataSheet net values

            // Put post-multiplcation net into the projection boxes

            throw new NotImplementedException();
        }

        //private void ApplyPopulationGrowthChange(uint _changed_turn, uint _growth)
        //{
        //    for (uint i = _changed_turn; i < DataSheet.turns_number; i++)
        //    {
        //        // Clear Game Changes of pop and distribution changes from game_changes[i]
        //        foreach (PopulationGrowth c in game_changes[i].FindAll(x => x is PopulationGrowth))
        //            game_changes[i].Remove(c);
        //        foreach (SetFactionDistribution c in game_changes[i].FindAll(x => x is SetFactionDistribution))
        //            game_changes[i].Remove(c);

        //        CTYearData data = prime_timeline.GetYearData(i);
        //        game_changes[i].Add(new PopulationGrowth(_growth));

        //        game_changes[i].Add(new SetFactionDistribution(
        //            GetFactionDistribtion(CTFaction.Worker, data),
        //            GetFactionDistribtion(CTFaction.Scientist, data),
        //            GetFactionDistribtion(CTFaction.Farmer, data),
        //            GetFactionDistribtion(CTFaction.Planner, data)));
        //    }
        //}

        public void AddDisastersToGameChanges(Disaster _disaster)
        {
            // Take generated disasters from the disaster manager and insert them into the game changes list
            game_changes[_disaster.turn].Add(new ApplyDisaster(_disaster));            
        }

        private void SetBaseFactionSpreadPerTurn()
        {
            // for each turn between zero and max turns
            // generate a new faction distribution for each turn
            // add to list of turns in index of for loop

            for (int i = 1; i < DataSheet.turns_number; i++)
            {
                Vector4 spread = GetRandomFactionSpread();
                game_changes[i].Add(new SetFactionDistribution(spread[0], spread[1], spread[2], spread[3]));
            }
        }


        #region Utility

        private uint GetPopulationGrowth()
        {
            return 100;

            throw new NotImplementedException();
        }

        private void UpdatePipsWithCurrentTurnData()
        {
            //// Sci
            //ComputerController.Instance.pointSelectors[0].SetPoints(GetFactionDistribtion(CTFaction.Scientist, turn) * 10);
            //// Plan
            //ComputerController.Instance.pointSelectors[1].SetPoints(GetFactionDistribtion(CTFaction.Planner, turn) * 10);
            //// Farmer
            //ComputerController.Instance.pointSelectors[2].SetPoints(GetFactionDistribtion(CTFaction.Farmer, turn) * 10);
            //// Worker
            //ComputerController.Instance.pointSelectors[3].SetPoints(GetFactionDistribtion(CTFaction.Worker, turn) * 10);

            // Sci
            ComputerController.Instance.pointSelectors[0].SetPoints(GetFactionDistribtion(CTFaction.Scientist, turn) * 10);
            // Plan
            ComputerController.Instance.pointSelectors[1].SetPoints(GetFactionDistribtion(CTFaction.Planner, turn) * 10);
            // Farmer
            ComputerController.Instance.pointSelectors[3].SetPoints(GetFactionDistribtion(CTFaction.Farmer, turn) * 10);
            // Worker
            ComputerController.Instance.pointSelectors[2].SetPoints(GetFactionDistribtion(CTFaction.Worker, turn) * 10);
        }

        private float GetFactionDistribtion(CTFaction _faction, CTYearData _turn)
        {
            switch (_faction)
            {
                case CTFaction.Scientist:
                    return ((float)_turn.Scientists / (float)_turn.Population);

                case CTFaction.Planner:
                    return ((float)_turn.Planners / (float)_turn.Population);

                case CTFaction.Farmer:
                    return ((float)_turn.Farmers / (float)_turn.Population);

                case CTFaction.Worker:
                    return ((float)_turn.Workers / (float)_turn.Population);

                // Default at impossible error value
                default:
                    Debug.LogError("GameManager.GetFactionDistribution requested faction type is not implemented!");
                    return -1.0f;
            }
        }

        private void ConfirmFactionDistribution()
        {
            float sci_abs = ComputerController.Instance.pointSelectors[0].pointValue; // Scientist
            float plan_abs = ComputerController.Instance.pointSelectors[1].pointValue; // Planner
            float farm_abs = ComputerController.Instance.pointSelectors[2].pointValue; // Farmer
            float work_abs = ComputerController.Instance.pointSelectors[3].pointValue; // Worker

            float req_sci_ratio = Remap(sci_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
            float req_plan_ratio = Remap(plan_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
            float req_farm_ratio = Remap(farm_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
            float req_work_ratio = Remap(work_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);

            float current_sci_ratio = (float)turn.Scientists / (float)turn.Population;
            float current_plan_ratio = (float)turn.Planners / (float)turn.Population;
            float current_farm_ratio = (float)turn.Farmers / (float)turn.Population;
            float current_work_ratio = (float)turn.Workers / (float)turn.Population;

            // Should make sure that faction spread is not the same as the start of the turn before applying it as a "Change"
            if (req_sci_ratio == current_sci_ratio &&
                req_plan_ratio == current_plan_ratio &&
                req_farm_ratio == current_farm_ratio &&
                req_work_ratio == current_work_ratio)
            {
                Debug.Log("Requested choice is identical to base distribution!");
                return;
            }
            else
            {
                prime_timeline.ChangePopulationDistribution(current_turn, req_work_ratio, req_sci_ratio, req_farm_ratio, req_plan_ratio);
                Debug.Log("Choices for faction distribution locked in!");
            }
        }

        public float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        #endregion

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

        private Vector4 GetRandomFactionSpread()
        {
            System.Random rand = new System.Random();

            float[] floats = { 0, 0, 0, 0 };

            floats[0] = (float)rand.NextDouble();
            floats[1] = (float)rand.NextDouble();
            floats[2] = (float)rand.NextDouble();
            floats[3] = (float)rand.NextDouble();

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

        public CTYearData GetTurnData()
        {
            return turn;
        }

        public bool PurchaseTechnology(CTTechnologies _t)
        {
            // Check if you can afford tech
            if (DataSheet.technology_price[_t] <= GetResourceTotals())
            {
                user_changes[current_turn].Add(new PurchaseTechnology(_t));
                return true;
            }
            return false;            
        }

        private CTResourceTotals GetResourceTotals()
        {
            return new CTResourceTotals(turn.Money, turn.Science, turn.Food, turn.Population);
        }

        public List<CTTechnologies> GetUnlockedTechnologiesInTurn()
        {
            List<CTTechnologies> ret = new List<CTTechnologies>();

            for (int i = 0; i <= current_turn; i++)
            {
                foreach (CTChange c in user_changes[i])
                {
                    if (c.GetType() == typeof(PurchaseTechnology))
                    {
                        PurchaseTechnology p = (PurchaseTechnology)c;
                        ret.Add(p.tech);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Look at the change being made, and if the same change is made in the future, remove the change from the timeline
        /// This specifically applies to technology purchaes for now
        /// </summary>
        private void CheckForTimelineConflicts()
        {
            Dictionary<CTTechnologies, Vector2> changes = new Dictionary<CTTechnologies, Vector2>();

            // Backwards from the end of the timeline
            for (int t = user_changes.Count() - 1; t >= 0; t--)
            {   // Look through every change in the list
                for (int c = 0; c < user_changes[t].Count(); c++)
                {   // If the change in the list is of type PurchaseTechnology
                    if (user_changes[t][c].GetType() == typeof(PurchaseTechnology))
                    {
                        Debug.Log("Found type of purchase technology");
                        // Copy the change
                        PurchaseTechnology p = (PurchaseTechnology)user_changes[t][c];
                        
                        // Add the change to the dictionary
                        if (!changes.ContainsKey(p.tech))
                        {
                            changes.Add(p.tech, new Vector2(t, c));
                        }
                        else
                        {
                            Debug.Log($"The cost of the technology {p.tech} {DataSheet.technology_price[p.tech]} was refunded in the year {(int)changes[p.tech].x}");                            
                            user_changes[(int)changes[p.tech].x].RemoveAt((int)changes[p.tech].y);
                        }
                    }
                }
            }
        }
    }
}
