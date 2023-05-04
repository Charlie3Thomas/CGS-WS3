using CT;
using CT.Data;
using CT.Enumerations;
using CT.Lookup;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

//Nodes 23, 24, 46, 47, 48, 62 are unique and need their own implementations
public class TechNode : MonoBehaviour
{
    private TechTree tree;
    public CTTechnologies tech;
    public bool unlocked;
    public TechNode[] requiredNodes;
    private List<LineRenderer> lineRenderers;
    public Material lineMat;
    private float lineWidth = 1.5f;

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

        //Debug.Log(tech.ToString());

        if (unlocked)
        {
            UIHoverManager.instance.ShowTip("Node Unlocked!", Input.mousePosition);
            AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.nodeSelectorEvent, null, ("NodeState", "AlreadyUnlocked"));

            UIHoverManager.instance.ManuallyHideToolTip();
            return;
        }

        bool allRequiredNodesUnlocked = true;

        foreach (TechNode requiredNode in requiredNodes)
        {
            if (!requiredNode.unlocked)
            {
                allRequiredNodesUnlocked = false;
                Debug.Log($"Can't unlock {this.tech} as the required nodes are not unlocked yet {requiredNode.tech}");
                Debug.Log("Can't unlock as the required nodes are not unlocked yet");
                UIHoverManager.instance.ShowTip("Can't unlock as the required nodes are not unlocked yet!", Input.mousePosition);
                AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.nodeSelectorEvent, null, ("NodeState", "CantUnlock"));
                UIHoverManager.instance.ManuallyHideToolTip();
                break;
            }
        }

        if (allRequiredNodesUnlocked)
        {
            if (GameManager._INSTANCE.PurchaseTechnology(tech, GameManager._INSTANCE.turn_data.turn))
            {
                unlocked = true;
                //mat.SetVector("_Color", Lit * 8);
                //tree.UpdateBuffs(buffs);
                SpecialCase();
                UIHoverManager.instance.ShowTip("Node Unlocked!", Input.mousePosition);
                AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.nodeSelectorEvent, null, ("NodeState", "Unlocked"));
                UpdateTechNodes();
                UIHoverManager.instance.ManuallyHideToolTip();
            }
            else
            {
                Debug.Log("Not enough points");
                UIHoverManager.instance.ShowTip("Can't unlock, not enough points!", Input.mousePosition);
                AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.nodeSelectorEvent, null, ("NodeState", "CantUnlock"));
                UIHoverManager.instance.ManuallyHideToolTip();
            }
        }
    }

    void SpecialCase()
    {
        switch(tech)
        {
            case CTTechnologies.RiskAssessment:
                {
                    DisasterManager.instance.showMagnitude = true;
                    DisasterManager.instance.WriteDisastersInJournal();
                }
                break;
            case CTTechnologies.PopulationAssessment:
                {
                    DisasterManager.instance.showDeathToll = true;
                    DisasterManager.instance.WriteDisastersInJournal();
                }
                break;
            case CTTechnologies.Marketplace1:
                {
                    // Swap money and science
                    // 
                    GameManager._INSTANCE.turn_data.SwapResources(CTResources.Money, 5.0f, CTResources.Science, 1.0f);
                    //float temp_money = tree.money.amount;
                    //float temp_science = tree.sciencePoints.amount;
                    //tree.money.amount = temp_science / 5;
                    //tree.sciencePoints.amount = temp_money;
                }
                break;
            case CTTechnologies.Marketplace2:
                {
                    GameManager._INSTANCE.turn_data.SwapResources(CTResources.Money, 1.0f, CTResources.Science, 2.0f);
                    //float temp_money = tree.money.amount;
                    //float temp_science = tree.sciencePoints.amount;
                    //tree.money.amount = temp_science * 2;
                    //tree.sciencePoints.amount = temp_money;
                }
                break;
            case CTTechnologies.SafetyRating:
                {
                    DisasterManager.instance.showSafety = true;
                }
                break;
            case CTTechnologies.MemoryFlash:
                // Reset awareness
                GameManager._INSTANCE.ResetAwareness();
                break;
        }
    }

    public void UpdateTechNodes()
    {
        this.unlocked = false;
        List<CTTechnologies> techs = GameManager._INSTANCE.GetUnlockedTechnologiesInTurn();

        //Debug.Log($"TechNode:UpdateTechNodes:active_techs = {active_techs.Count}");

        foreach (CTTechnologies t in techs)
        {
            if (t == this.tech)
            {
                this.unlocked = true;
                tree.LookupBuffs(t);
                break;
            }
        }

        this.mat.SetVector("_Color", this.unlocked ? Lit * 8 : faded);
    }

    public List<TechNode> GetRequiredTechs()
    {
        List<TechNode> ret = new List<TechNode>();

        for(int i = 0; i < requiredNodes.Length; i++)
        {
            ret.Add(requiredNodes[i]);
        }

        return ret;
    }
}
