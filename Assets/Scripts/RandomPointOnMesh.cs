using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPointOnMesh : MonoBehaviour
{
    [Header("Mesh Collider of surface.")]
    public Collider m_collider;
    [Header("Maximal high of the surface. Raycast")]
    public float surfaceHigh = 1f; //
    [Header("Number of random points to generate on the surface.")]
    public int numPoints = 100;
    [Header("Maximal number of iterations to find the points.")]
    public int maxIterations = 1000;
    [Header("Size of the generated sphere primitives.")]
    public Vector3 scaleFactor = new Vector3(2f, 2f, 2f);
    [Header("Box colliders to ignore.")]
    public List<BoxCollider> ignoreColliders;
    [Header("Transform parent of instantiated objects")]
    public Transform ObjectsParent;

    private bool isUnityTerrain = false;
    private float bboxScale = 1f;

    [Header("BuildingPrefabs")]
    public List<GameObject> BuildingPrefabs;

    public void GenerateRandomPositions()
    {
        // Check if we have a Unity terrain
        Terrain terrain = GetComponent<Terrain>();
        if (terrain != null)
        {
            surfaceHigh = terrain.terrainData.size.y;
            isUnityTerrain = true;
        }

        if (ObjectsParent.childCount > 0)
        {
            for (int i=0;i< ObjectsParent.childCount;i++)
            {
                DestroyImmediate(ObjectsParent.GetChild(i));
            }
        }
        HashSet<Vector3> points = new HashSet<Vector3>();

        Vector3 pointRandom;
        Vector3 pointOnSurface = Vector3.zero;
        bool pointFound = false;
        int indexPoints = 0;
        int indexLoops = 0;
        do
        {
            indexLoops++;
            // Double the size of the bounding box here to get better results if not a Unity terrain
            if (isUnityTerrain) bboxScale = 1f;
            else bboxScale = 2f;

            do
            {
                if (isUnityTerrain) pointRandom = RandomPointInBounds(m_collider.bounds, bboxScale);
                else pointRandom = RandomPointInBounds(m_collider.bounds, bboxScale) - transform.position;
            } while (points.Contains(pointRandom) || IsPointInAnyIgnoreCollider(pointRandom));

            pointFound = GetRandomPointOnColliderSurface(pointRandom, out pointOnSurface);

            if (pointFound)
            {
                points.Add(pointRandom);
                indexPoints++;

                int indexO = Random.Range(0, BuildingPrefabs.Count - 1);

                GameObject building = Instantiate(BuildingPrefabs[indexO]);
                building.transform.position = pointOnSurface;
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
                building.name = building.name + "_" + indexPoints.ToString();
            }
        } while ((indexPoints < numPoints) && (indexLoops < maxIterations));
    }

    private bool IsPointInAnyIgnoreCollider(Vector3 point)
    {
        foreach (BoxCollider collider in ignoreColliders)
        {
            if (collider.bounds.Contains(point)) return true;
        }

        return false;
    }

    private bool GetRandomPointOnColliderSurface(Vector3 point, out Vector3 pointSurface)
    {

        Vector3 pointOnSurface = Vector3.zero;
        RaycastHit hit;
        bool pointFound = false;
        // Raycast against the surface of the transform
        if (Physics.Raycast(point - surfaceHigh * transform.up, transform.up, out hit, Mathf.Infinity))
        {
            pointOnSurface = hit.point;
            pointFound = true;
        }
        else
        {
            if (Physics.Raycast(point + surfaceHigh * transform.up, -transform.up, out hit, Mathf.Infinity))
            {
                //Debug.Log("Found point -up");
                pointOnSurface = hit.point;
                pointFound = true;
            }
        }

        pointSurface = pointOnSurface;
        return pointFound;
    }

    private Vector3 RandomPointInBounds(Bounds bounds, float scale)
    {
        return new Vector3(
            Random.Range(bounds.min.x * scale, bounds.max.x * scale),
            Random.Range(bounds.min.y * scale, bounds.max.y * scale),
            Random.Range(bounds.min.z * scale, bounds.max.z * scale)
        );
    }

  

}