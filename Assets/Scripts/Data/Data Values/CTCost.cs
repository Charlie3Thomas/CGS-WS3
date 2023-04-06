using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTCost 
{
    public CTCost() { }

    public CTCost(int _money, int _science, int _food, int _population)
    {
        money = _money;
        science = _science;
        food = _food;
        population = _population;
    }


    public int money;
    public int science;
    public int food;
    public int population;

    public static CTCost operator* (CTCost _c, int m)
    {
        CTCost ret = new CTCost();

        ret.money       = _c.money      * m;
        ret.science     = _c.science    * m;
        ret.food        = _c.food       * m;
        ret.population  = _c.population * m;

        return ret;
    }

    public static CTCost operator+ (CTCost _c1, CTCost _c2)
    {
        CTCost ret = new CTCost();

        ret.money       = _c1.money         + _c2.money;
        ret.science     = _c1.science       + _c2.science;
        ret.food        = _c1.food          + _c2.food;
        ret.population  = _c1.population    + _c2.population;

        return ret;
    }

}
