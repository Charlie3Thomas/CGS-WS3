using CT;
using System;
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
    public GameObject pip;
    public Material pipMat;

    //[HideInInspector]
    public float pointValue;
    private float pointLimit = 10f;

    private void Awake()
    {
        pipMat = pip.GetComponent<Renderer>().materials[1];
    }

    public void AddPoints(float points)
    {
        if (pointValue < pointLimit)
            pointValue += points;

        if (pointValue > 10.0f)
            pointValue = 10.0f;

        if (pipMat != null)
            pipMat.SetFloat("_FillAmount", pointValue);

        ComputerController.Instance.CheckPoints(this);
    }

    public void RemovePoints(float points)
    {

        if(pointValue > 0)
            pointValue -= points;

        if (pointValue < 0)
            pointValue = 0;

        if (pipMat != null)
            pipMat.SetFloat("_FillAmount", pointValue);

        ComputerController.Instance.CheckPoints(this);
    }

    public void SetPoints(float _points)
    {
        if (pointValue < 0)
            throw new ArgumentException("Point value is out of range");

        pointValue = _points;

        if (pointValue > 10.0f)
            pointValue = 10.0f;

        if (pipMat != null)
        {
            pipMat.SetFloat("_FillAmount", pointValue);
        }
        else
            Debug.LogError("pipMat is null");
    }
}