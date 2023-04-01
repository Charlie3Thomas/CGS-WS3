using System.Collections;
using System.Collections.Generic;
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
    using Data.Changes;
    using Data;
    using Lookup;
    using TMPro;

    public class GameManager : MonoBehaviour
    {
        public static GameManager _INSTANCE;

        public List<CTChange>[] user_changes;
        public List<CTChange>[] game_changes;
        CTYearData turn = new CTYearData();
        CTTimelineData prime_timeline;

        public uint current_turn = 0;

        [SerializeField] private TextMeshProUGUI Money;
        [SerializeField] private TextMeshProUGUI Science;
        [SerializeField] private TextMeshProUGUI Food;

        private IEnumerator coroutine;

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
            user_changes = new List<CTChange>[DataSheet.turns_number];
            for (uint year = 0; year < DataSheet.turns_number; year++)
            {
                user_changes[year] = new List<CTChange>();
            }

            game_changes = new List<CTChange>[DataSheet.turns_number];
            for (uint year = 0; year < DataSheet.turns_number; year++)
            {
                game_changes[year] = new List<CTChange>();
            }

            prime_timeline = new CTTimelineData(0, DataSheet.turns_number, user_changes, game_changes);
            
            turn = prime_timeline.GetYearData(0);
            user_changes = prime_timeline.user_changes;
            game_changes = prime_timeline.game_changes;
            coroutine = WaitThenNexTTurn(1);
            StartCoroutine(coroutine);
        }

        private void Update()
        {
            
        }
        


        private IEnumerator WaitThenNexTTurn(float t)
        {
            while (true)
            {
                yield return new WaitForSeconds(t);

                prime_timeline = new CTTimelineData(0, DataSheet.turns_number, user_changes, game_changes);

                int r = Random.Range(0, 10);

                if (r > 7)
                {
                    prime_timeline.ChangePopulationDistribution(current_turn, 0.25f, 0.25f, 0.25f, 0.25f);
                }
                else
                {
                    prime_timeline.ChangePopulationDistribution(current_turn, 0.1f, 0.1f, 0.8f, 0.0f);
                }

                // Copy changes from timeline to local
                user_changes = prime_timeline.user_changes;
                game_changes = prime_timeline.game_changes;

                turn = prime_timeline.GetYearData(++current_turn);

                UpdateResourceCounters();
                UpdateFactionDistributionSliders();
            }
        }

        public void OnClickCheckoutYearButton(uint _turn)
        {
            // Construct new timeline using stored changes
            prime_timeline = new CTTimelineData(_turn, DataSheet.turns_number, user_changes, game_changes);

            // store timeline turn data
            turn = prime_timeline.GetYearData(_turn);

            // Set resource counters to values at turn
            UpdateResourceCounters();

            // Set faction distribution sliders to values at turn
            UpdateFactionDistributionSliders();
        }

        private void UpdateResourceCounters()
        {
            Money.text = turn.Money.ToString();
            Science.text = turn.Science.ToString();
            Food.text = turn.Food.ToString();

            ComputerController.Instance.foodText.text = turn.Food.ToString();
            ComputerController.Instance.rpText.text = turn.Science.ToString();
            ComputerController.Instance.currencyText.text = turn.Money.ToString();
            ComputerController.Instance.populationText.text = turn.Population.ToString();
        }

        private void UpdateFactionDistributionSliders()
        {
            float workers = turn.Workers / turn.Population;
            float scientists = turn.Scientists / turn.Population;
            float farmers = turn.Farmers / turn.Population;
            float planners = turn.Planners / turn.Population;

            ComputerController.Instance.pointSelectors[3].pointValue = workers; // Worker
            ComputerController.Instance.pointSelectors[0].pointValue = scientists; // Scientist
            ComputerController.Instance.pointSelectors[2].pointValue = farmers; // Farmer
            ComputerController.Instance.pointSelectors[1].pointValue = planners; // Planner
        }


        #region Utility
        private void GetFactionSelectorRatio()
        {
            float sci_abs = ComputerController.Instance.pointSelectors[0].pointValue; // Scientist
            float plan_abs = ComputerController.Instance.pointSelectors[1].pointValue; // Planner
            float farm_abs = ComputerController.Instance.pointSelectors[2].pointValue; // Farmer
            float work_abs = ComputerController.Instance.pointSelectors[3].pointValue; // Worker

            float sci_ratio = Remap(sci_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
            float plan_ratio = Remap(plan_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
            float farm_ratio = Remap(farm_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
            float work_ratio = Remap(work_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);

            prime_timeline.ChangePopulationDistribution(current_turn, work_ratio, sci_ratio, farm_ratio, plan_ratio);

            Debug.Log("Choices for faction distribution locked in!");
        }

        public float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        #endregion

    }
}
