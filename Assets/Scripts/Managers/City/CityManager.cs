using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    public int population;
    private int lastPopulation;
    public int populationPerFloor = 10;
    public int maxBuildingCount = 100;
    public float minDistanceBetweenBuildings = 1f;

    public GameObject buildingPrefab;

    public Transform CityParent;

    private List<Building> buildings = new List<Building>();

    private void Start()
    {
        lastPopulation = population;
        CreateInitialBuildings();
    }

    private void Update()
    {
        if (lastPopulation != population)
        {
            UpdateCity();
            lastPopulation = population;
        }
    }

    private Vector3 ApplyRandomOffset(Vector3 position, float minDistance)
    {
        Vector3 offset;
        do
        {
            offset = new Vector3(Random.Range(-1f, 1f), 0.35f, Random.Range(-1f, 1f));
        } while (offset.magnitude < minDistance);

        return position + offset;
    }

    private void CreateInitialBuildings()
    {
        int buildingCount = Mathf.Min(population / populationPerFloor, maxBuildingCount);
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(buildingCount));

        for (int i = 0; i < buildingCount; i++)
        {
            int x = i % gridSize;
            int z = i / gridSize;
            Vector3 position = new Vector3(x * 1.5f, 0.35f, z * 1.8f);
            position = ApplyRandomOffset(position, minDistanceBetweenBuildings);
            GameObject newBuilding = Instantiate(buildingPrefab, position, Quaternion.identity);
            newBuilding.transform.SetParent(CityParent);
            newBuilding.AddComponent<Building>();
            newBuilding.GetComponent<Building>().Initialise(newBuilding, populationPerFloor, buildingPrefab);
            buildings.Add(newBuilding.GetComponent<Building>());
        }
    }

    private void UpdateCity()
    {
        int targetBuildingCount = Mathf.Min(population / populationPerFloor, maxBuildingCount);
        int currentBuildingCount = buildings.Count;

        if (targetBuildingCount > currentBuildingCount)
        {
            int buildingsToAdd = targetBuildingCount - currentBuildingCount;
            int gridSize = Mathf.CeilToInt(Mathf.Sqrt(targetBuildingCount));

            for (int i = 0; i < buildingsToAdd; i++)
            {
                int x = (currentBuildingCount + i) % gridSize;
                int z = (currentBuildingCount + i) / gridSize;
                Vector3 position = new Vector3(x * 1.5f, 0.35f, z * 1.8f);
                position = ApplyRandomOffset(position, minDistanceBetweenBuildings);
                GameObject newBuilding = Instantiate(buildingPrefab, position, Quaternion.identity);
                newBuilding.transform.SetParent(CityParent);
                newBuilding.AddComponent<Building>();
                newBuilding.GetComponent<Building>().Initialise(newBuilding, populationPerFloor, buildingPrefab);
                buildings.Add(newBuilding.GetComponent<Building>());
            }
        }
        else if (targetBuildingCount < currentBuildingCount)
        {
            int buildingsToRemove = currentBuildingCount - targetBuildingCount;

            for (int i = 0; i < buildingsToRemove; i++)
            {
                Building buildingToRemove = buildings[buildings.Count - 1];
                buildingToRemove.DestroyBuilding();
                buildings.RemoveAt(buildings.Count - 1);
            }
        }

        DistributePopulation();
    }

    private void DistributePopulation()
    {
        if(buildings.Count == 0) return;
        int totalFloors = population / populationPerFloor;
        int minFloorsPerBuilding = totalFloors / buildings.Count;

        foreach (Building building in buildings)
        {
            int floorsToAddOrRemove = minFloorsPerBuilding - building.FloorsCount;
            if (floorsToAddOrRemove > 0)
            {
                for (int i = 0; i < floorsToAddOrRemove; i++)
                {
                    building.AddFloor();
                }
            }
            else if (floorsToAddOrRemove < 0)
            {
                for (int i = 0; i < Mathf.Abs(floorsToAddOrRemove); i++)
                {
                    building.RemoveFloor();
                }
            }
        }

        int remainingFloors = totalFloors - (minFloorsPerBuilding * buildings.Count);
        for (int i = 0; i < remainingFloors; i++)
        {
            int randomBuildingIndex = Random.Range(0, buildings.Count);
            buildings[randomBuildingIndex].AddFloor();
        }
    }
}
