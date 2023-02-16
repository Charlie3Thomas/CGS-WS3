using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffsNerfsType
{
    MONEY_CAPACITY,
    SCIENCE_CAPACITY,
    MONEY_GAIN,
    FOOD_GAIN,
    SCIENCE_GAIN,
    MONEY_UPKEEP,
    FOOD_UPKEEP,
    SCIENCE_UPKEEP,
    FOOD_RESERVES,
    SAFETY_FACTOR,
    AWARENESS_FACTOR,
    MONEY_BOOST,
    SCIENCE_BOOST,
    MAGNITUDE_FACTOR
}

[System.Serializable]
public class BuffsNerfs
{
    public BuffsNerfsType type;
    public float amount;

    public BuffsNerfs(BuffsNerfsType newType, float newAmount)
    {
        type = newType;
        amount = newAmount;
    }
}
