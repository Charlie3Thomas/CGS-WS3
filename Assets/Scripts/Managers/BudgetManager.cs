using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person
{
    public int population;
    public int upkeep;
    public int produce;
}

[System.Serializable]
public class Worker : Person
{

}

[System.Serializable]
public class Scientist : Person
{

}

[System.Serializable]
public class Planner : Person
{

}

[System.Serializable]
public class Farmer : Person
{

}

public class BudgetManager : MonoBehaviour
{
    [System.Serializable]
    public class Budget
    {
        public int year;
        public Worker worker;
        public Scientist scientist;
        public Planner planner;
        public Farmer farmer;
    }

    public List<Budget> budgetList = new List<Budget>();

    void Start()
    {
        NewBudget();
    }

    public void NewBudget()
    {
        Budget bud = new Budget();
        bud.year = YearData._INSTANCE.current_year;
        budgetList.Add(bud);
        var bud1 = bud;
        budgetList.Sort(SortByYear);
    }

    private static int SortByYear(Budget bud1, Budget bud2)
    {
        return bud1.year.CompareTo(bud2.year);
    }
}
