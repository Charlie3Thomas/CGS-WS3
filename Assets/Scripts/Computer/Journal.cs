using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Journal : MonoBehaviour
{
    private bool onJournal = false;
    private string input;
    private GameObject keyboard;
    private List<GameObject> keys = new List<GameObject>();

    private void Start()
    {
        keyboard = GameObject.FindGameObjectWithTag("Keyboard");

        for (int i = 0; i < keyboard.transform.childCount; i++)
        {
            keys.Add(keyboard.transform.GetChild(i).GetChild(0).gameObject);
        }
    }

    public void ReadStringInput(string s)
    {
        input = s;
        int randomKey = Random.Range(0, keyboard.transform.childCount);
        keys[randomKey].GetComponent<Animator>().SetTrigger("Press");
    }
}
