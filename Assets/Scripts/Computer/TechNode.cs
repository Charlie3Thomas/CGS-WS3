using CT;
using CT.Data;
using CT.Enumerations;
using CT.Lookup;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using System;
using System.Linq;

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
            AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.nodeSelectorEvent, null, ("NodeState", "AlreadyUnlocked"));

            return;
        }

        bool allRequiredNodesUnlocked = true;

        foreach (TechNode requiredNode in requiredNodes)
        {
            if (!requiredNode.unlocked)
            {
                allRequiredNodesUnlocked = false;
                Debug.Log($"Can't unlock {this.tech} as the required nodes are not unlocked yet {requiredNode.tech}");
                AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.nodeSelectorEvent, null, ("NodeState", "CantUnlock"));
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
                SpecialCase(tech);
                // Refresh graph every time player purchases a node?
                ComputerController.Instance.RefreshGraph();
                AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.nodeSelectorEvent, null, ("NodeState", "Unlocked"));
                UpdateTechNodes();
            }
            else
            {
                Debug.Log("Not enough points");

                AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.nodeSelectorEvent, null, ("NodeState", "CantUnlock"));
            }
        }
    }

    string text = "";
    public string GetDescription()
    {
        text = "";
        text = tech.ToString();
        text = string.Concat(text.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        text = string.Concat(text.Select(x => char.IsNumber(x) ? " " + x : x.ToString())).TrimStart(' ') + "\n";
        switch(tech)
        {
            case CTTechnologies.Banking:
                text += "Nothing";
                break;
            case CTTechnologies.Laboratory:
                text += "Nothing";
                break;
            case CTTechnologies.TownPlanning:
                text += "Nothing";
                break;
            case CTTechnologies.Granary:
                text += "Nothing";
                break;
            case CTTechnologies.RiskAssessment:
                text += "Disaster List now shows the Magnitude of the Disaster";
                break;
            case CTTechnologies.PopulationAssessment:
                text += "Disaster List now shows the population lost during disaster";
                break;
            case CTTechnologies.Marketplace1:
                text += "Allows User to Swap Money to Science (5: 1)";
                break;
            case CTTechnologies.Marketplace2:
                text += "Allows User to Swap Science to Money (1: 2)";
                break;
            case CTTechnologies.SafetyRating:
                text += "The Disaster list now shows your present Safety Rating against disaster";
                break;
            case CTTechnologies.MemoryFlash:
                text += "Resets awareness";
                break;
            default:
                for (int i = 0; i < tree.GetBuffs(tech).type.Count; i++)
                {
                    text += tree.GetBuffs(tech).type[i].ToString() + " " + tree.GetBuffs(tech).amount[i].ToString() + "\n";
                }
                break;
        }
        return text;
    }

    void SpecialCase(CTTechnologies t)
    {
        switch(t)
        {
            case CTTechnologies.RiskAssessment:
                {
                    DisasterManager.instance.showMagnitude = unlocked;
                    DisasterManager.instance.WriteDisastersInJournal();
                }
                break;
            case CTTechnologies.PopulationAssessment:
                {
                    DisasterManager.instance.showDeathToll = unlocked;
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
                    DisasterManager.instance.showSafety = unlocked;
                    DisasterManager.instance.WriteDisastersInJournal();
                }
                break;
            case CTTechnologies.MemoryFlash:
                // Reset awareness
                if (unlocked)
                    GameManager._INSTANCE.ResetAwareness();
                break;
        }
    }

    public void UpdateTechNodes()
    {
        this.unlocked = false;
        SpecialCase(tech);
        List<CTTechnologies> techs = GameManager._INSTANCE.GetUnlockedTechnologiesInTurn();

        //Debug.Log($"TechNode:UpdateTechNodes:active_techs = {active_techs.Count}");

        foreach (CTTechnologies t in techs)
        {
            if (t == this.tech)
            {
                this.unlocked = true;
                tree.LookupBuffs(t);
                SpecialCase(t);
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
