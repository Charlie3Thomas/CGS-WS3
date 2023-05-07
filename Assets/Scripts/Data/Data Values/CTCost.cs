using CT.Data.Resources;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Money, Science, Food, Population
/// </summary>
public class CTCost 
{
    public CTCost() { }

    public CTCost(float _money, float _science, float _food, float _population)
    {
        money = _money;
        science = _science;
        food = _food;
        population = _population;
    }

    public CTCost(CTCost _copy)
    {
        money = _copy.money;
        science = _copy.science;
        food = _copy.science;
        population = _copy.population;
    }


    public float money;
    public float science;
    public float food;
    public float population;


    #region Operator Overrides
    #region CTCost Operators
    public static CTCost operator *(CTCost _c, float m)
    {
        CTCost ret = new CTCost();

        ret.money = _c.money * m;
        ret.science = _c.science * m;
        ret.food = _c.food * m;
        ret.population = _c.population * m;

        return ret;
    }

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

    public static CTCost operator- (CTCost _c1, CTCost _c2)
    {
        CTCost ret = new CTCost();

        ret.money       = _c1.money         - _c2.money;
        ret.science     = _c1.science       - _c2.science;
        ret.food        = _c1.food          - _c2.food;
        ret.population  = _c1.population    - _c2.population;

        return ret;
    }

    public static bool operator>= (CTCost _c1, CTCost _c2)
    {
        return (_c1.money       >= _c2.money &&
                _c1.science     >= _c2.science &&
                _c1.population  >= _c2.population &&
                _c1.food        >= _c2.food);
    }

    public static bool operator<= (CTCost _c1, CTCost _c2)
    {
        return (_c1.money       <= _c2.money &&
                _c1.science     <= _c2.science &&
                _c1.population  <= _c2.population &&
                _c1.food        <= _c2.food);
    }

    public static bool operator> (CTCost _c1, CTCost _c2)
    {
        return (_c1.money       > _c2.money &&
                _c1.science     > _c2.science &&
                _c1.population  > _c2.population &&
                _c1.food        > _c2.food);
    }

    public static bool operator< (CTCost _c1, CTCost _c2)
    {
        return (_c1.money       < _c2.money &&
                _c1.science     < _c2.science &&
                _c1.population  < _c2.population &&
                _c1.food        < _c2.food);
    }

    public static bool operator!= (CTCost _c1, CTCost _c2)
    {
        return (    _c1.money        != _c2.money &&
                    _c1.science      != _c2.science &&
                    _c1.population   != _c2.population &&
                    _c1.food         != _c2.food);
    }

    public static bool operator== (CTCost _c1, CTCost _c2)
    {
        return (    _c1.money       == _c2.money &&
                    _c1.science     == _c2.science &&
                    _c1.population  == _c2.population &&
                    _c1.food        == _c2.food);
    }
    #endregion


    #region CTCost vs. CTResourceTotal overrides
    public static bool operator>= (CTCost _c1, CTResourceTotals _r1)
    {
        return (_c1.money       >= _r1.money &&
                _c1.science     >= _r1.science &&
                _c1.population  >= _r1.population &&
                _c1.food        >= _r1.food);
    }

    public static bool operator<= (CTCost _c1, CTResourceTotals _r1)
    {
        return (_c1.money       <= _r1.money &&
                _c1.science     <= _r1.science &&
                _c1.population  <= _r1.population &&
                _c1.food        <= _r1.food);
    }

    public static bool operator> (CTCost _c1, CTResourceTotals _r1)
    {
        return (_c1.money       > _r1.money &&
                _c1.science     > _r1.science &&
                _c1.population  > _r1.population &&
                _c1.food        > _r1.food);
    }

    public static bool operator< (CTCost _c1, CTResourceTotals _r1)
    {
        return (_c1.money       < _r1.money &&
                _c1.science     < _r1.science &&
                _c1.population  < _r1.population &&
                _c1.food        < _r1.food);
    }

    #endregion

    #endregion

    #region Utility

    public string GetString()
    {
        string ret = "";

        if (money > 0)
            ret += "Money: " + money + " ";
        if (science > 0)
            ret += "Science: " + science + " ";
        if (food > 0)
            ret += "Food: " + food + " ";
        if (population > 0)
            ret += "Population: " + population + " ";

        return ret;
    }

    #endregion

}
