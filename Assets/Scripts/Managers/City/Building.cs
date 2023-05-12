using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject buildingObject;
    public int populationPerFloor;
    private List<GameObject> floors = new List<GameObject>();

    public int FloorsCount => floors.Count;

    public GameObject buildingPrefab;

    public void Initialise(GameObject buildingObject, int populationPerFloor, GameObject buildingPrefab)
    {
        this.buildingObject = buildingObject;
        this.populationPerFloor = populationPerFloor;
        this.buildingPrefab = buildingPrefab;;
    }

    public void AddFloor()
    {
        GameObject floor = Instantiate(buildingPrefab, transform.position, Quaternion.identity);
        floor.transform.SetParent(buildingObject.transform);
        floor.transform.localPosition = new Vector3(0, floors.Count, 0);
        floors.Add(floor);
    }

    public void RemoveFloor()
    {
        if (floors.Count > 0)
        {
            GameObject.Destroy(floors[floors.Count - 1]);
            floors.RemoveAt(floors.Count - 1);
        }
    }

    public void DestroyBuilding()
    {
        foreach (GameObject floor in floors)
        {
            GameObject.Destroy(floor);
        }
        GameObject.Destroy(buildingObject);
    }
}
