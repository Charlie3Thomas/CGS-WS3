using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DisasterManager : MonoBehaviour
{
    public static DisasterManager _INSTANCE;

    public enum disasterType
    {
        FLOOD,
        EARTHQUAKE,
        VOLCANO,
        TORNADO
    }

    [Serializable]
    public class Disaster
    {
        public disasterType type;
        public int year;
        public float intensity;
    }

    public int numOfDisasters = 10;
    public List<Disaster> disasterList;

    void Start()
    {
        disasterList = new List<Disaster>();
        for (int i = 0; i < numOfDisasters; i++)
        {
            Disaster dis = new Disaster();
            dis.type = (disasterType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(disasterType)).Length);
            dis.year = UnityEngine.Random.Range(YearData._INSTANCE.earliest_year, YearData._INSTANCE.latest_year);
            dis.intensity = UnityEngine.Random.Range(1f, 10f);
            disasterList.Add(dis);
        }
    }
}
