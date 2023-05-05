using System.Collections;
using System.Collections.Generic;
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
}
