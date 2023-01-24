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

    public float pointValue;

    private void Start()
    {
        pipMat = pip.GetComponent<Renderer>().material;
    }

    public void AddPoints(float points)
    {
        if(pointValue < 5)
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