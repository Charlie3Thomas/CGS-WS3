using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RAUtility
{
    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public struct Vector4List
    {
        public List<float> x;
        public List<float> y;
        public List<float> z;
        public List<float> w;

        public Vector4List(List<float> x, List<float> y, List<float> z, List<float> w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }

    public static List<float> GetListWithMaxValue(List<float> list1, List<float> list2, List<float> list3, List<float> list4)
    {
        List<float> lists = new List<float>();
        lists.AddRange(list1);
        lists.AddRange(list2);
        lists.AddRange(list3);
        lists.AddRange(list4);

        float maxVal = lists.Max();
        List<float> maxList = null;

        if (list1.Contains(maxVal))
            maxList = list1;
        else if (list2.Contains(maxVal))
            maxList = list2;
        else if (list3.Contains(maxVal))
            maxList = list3;
        else if (list4.Contains(maxVal))
            maxList = list4;

        return maxList;
    }
}
