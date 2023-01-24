using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AllocType
{
    MONEY,
    FOOD,
    ELEMENT,
    RESEARCH_POINT
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
    public new List<Resource> upkeep = new List<Resource> { new Resource { allocType = AllocType.FOOD, amount = 2f } };
    public new List<Resource> produce = new List<Resource> { new Resource { allocType = AllocType.MONEY, amount = 2f }, new Resource { allocType = AllocType.ELEMENT, amount = 0.5f } };
}

[System.Serializable]
public class Scientist : Person
{
    public new List<Resource> upkeep = new List<Resource> { new Resource { allocType = AllocType.FOOD, amount = 2f }, new Resource { allocType = AllocType.ELEMENT, amount = 2f } };
    public new List<Resource> produce = new List<Resource> { new Resource { allocType = AllocType.RESEARCH_POINT, amount = 3f } };
}

[System.Serializable]
public class Planner : Person
{
    public new List<Resource> upkeep = new List<Resource> { new Resource { allocType = AllocType.FOOD, amount = 2f }, new Resource { allocType = AllocType.RESEARCH_POINT, amount = 2f } };
    public new List<Resource> produce = new List<Resource> { new Resource { allocType = AllocType.ELEMENT, amount = 2f } };
}

[System.Serializable]
public class Farmer : Person
{
    public new List<Resource> upkeep = new List<Resource> { new Resource { allocType = AllocType.FOOD, amount = 1f }, new Resource { allocType = AllocType.ELEMENT, amount = 1f } };
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
    public int elements;
    public Worker worker;
    public Scientist scientist;
    public Planner planner;
    public Farmer farmer;
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    public int current_total_population;
    public int current_currency;
    public int current_researchPoints;
    public int current_food;
    public int current_elements;

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
        current_total_population = Random.Range(1000, 10000);
        current_currency = Random.Range(1000, 10000);
        current_researchPoints = Random.Range(1000, 10000);
        current_food = Random.Range(1000, 10000);
        current_elements = Random.Range(1000, 10000);

        //Test
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
        turn.elements = current_elements;

        //Testing values until prompt
        AllocatePopulation(turn, Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        turnList.Add(turn);
        turnList.Sort(SortByYear);
    }

    public void AllocatePopulation(Turn turn, float workerPopulation, float scientistPopulation, float plannerPopulation, float farmerPopulation)
    {
        //Worker
        {
            turn.worker.population = (int)(workerPopulation * turn.total_population);

            foreach (var uk in turn.worker.upkeep)
            {
                uk.amount = uk.amount * (workerPopulation);
                Debug.Log("Workers upkeep " + uk.allocType.ToString() + " = " + uk.amount.ToString());
            }

            foreach (var p in turn.worker.produce)
            {
                p.amount = p.amount * (workerPopulation * turn.currency);
                Debug.Log("Workers produce " + p.allocType.ToString() + " = " + p.amount.ToString());
            }
        }
        //Scientist
        {
            turn.scientist.population = (int)(scientistPopulation * turn.total_population);

            foreach (var uk in turn.scientist.upkeep)
            {
                uk.amount = uk.amount * (scientistPopulation);
                Debug.Log("Scientists upkeep " + uk.allocType.ToString() + " = " + uk.amount.ToString());
            }

            foreach (var p in turn.scientist.produce)
            {
                p.amount = p.amount * (scientistPopulation * turn.currency);
                Debug.Log("Scientists produce " + p.allocType.ToString() + " = " + p.amount.ToString());
            }
        }
        //Planner
        {
            turn.planner.population = (int)(plannerPopulation * turn.total_population);

            foreach (var uk in turn.planner.upkeep)
            {
                uk.amount = uk.amount * (plannerPopulation);
                Debug.Log("Planners upkeep " + uk.allocType.ToString() + " = " + uk.amount.ToString());
            }

            foreach (var p in turn.planner.produce)
            {
                p.amount = p.amount * (plannerPopulation * turn.currency);
                Debug.Log("Planners produce " + p.allocType.ToString() + " = " + p.amount.ToString());
            }
        }
        //Farmer
        {
            turn.farmer.population = (int)(farmerPopulation * turn.total_population);

            foreach (var uk in turn.farmer.upkeep)
            {
                uk.amount = uk.amount * (farmerPopulation);
                Debug.Log("Farmers upkeep " + uk.allocType.ToString() + " = " + uk.amount.ToString());
            }

            foreach (var p in turn.farmer.produce)
            {
                p.amount = p.amount * (farmerPopulation * turn.currency);
                Debug.Log("Farmers produce " + p.allocType.ToString() + " = " + p.amount.ToString());
            }
        }
    }

    private static int SortByYear(Turn bud1, Turn bud2)
    {
        return bud1.year.CompareTo(bud2.year);
    }
}
