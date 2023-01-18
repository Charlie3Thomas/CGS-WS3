using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectorType
{
    FOOD,
    SCIENCE,
    PLANNER,
    WORKER
}

public class PointSelector : MonoBehaviour
{
    public SelectorType type = SelectorType.WORKER;

    public GameObject pip;
    private Material pipMat;

    private int points = 0;

    // Start is called before the first frame update
    void Start()
    {
        pipMat = pip.GetComponent<Renderer>().material;
    }

    public void IncreasePoint()
    {
        if (points < 5)
            points++;

        if (pipMat != null)
            pipMat.SetFloat("_FillAmount", points);
    }

    public void DecreasePoint()
    {
        if (points > 0)
            points--;

        if(pipMat != null)
            pipMat.SetFloat("_FillAmount", points);
    }
}
