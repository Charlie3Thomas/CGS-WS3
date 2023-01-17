using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public static DisasterManager instance;

    public enum disasterType
    {
        FLOOD,
        EARTHQUAKE,
        VOLCANO,
        TORNADO,
        WILDFIRE
    }

    [System.Serializable]
    public class Disaster
    {
        public disasterType type;
        public int year;
        public float intensity;
    }

    public int numOfDisasters = 10;
    public List<Disaster> disasterList = new List<Disaster>();

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
        CreateDisasterList();
        WriteDisastersInJournal();
    }

    void WriteDisastersInJournal()
    {
        UIController.Instance.notepadText.text = "Disasters:\n";
        foreach (Disaster dis in disasterList)
        {
            UIController.Instance.notepadText.text += dis.type.ToString() + " - Year: " + dis.year + " - Intensity: " + dis.intensity.ToString("F1") + "\n";
        }
    }

    void CreateDisasterList()
    {
        for (int i = 0; i < numOfDisasters; i++)
        {
            Disaster dis = new Disaster();
            dis.type = (disasterType)Random.Range(0, System.Enum.GetValues(typeof(disasterType)).Length);
            dis.year = Random.Range(YearData._INSTANCE.earliest_year, YearData._INSTANCE.latest_year);
            dis.intensity = Random.Range(1f, 10f);
            disasterList.Add(dis);
            var dis1 = dis;
            disasterList.Sort(SortByYear);
        }
    }

    private static int SortByYear(Disaster dis1, Disaster dis2)
    {
        return dis1.year.CompareTo(dis2.year);
    }
}
