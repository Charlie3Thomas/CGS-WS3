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

public class Value
{
    public AllocType allocType;
    public float value;
}

public class Person
{
    public float allocation;
    public int population;
    public List<Value> upkeep;
    public List<Value> produce;
}

[System.Serializable]
public class Worker : Person
{
    public new List<Value> upkeep = new List<Value> { new Value { allocType = AllocType.FOOD, value = 2f } };
    public new List<Value> produce = new List<Value> { new Value { allocType = AllocType.MONEY, value = 2f }, new Value { allocType = AllocType.ELEMENT, value = 0.5f } };
}

[System.Serializable]
public class Scientist : Person
{
    public new List<Value> upkeep = new List<Value> { new Value { allocType = AllocType.FOOD, value = 2f }, new Value { allocType = AllocType.ELEMENT, value = 2f } };
    public new List<Value> produce = new List<Value> { new Value { allocType = AllocType.RESEARCH_POINT, value = 3f } };
}

[System.Serializable]
public class Planner : Person
{
    public new List<Value> upkeep = new List<Value> { new Value { allocType = AllocType.FOOD, value = 2f }, new Value { allocType = AllocType.RESEARCH_POINT, value = 2f } };
    public new List<Value> produce = new List<Value> { new Value { allocType = AllocType.ELEMENT, value = 2f } };
}

[System.Serializable]
public class Farmer : Person
{
    public new List<Value> upkeep = new List<Value> { new Value { allocType = AllocType.FOOD, value = 1f }, new Value { allocType = AllocType.ELEMENT, value = 1f } };
    public new List<Value> produce = new List<Value> { new Value { allocType = AllocType.FOOD, value = 3f } };
}

public class BudgetManager : MonoBehaviour
{
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
        var bud1 = bud;
        budgetList.Sort(SortByYear);
    }

    public void AllocateBudget(Budget bud, float workerAlloc, float scientistAlloc, float plannerAlloc, float farmerAlloc)
    {
        //Worker
        {
            bud.worker.allocation = workerAlloc;

            foreach(var uk in bud.worker.upkeep)
            {
                uk.value = uk.value * (bud.worker.population);
                Debug.Log("Workers upkeep " + uk.allocType.ToString() + " = " + uk.value.ToString());
            }

            foreach (var p in bud.worker.produce)
            {
                p.value = p.value * ((workerAlloc / 100) * bud.current_budget * bud.worker.population);
                Debug.Log("Workers produce " + p.allocType.ToString() + " = " + p.value.ToString());
            }
        }
        //Scientist
        {
            bud.scientist.allocation = scientistAlloc;

            foreach (var uk in bud.scientist.upkeep)
            {
                uk.value = uk.value * (bud.scientist.population);
                Debug.Log("Scientists upkeep " + uk.allocType.ToString() + " = " + uk.value.ToString());
            }

            foreach (var p in bud.scientist.produce)
            {
                p.value = p.value * ((scientistAlloc / 100) * bud.current_budget * bud.scientist.population);
                Debug.Log("Scientists produce " + p.allocType.ToString() + " = " + p.value.ToString());
            }
        }
        //Planner
        {
            bud.planner.allocation = plannerAlloc;

            foreach (var uk in bud.planner.upkeep)
            {
                uk.value = uk.value * (bud.planner.population);
                Debug.Log("Planners upkeep " + uk.allocType.ToString() + " = " + uk.value.ToString());
            }

            foreach (var p in bud.planner.produce)
            {
                p.value = p.value * ((plannerAlloc / 100) * bud.current_budget * bud.planner.population);
                Debug.Log("Planners produce " + p.allocType.ToString() + " = " + p.value.ToString());
            }
        }
        //Farmer
        {
            bud.farmer.allocation = farmerAlloc;

            foreach (var uk in bud.farmer.upkeep)
            {
                uk.value = uk.value * (bud.farmer.population);
                Debug.Log("Farmers upkeep " + uk.allocType.ToString() + " = " + uk.value.ToString());
            }

            foreach (var p in bud.farmer.produce)
            {
                p.value = p.value * ((farmerAlloc / 100) * bud.current_budget * bud.farmer.population);
                Debug.Log("Farmers produce " + p.allocType.ToString() + " = " + p.value.ToString());
            }
        }
    }

    private static int SortByYear(Budget bud1, Budget bud2)
    {
        return bud1.year.CompareTo(bud2.year);
    }
}
