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
    public Material mat;
    private float lineWidth = 5f;
    public AllocType effectType;
    public float effect;

    private void Start()
    {
        tree = GameObject.FindGameObjectWithTag("TechTree").GetComponent<TechTree>();
        lineRenderers = new List<LineRenderer>();

        if (requiredNodes.Length <= 0)
            return;

        for (int i = 0; i < requiredNodes.Length; i++)
        {
            TechNode requiredNode = requiredNodes[i];
            GameObject lineObj = new GameObject("lineObj");
            lineObj.transform.SetParent(requiredNode.transform);
            lineObj.layer = LayerMask.NameToLayer("TechTree");
            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
            if(lineRenderer != null)
            {
                lineRenderer.material = mat;
                lineRenderer.positionCount = 2;
                lineRenderer.startWidth = lineWidth;
                lineRenderer.endWidth = lineWidth;
                lineRenderer.SetPosition(0, requiredNode.transform.position);
                lineRenderer.SetPosition(1, transform.position);
                lineRenderers.Add(lineRenderer);
            }
        }
    }

    public void Unlock()
    {
        if (tree == null)
        {
            Debug.Log("Parent does not contain the tech tree script, can not unlock without it");
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
