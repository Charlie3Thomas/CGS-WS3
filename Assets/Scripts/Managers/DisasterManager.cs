using CT;
using CT.Lookup;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;

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
        
    }
    private void Update()
    {
        //WriteDisastersInJournal();
    }

    public void Generate()
    {
        CreateDisasterList();
        WriteDisastersInJournal();
    }

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

            if (showMagnitude)
                ComputerController.Instance.disasterMagnitudeText.text += dis.intensity.ToString("F1") + "\n";
            else
                ComputerController.Instance.disasterMagnitudeText.text += "???\n";

            if (showDeathToll)
                ComputerController.Instance.disasterDeathTollText.text += GameManager._INSTANCE.PGetYearData((uint)dis.turn).GetDeathToll() + "\n";
            else
                ComputerController.Instance.disasterDeathTollText.text += "???\n";
        }

        UpdateSafetyText();
    }

    public void UpdateSafetyText()
    {
        float safety = (GameManager._INSTANCE.GetTurn().Science >= (GameManager._INSTANCE.GetTurn().Population * GameManager._INSTANCE.GetFactionDistribution().w) * DataSheet.PLANNERS_NET.science) ?
            GameManager._INSTANCE.GetFactionDistribution().w : GameManager._INSTANCE.GetFactionDistribution().w * 0.5f;

        if (showSafety)
            ComputerController.Instance.safetyText.text = "Safety: " + ((safety) * 100).ToString() + "%";
        else
            ComputerController.Instance.safetyText.text = "Safety: ???";
    }

    void CreateDisasterList()
    {
        HashSet<int> uniqueTurns = new HashSet<int>();
        for (int i = 0; i < numOfDisasters; i++)
        {
            Disaster dis = new Disaster();
            //dis.type = (CTDisasters)Random.Range(0, System.Enum.GetValues(typeof(CTDisasters)).Length);
            dis.type = (CTDisasters)CTSeed.RandFromSeed((uint)i, "dis.type").Next(System.Enum.GetValues(typeof(CTDisasters)).Length - 1); // -1 to exclude the .NONE type
            //dis.year = (Random.Range((YearData._INSTANCE.earliest_year / 5), (YearData._INSTANCE.latest_year / 5) + 1) * 5);
            //while (!uniqueYears.Add(dis.year))
            //{
            //    //dis.year = (Random.Range((YearData._INSTANCE.earliest_year / 5), (YearData._INSTANCE.latest_year / 5) + 1) * 5);
            //}

            //dis.turn = Random.Range(4,  (int)CT.Lookup.DataSheet.turns_number);
            dis.turn = CTSeed.RandFromSeed((uint)i, "dis.turn").Next(4, (int)CT.Lookup.DataSheet.TURNS_NUMBER);
            //while (!uniqueTurns.Add(dis.turn))
            //{
            //    dis.turn = CTSeed.RandFromSeed((uint)i, "dis.turn").Next((int)CT.Lookup.DataSheet.turns_number);
            //}

            dis.year = (int)(dis.turn * CT.Lookup.DataSheet.TURN_STEPS + CT.Lookup.DataSheet.STARTING_YEAR) + CTSeed.RandFromSeed((uint)i, "dis.year").Next(5);/*Random.Range(0, 5)*/;

            dis.intensity = CTSeed.RandFromSeed((uint)i, "dis.intensity").Next(1, 10);              /*Random.Range(1f, 10f);*/;
            dis.intensity += (float)CTSeed.RandFromSeed((uint)i, "dis.intensity").NextDouble();

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
        GameManager._INSTANCE.AddDisastersToGameChanges(disasterList);
        //foreach (Disaster d in disasterList)
        //{
        //    GameManager._INSTANCE.AddDisastersToGameChanges(disasterList);
        //    //GameManager._INSTANCE.
        //}
    }
}
