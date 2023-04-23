using CT;
using CT.Data;
using CT.Lookup;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

//Nodes 23, 24, 46, 47, 48, 62 are unique and need their own implementations
public class TechNode : MonoBehaviour
{
    private TechTree tree;
    public CTTechnologies tech;
    [SerializeReference]
    public Resource requiredMoney = new Resource { allocType = AllocType.MONEY };
    [SerializeReference]
    public Resource requiredScience = new Resource { allocType = AllocType.SCIENCE };
    public bool unlocked;
    public TechNode[] requiredNodes;
    private List<LineRenderer> lineRenderers;
    public Material lineMat;
    private float lineWidth = 1.5f;
    public List<BuffsNerfs> buffs = new List<BuffsNerfs>();

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
                Debug.Log("Can't unlock as the required nodes are not unlocked yet");
                AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.nodeSelectorEvent, null, ("NodeState", "CantUnlock"));
                break;
            }
        }

        if (allRequiredNodesUnlocked)
        {
            if (GameManager._INSTANCE.PurchaseTechnology(tech))
            {
                unlocked = true;
                //mat.SetVector("_Color", Lit * 8);
                //tree.UpdateBuffs(buffs);
                SpecialCase();
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
                    //float temp_money = tree.money.amount;
                    //float temp_science = tree.sciencePoints.amount;
                    //tree.money.amount = temp_science / 5;
                    //tree.sciencePoints.amount = temp_money;
                }
                break;
            case CTTechnologies.Marketplace2:
                {
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
                // NUKE GAMEMANAGER.INSTANCE.AWARENESS_CHANGES
                break;
        }
    }

    public void UpdateTechNodes()
    {
        unlocked = false;
        List<CTTechnologies> active_techs = GameManager._INSTANCE.GetUnlockedTechnologiesInTurn();

        //Debug.Log($"TechNode:UpdateTechNodes:active_techs = {active_techs.Count}");

        foreach (CTTechnologies t in active_techs)
        {
            if (t == this.tech)
            {
                this.unlocked = true;
                tree.LookupBuffs(t);
                break;
            }
        }

        this.mat.SetVector("_Color", unlocked ? Lit * 8 : faded);
    }
}
