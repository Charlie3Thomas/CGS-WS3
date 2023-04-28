using CT;
using CT.Lookup;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public static DisasterManager instance;

    public int numOfDisasters = 10;
    public List<Disaster> disasterList = new List<Disaster>();
    public bool showMagnitude = false;
    public bool showDeathToll = false;
    public bool showSafety = false;

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
    private void Update()
    {
        //WriteSafetyInJournal();  // To implement - Display safety value in journal when show_safetyunlocked
    }

    //public void WriteSafetyInJournal()
    //{

    //    if (showSafety)
    //    {
            
    //    }
    //}
    // Call this to update the list visually whenever something new happens
    public void WriteDisastersInJournal()
    {
        ComputerController.Instance.disasterNameText.text = "";
        ComputerController.Instance.disasterYearText.text = "";
        ComputerController.Instance.disasterMagnitudeText.text = "";
        ComputerController.Instance.disasterDeathTollText.text = "";

        foreach (Disaster dis in disasterList)
        {
            ComputerController.Instance.disasterNameText.text += dis.type.ToString() + "\n";
            ComputerController.Instance.disasterYearText.text += dis.year + "\n";

            //if (showMagnitude)
            //    ComputerController.Instance.disasterMagnitudeText.text += dis.magnitude.ToString("F1") + "\n";
            //else
            //    ComputerController.Instance.disasterMagnitudeText.text += "???\n";

            // Random for now, change later when everything gets hooked up
            if (showDeathToll)
                ComputerController.Instance.disasterDeathTollText.text += Random.Range(0, 1000) + "\n";
            else
                ComputerController.Instance.disasterDeathTollText.text += "???\n";
        }
    }

    void CreateDisasterList()
    {
        HashSet<int> uniqueTurns = new HashSet<int>();
        for (int i = 0; i < numOfDisasters; i++)
        {
            Disaster dis = new Disaster();
            //dis.type = (CTDisasters)Random.Range(0, System.Enum.GetValues(typeof(CTDisasters)).Length);
            dis.type = (CTDisasters)CTSeed.RandFromSeed((uint)i, "dis.type").Next(System.Enum.GetValues(typeof(CTDisasters)).Length);
            //dis.year = (Random.Range((YearData._INSTANCE.earliest_year / 5), (YearData._INSTANCE.latest_year / 5) + 1) * 5);
            //while (!uniqueYears.Add(dis.year))
            //{
            //    //dis.year = (Random.Range((YearData._INSTANCE.earliest_year / 5), (YearData._INSTANCE.latest_year / 5) + 1) * 5);
            //}

            //dis.turn = Random.Range(4,  (int)CT.Lookup.DataSheet.turns_number);
            dis.turn = CTSeed.RandFromSeed((uint)i, "dis.turn").Next(4, (int)CT.Lookup.DataSheet.turns_number);
            while (!uniqueTurns.Add(dis.turn))
            {
                dis.turn = CTSeed.RandFromSeed((uint)i, "dis.turn").Next((int)CT.Lookup.DataSheet.turns_number);
            }

            dis.year = (int)(dis.turn * CT.Lookup.DataSheet.turn_steps + CT.Lookup.DataSheet.starting_year) + CTSeed.RandFromSeed((uint)i, "dis.year").Next(5);/*Random.Range(0, 5)*/;

            dis.intensity = CTSeed.RandFromSeed((uint)i, "dis.intensity").Next(1, 10);        /*Random.Range(1f, 10f);*/;

            disasterList.Add(dis);
        }
        disasterList.Sort(SortByYear);

        WriteDisastersToGameManager();
    }

    private static int SortByYear(Disaster dis1, Disaster dis2)
    {
        return dis1.year.CompareTo(dis2.year);
    }

    private void WriteDisastersToGameManager()
    {
        foreach (Disaster d in disasterList)
        {
            GameManager._INSTANCE.AddDisastersToGameChanges(d);
        }
    }
}
