using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AllocType
{
    MONEY,
    FOOD,
    SCIENCE,
    SAFETY
}

public class Resource
{
    public AllocType allocType;
    public float amount;
}

public class Person
{
    public int population;
    public List<Resource> upkeep;
    public List<Resource> produce;
}

[System.Serializable]
public class Worker : Person
{
    public new List<Resource> upkeep = new List<Resource> { new Resource { allocType = AllocType.FOOD, amount = 1f } };
    public new List<Resource> produce = new List<Resource> { new Resource { allocType = AllocType.MONEY, amount = 2f } };
}

[System.Serializable]
public class Scientist : Person
{
    public new List<Resource> upkeep = new List<Resource> { new Resource { allocType = AllocType.FOOD, amount = 1f } };
    public new List<Resource> produce = new List<Resource> { new Resource { allocType = AllocType.SCIENCE, amount = 3f } };
}

[System.Serializable]
public class Planner : Person
{
    public new List<Resource> upkeep = new List<Resource> { new Resource { allocType = AllocType.FOOD, amount = 1f } };
    public new List<Resource> produce = new List<Resource> { new Resource { allocType = AllocType.SAFETY, amount = 2f } };
}

[System.Serializable]
public class Farmer : Person
{
    public new List<Resource> upkeep = new List<Resource> { new Resource { allocType = AllocType.FOOD, amount = 1f } };
    public new List<Resource> produce = new List<Resource> { new Resource { allocType = AllocType.FOOD, amount = 3f } };
}

[System.Serializable]
public class Turn
{
    public int year;
    public int total_population;
    public int currency;
    public int researchPoints;
    public int food;
    public int safety;
    public Worker worker;
    public Scientist scientist;
    public Planner planner;
    public Farmer farmer;
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    public Turn current_turn;
    public int current_total_population;
    public int current_currency;
    public int current_researchPoints;
    public int current_food;
    public int current_safety;

    public List<Turn> turnList = new List<Turn>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        //Testing values
        current_total_population = 10000;
        current_currency = 1000;
        current_researchPoints = 1000;
        current_food = 1000;
        current_safety = 0;

        //Test turn
        NewTurn();
    }

    public void NewTurn()
    {
        Turn turn = new Turn();
        turn.worker = new Worker();
        turn.scientist = new Scientist();
        turn.planner = new Planner();
        turn.farmer = new Farmer();

        turn.year = YearData._INSTANCE.current_year;
        turn.total_population = current_total_population;
        turn.currency = current_currency;
        turn.researchPoints = current_researchPoints;
        turn.food = current_food;
        turn.safety = current_safety;

        //Debug.Log(YearData._INSTANCE.current_year);

        current_turn = turn;

        //Update counters
        ComputerController.Instance.foodText.text = current_turn.food.ToString();
        ComputerController.Instance.rpText.text = current_turn.researchPoints.ToString();
        ComputerController.Instance.currencyText.text = current_turn.currency.ToString();
        ComputerController.Instance.populationText.text = current_turn.total_population.ToString();
    }

    public void AllocatePopulation(float workerPopulation, float scientistPopulation, float plannerPopulation, float farmerPopulation)
    {
        if(current_turn == null)
        {
            Debug.Log("No turn available");
            return;
        }

        workerPopulation = RAUtility.Remap(workerPopulation, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
        scientistPopulation = RAUtility.Remap(scientistPopulation, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
        plannerPopulation = RAUtility.Remap(plannerPopulation, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);
        farmerPopulation = RAUtility.Remap(farmerPopulation, 0f, ComputerController.Instance.totalPointsLimit, 0f, 1f);

        //Worker
        {
            current_turn.worker.population = (int)(workerPopulation * current_turn.total_population);

            foreach (var uk in current_turn.worker.upkeep)
            {
                uk.amount = uk.amount * (workerPopulation);
                Debug.Log("Workers upkeep " + uk.allocType.ToString() + " = " + uk.amount.ToString());
            }

            foreach (var p in current_turn.worker.produce)
            {
                p.amount = p.amount * (workerPopulation * current_turn.currency);
                Debug.Log("Workers produce " + p.allocType.ToString() + " = " + p.amount.ToString());
            }
        }
        //Scientist
        {
            current_turn.scientist.population = (int)(scientistPopulation * current_turn.total_population);

            foreach (var uk in current_turn.scientist.upkeep)
            {
                uk.amount = uk.amount * (scientistPopulation);
                Debug.Log("Scientists upkeep " + uk.allocType.ToString() + " = " + uk.amount.ToString());
            }

            foreach (var p in current_turn.scientist.produce)
            {
                p.amount = p.amount * (scientistPopulation * current_turn.currency);
                Debug.Log("Scientists produce " + p.allocType.ToString() + " = " + p.amount.ToString());
            }
        }
        //Planner
        {
            current_turn.planner.population = (int)(plannerPopulation * current_turn.total_population);

            foreach (var uk in current_turn.planner.upkeep)
            {
                uk.amount = uk.amount * (plannerPopulation);
                Debug.Log("Planners upkeep " + uk.allocType.ToString() + " = " + uk.amount.ToString());
            }

            foreach (var p in current_turn.planner.produce)
            {
                p.amount = p.amount * (plannerPopulation * current_turn.currency);
                Debug.Log("Planners produce " + p.allocType.ToString() + " = " + p.amount.ToString());
            }
        }
        //Farmer
        {
            current_turn.farmer.population = (int)(farmerPopulation * current_turn.total_population);

            foreach (var uk in current_turn.farmer.upkeep)
            {
                uk.amount = uk.amount * (farmerPopulation);
                Debug.Log("Farmers upkeep " + uk.allocType.ToString() + " = " + uk.amount.ToString());
            }

            foreach (var p in current_turn.farmer.produce)
            {
                p.amount = p.amount * (farmerPopulation * current_turn.currency);
                Debug.Log("Farmers produce " + p.allocType.ToString() + " = " + p.amount.ToString());
            }
        }

        // Replace turn if it exists
        if(turnList.Contains(current_turn))
            turnList.Remove(current_turn);

        turnList.Add(current_turn);
        turnList.Sort(SortByYear);
    }

    private static int SortByYear(Turn bud1, Turn bud2)
    {
        return bud1.year.CompareTo(bud2.year);
    }
}
