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
    public float allocation;
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

public class BudgetManager : MonoBehaviour
{
    public static BudgetManager instance;

    [System.Serializable]
    public class Budget
    {
        public int current_budget;
        public int year;
        public Worker worker;
        public Scientist scientist;
        public Planner planner;
        public Farmer farmer;
    }

    public List<Budget> budgetList = new List<Budget>();

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
        //Test
        NewBudget();
    }

    public void NewBudget()
    {
        Budget bud = new Budget();
        bud.worker = new Worker();
        bud.scientist = new Scientist();
        bud.planner = new Planner();
        bud.farmer = new Farmer();

        //Use actual total money from economy manager possibly
        bud.current_budget = Random.Range(1000, 10000);
        bud.year = YearData._INSTANCE.current_year;

        //Testing values
        bud.worker.population = Random.Range(0, 100);
        bud.scientist.population = Random.Range(0, 100);
        bud.planner.population = Random.Range(0, 100);
        bud.farmer.population = Random.Range(0, 100);

        AllocateBudget(bud, 24f, 26f, 13f, 37f);
        budgetList.Add(bud);
        budgetList.Sort(SortByYear);
    }

    public void AllocateBudget(Budget bud, float workerAlloc, float scientistAlloc, float plannerAlloc, float farmerAlloc)
    {
        //Worker
        {
            bud.worker.allocation = workerAlloc;

            foreach(var uk in bud.worker.upkeep)
            {
                uk.amount = uk.amount * (bud.worker.population);
                Debug.Log("Workers upkeep " + uk.allocType.ToString() + " = " + uk.amount.ToString());
            }

            foreach (var p in bud.worker.produce)
            {
                p.amount = p.amount * ((workerAlloc / 100) * bud.current_budget * bud.worker.population);
                Debug.Log("Workers produce " + p.allocType.ToString() + " = " + p.amount.ToString());
            }
        }
        //Scientist
        {
            bud.scientist.allocation = scientistAlloc;

            foreach (var uk in bud.scientist.upkeep)
            {
                uk.amount = uk.amount * (bud.scientist.population);
                Debug.Log("Scientists upkeep " + uk.allocType.ToString() + " = " + uk.amount.ToString());
            }

            foreach (var p in bud.scientist.produce)
            {
                p.amount = p.amount * ((scientistAlloc / 100) * bud.current_budget * bud.scientist.population);
                Debug.Log("Scientists produce " + p.allocType.ToString() + " = " + p.amount.ToString());
            }
        }
        //Planner
        {
            bud.planner.allocation = plannerAlloc;

            foreach (var uk in bud.planner.upkeep)
            {
                uk.amount = uk.amount * (bud.planner.population);
                Debug.Log("Planners upkeep " + uk.allocType.ToString() + " = " + uk.amount.ToString());
            }

            foreach (var p in bud.planner.produce)
            {
                p.amount = p.amount * ((plannerAlloc / 100) * bud.current_budget * bud.planner.population);
                Debug.Log("Planners produce " + p.allocType.ToString() + " = " + p.amount.ToString());
            }
        }
        //Farmer
        {
            bud.farmer.allocation = farmerAlloc;

            foreach (var uk in bud.farmer.upkeep)
            {
                uk.amount = uk.amount * (bud.farmer.population);
                Debug.Log("Farmers upkeep " + uk.allocType.ToString() + " = " + uk.amount.ToString());
            }

            foreach (var p in bud.farmer.produce)
            {
                p.amount = p.amount * ((farmerAlloc / 100) * bud.current_budget * bud.farmer.population);
                Debug.Log("Farmers produce " + p.allocType.ToString() + " = " + p.amount.ToString());
            }
        }
    }

    private static int SortByYear(Budget bud1, Budget bud2)
    {
        return bud1.year.CompareTo(bud2.year);
    }
}
