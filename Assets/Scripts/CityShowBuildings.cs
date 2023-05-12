using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityShowBuildings : MonoBehaviour
{
    public int population = 1000;
    int p = 0;

    public CheckSphereRadius[] AddedObjects;
    private void Start()
    {
        AddedObjects = FindObjectsOfType<CheckSphereRadius>();
    }

    List<int> ints = new List<int>();
    private void Update()
    {
        p = (int)(population * 0.25f);
        if (p <= 0) { p = 10; }
        
        for (int i = 0; i < AddedObjects.Length; i++)
        {
            AddedObjects[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < p; i++)
        {
            System.Random random = new System.Random();
            int minValue = 0;  // inclusive
            int maxValue = AddedObjects.Length;  // exclusive
            int randomNumber = random.Next(minValue, maxValue);
            ints.Add(randomNumber);
            AddedObjects[i].gameObject.SetActive(true);
        }
        
    }

}


