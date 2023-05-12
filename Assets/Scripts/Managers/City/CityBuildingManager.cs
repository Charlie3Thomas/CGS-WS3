using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBuildingManager : MonoBehaviour
{
    [Header("Size of the generated sphere primitives.")]
    public Vector3 scaleFactor = new Vector3(0.5f, 0.5f, 0.5f);

    [Header("BuildingPrefabs")]
    public List<GameObject> BuildingPrefabs;

    [Header("Transform parent of instantiated objects")]
    public Transform ObjectsParent;


    public List<GameObject> BuildingObjects;
    public Transform[] EmptyObjects;
    private int buildingCounts = 0;
    List<int> ints = new List<int>();

    public static CityBuildingManager Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    void Start()
    {
        EmptyObjects = ObjectsParent.GetComponentsInChildren<Transform>();
        GenerateBuildings();
    }

    void GenerateBuildings()
    {
        Debug.Log("Generating BUildings");
        for(int i=1; i<EmptyObjects.Length; i++)
        {
            int indexO = Random.Range(0, BuildingPrefabs.Count - 1);

            GameObject building = Instantiate(BuildingPrefabs[indexO]);
            building.transform.position = EmptyObjects[i].transform.position;
            if (building.name.ToLower().Contains("small"))
            {
                building.transform.localScale = new Vector3(scaleFactor.x + 0.25f,
                    scaleFactor.y + 0.25f,
                    scaleFactor.z + 0.25f);
            }
            else
            {
                building.transform.localScale = scaleFactor;
            }

            //building.GetComponent<Renderer>().sharedMaterial.color = colorPrimitives;
            building.transform.SetParent(ObjectsParent);
            float randomYRotation = Random.Range(0f, 360f);
            building.transform.Rotate(0, 0, randomYRotation);

            Material material = building.GetComponent<Renderer>().material;
            material.color = new Color(Random.value, Random.value, Random.value);
            building.SetActive(true);
            building.name = building.name + "_" + i.ToString();
            BuildingObjects.Add(building);
        }

    }

    public void UpdatePopulation(int population)
    {
        //Debug.Log("POPULATION UPDATE : " + population);
        buildingCounts = (int)(population * 0.25f);
        if (buildingCounts <= 0) { buildingCounts = 10; }

        for (int i = 0; i < BuildingObjects.Count; i++)
        {
            BuildingObjects[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < buildingCounts; i++)
        {
            System.Random random = new System.Random();
            int minValue = 0;  // inclusive
            int maxValue = BuildingObjects.Count;  // exclusive
            int randomNumber = random.Next(minValue, maxValue);
            ints.Add(randomNumber);
            BuildingObjects[i].gameObject.SetActive(true);
        }
    }
}
