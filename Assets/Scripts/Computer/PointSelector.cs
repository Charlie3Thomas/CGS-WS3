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

    [HideInInspector]
    public float pointValue;
    private float pointLimit = 10f;

    private void Start()
    {
        pipMat = pip.GetComponent<Renderer>().materials[1];
    }

    public void AddPoints(float points)
    {
        if(pointValue < pointLimit)
            pointValue += points;

        if (pipMat != null)
            pipMat.SetFloat("_FillAmount", pointValue);

        ComputerController.Instance.CheckPoints(this);
    }

    public void RemovePoints(float points)
    {
        if(pointValue > 0)
            pointValue -= points;

        if (pipMat != null)
            pipMat.SetFloat("_FillAmount", pointValue);

        ComputerController.Instance.CheckPoints(this);
    }
}