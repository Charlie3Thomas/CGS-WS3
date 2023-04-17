using CT.Lookup;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class BuffsNerfs
{
    public List<BuffsNerfsType> type;
    public List<float> amount;

    public BuffsNerfs(List<BuffsNerfsType> newType, List<float> newAmount)
    {
        type = newType;
        amount = newAmount;

        if (type.Count != amount.Count)
        {
            throw new ArgumentException("BuffsNerfs need to have same number of type and ammount.");
        }
    }
}
