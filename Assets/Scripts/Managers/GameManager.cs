using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    using Lookup;
    using TMPro;
    using Unity.VisualScripting;

    public class GameManager : MonoBehaviour
    {
        public static GameManager _INSTANCE;

        CTTimelineData prime_timeline;

        CTYearData turn = new CTYearData();

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
            prime_timeline = new CTTimelineData();
            prime_timeline.Initialise(1, DataSheet.turns_number);
            //prime_timeline.ChangePopulationDistribution(0, 0.1f, 0.1f, 0.8f, 0.0f);
            prime_timeline.ChangePopulationDistribution(0, 0.25f, 0.25f, 0.25f, 0.25f);
            turn = prime_timeline.GetYearData(15);
            coroutine = WaitThenNexTTurn(0.5f);
            StartCoroutine(coroutine);


        }

        private void Update()
        {
            Money.text = turn.Money.ToString();
            Science.text = turn.Science.ToString();
            Food.text = turn.Food.ToString();

            ComputerController.Instance.foodText.text = turn.Food.ToString();
            ComputerController.Instance.rpText.text = turn.Science.ToString();
            ComputerController.Instance.currencyText.text = turn.Money.ToString();
            ComputerController.Instance.populationText.text = turn.Population.ToString();
        }

        private IEnumerator WaitThenNexTTurn(float t)
        {

            while (true)
            {
                yield return new WaitForSeconds(t);
                if (current_turn < DataSheet.turns_number - 1)
                {
                    turn = new CTYearData();
                    turn = prime_timeline.GetYearData(15);
                }
            }
        }











        //private void Start()
        //{
        //    // Constant between turns
        //    prime_timeline = new CTTimelineData();
        //    prime_timeline.Initialise(0, DataSheet.turns_number);
        //    test = prime_timeline.GetYearData(current_turn);

        //}

        //private void Update()
        //{
        //    UpdateResourceCounters();
        //    Debug.Log(test.Food.ToString());
        //}
        //#endregion

        //// Confirm you want to look at another year
        //public void ConfirmCheckoutYear(uint turn)
        //{
        //    //if desired year is not current year
        //    current_turn++;
        //    test = prime_timeline.GetYearData(current_turn);
        //    Debug.Log($"ConfirmCheckoutYear {current_turn}");
        //}

        //// Lock in your changes to faction spread & policy.
        //public void ConfirmTurn()
        //{
        //    GetFactionSelectorRatio();

        //    Debug.Log("ConfirmTurn");
        //}

        //private void UpdateResourceCounters()
        //{
        //    ComputerController.Instance.foodText.text = test.Food.ToString();
        //    ComputerController.Instance.rpText.text = test.Science.ToString();
        //    ComputerController.Instance.currencyText.text = test.Money.ToString();
        //    ComputerController.Instance.populationText.text = test.Population.ToString();
        //    //ComputerController.Instance.foodText.text = prime_timeline.GetYearData(current_turn).Food.ToString();
        //    //ComputerController.Instance.rpText.text = prime_timeline.GetYearData(current_turn).Science.ToString();
        //    //ComputerController.Instance.currencyText.text = prime_timeline.GetYearData(current_turn).Money.ToString();
        //    //ComputerController.Instance.populationText.text = prime_timeline.GetYearData(current_turn).Population.ToString();
        //}

        //private void GetFactionSelectorRatio()
        //{
        //    float sci_abs = ComputerController.Instance.pointSelectors[0].pointValue; // Scientist
        //    float plan_abs = ComputerController.Instance.pointSelectors[1].pointValue; // Planner
        //    float farm_abs = ComputerController.Instance.pointSelectors[2].pointValue; // Farmer
        //    float work_abs = ComputerController.Instance.pointSelectors[3].pointValue; // Worker

        //    float sci_ratio = Remap(sci_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
        //    float plan_ratio = Remap(plan_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
        //    float farm_ratio = Remap(farm_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
        //    float work_ratio = Remap(work_abs, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);

        //    prime_timeline.ChangePopulationDistribution(current_turn, work_ratio, sci_ratio, farm_ratio, plan_ratio);

        //    Debug.Log("Choices for faction distribution locked in!");
        //}

        //public float Remap(float value, float from1, float to1, float from2, float to2)
        //{
        //    return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        //}
    }
}
