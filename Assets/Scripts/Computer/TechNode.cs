using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechNode : MonoBehaviour
{
    private TechTree tree;
    public int pointsRequired = 0;
    public bool unlocked;
    public TechNode[] requiredNodes;
    private List<LineRenderer> lineRenderers;
    public Material lineMat;
    private float lineWidth = 1.5f;
    public AllocType effectType;
    public float effect;

    private Material mat;
    private Color faded = new Color(0.5f, 0.5f, 0.5f) * 1;
    public Color Lit;

    private void Start()
    {
        tree = GameObject.FindGameObjectWithTag("TechTree").GetComponent<TechTree>();
        lineRenderers = new List<LineRenderer>();
        mat = GetComponent<Renderer>().material;
        mat.SetVector("_Color", faded);

        if (requiredNodes.Length <= 0)
            return;

        for (int i = 0; i < requiredNodes.Length; i++)
        {
            TechNode requiredNode = requiredNodes[i];
            GameObject lineObj = new GameObject("lineObj");
            lineObj.transform.SetParent(requiredNode.transform);
            lineObj.transform.localEulerAngles = new Vector3(90, 0, 0);
            lineObj.layer = LayerMask.NameToLayer("TechTree");
            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
            if(lineRenderer != null)
            {
                lineRenderer.material = lineMat;
                lineRenderer.alignment = LineAlignment.TransformZ;
                lineRenderer.sortingOrder = -1;
                lineRenderer.positionCount = 2;
                lineRenderer.startWidth = lineWidth;
                lineRenderer.endWidth = lineWidth;

                lineRenderer.SetPosition(0, new Vector3(requiredNode.transform.position.x + (transform.localScale.x / 2), 
                    requiredNode.transform.position.y + 2.5f, requiredNode.transform.position.z));

                lineRenderer.SetPosition(1, new Vector3(transform.position.x - (transform.localScale.x / 2), 
                    transform.position.y + 2.5f, transform.position.z));

                lineRenderers.Add(lineRenderer);
            }
        }
    }

    public void Unlock()
    {
        if (tree == null)
        {
            Debug.Log("Tech tree not found, can't upgrade without a tech tree gameobject with respective tag and component");
            return;
        }

        if (unlocked)
        {
            AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.nodeSelectorEvent, null, ("NodeState", "AlreadyUnlocked"));
            
            return;
        }

        bool allRequiredNodesUnlocked = true;

        foreach (TechNode requiredNode in requiredNodes)
        {
            if (!requiredNode.unlocked)
            {
                allRequiredNodesUnlocked = false;
                Debug.Log("Can't unlock as the required nodes are not unlocked yet");
                AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.nodeSelectorEvent, null, ("NodeState", "CantUnlock"));
                break;
            }
        }

        if (allRequiredNodesUnlocked)
        {
            if (tree.sciencePoints >= pointsRequired)
            {
                unlocked = true;
                mat.SetVector("_Color", Lit * 8);
                tree.sciencePoints -= pointsRequired;
                AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.nodeSelectorEvent, null, ("NodeState", "Unlocked"));
            }
            else
            {
                Debug.Log("Not enough points");
                AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.nodeSelectorEvent, null, ("NodeState", "CantUnlock"));
            }
        }
    }
}
