using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CT.Lookup;
using CT.Data;
using CT;

public class Journal : MonoBehaviour
{
    [SerializeField] private TMP_Text facText;
    private GameObject keyboard;
    private List<GameObject> keys = new List<GameObject>();

    private void Start()
    {
        keyboard = GameObject.FindGameObjectWithTag("Keyboard");

        for (int i = 0; i < keyboard.transform.childCount; i++)
        {
            keys.Add(keyboard.transform.GetChild(i).GetChild(0).gameObject);
        }

        UpdateFactionProductionText();
    }

    public void UpdateFactionProductionText()
    {
        CTTurnData ctd = GameManager._INSTANCE.GetTurn();
        facText.text = $"Scientists produce: {(int)(DataSheet.SCIENTIST_NET.science * (ctd.Population * GameManager._INSTANCE.GetFactionDistribution().y)) * -1} ({(ctd.GetModifiers().y) * 100}% base science) \n" +
            $"Workers produce: {(int)(DataSheet.WORKER_NET.money * (ctd.Population * GameManager._INSTANCE.GetFactionDistribution().x)) * -1} ({(ctd.GetModifiers().x) * 100}% base money) \n" +
            $"Farmers produce: {(int)(DataSheet.FARMERS_NET.food * (ctd.Population * GameManager._INSTANCE.GetFactionDistribution().z)) * -1} ({(ctd.GetModifiers().z) * 100}% base food)";
    }

    public void ReadStringInput(string _s)
    {
        int randomKey = Random.Range(0, keyboard.transform.childCount);
        keys[randomKey].GetComponent<Animator>().SetTrigger("Press");
    }
}
