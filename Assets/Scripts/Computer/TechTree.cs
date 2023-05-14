using CT.Lookup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTree : MonoBehaviour
{
    public List<TechNode> nodes;
    [SerializeField] private List<BuffsNerfs> buffs = new List<BuffsNerfs>();
    //[SerializeReference]
    //public Resource sciencePoints = new Resource { allocType = AllocType.SCIENCE };
    //[SerializeReference]
    //public Resource money = new Resource { allocType = AllocType.MONEY };

    public void UpdateNodes()
    {
        foreach (TechNode node in nodes) 
        {
            node.UpdateTechNodes();
        }

        DisasterManager.instance.WriteDisastersInJournal();
    }

    public BuffsNerfs GetBuffs(CTTechnologies _t)
    {
        List<BuffsNerfsType> types = DataSheet.technology_buffs[_t].type;
        List<float> ammounts = DataSheet.technology_buffs[_t].amount;

        BuffsNerfs bn = new BuffsNerfs(types, ammounts);

        return bn;
    }

    public void LookupBuffs(CTTechnologies _t)
    {
        List<BuffsNerfsType> types = DataSheet.technology_buffs[_t].type;
        List<float> ammounts = DataSheet.technology_buffs[_t].amount;

        //Debug.Log($"types size: {types.Count}. ammounts sise {ammounts.Count}");

        BuffsNerfs bn = new BuffsNerfs(types, ammounts);

        buffs.Add(bn);
    }

    public void ClearBuffs()
    {
        buffs.Clear();
    }

    public TechNode GetNodeOfType(CTTechnologies _type)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].tech == _type)
            {
                return nodes[i];
            }
        }

        Debug.LogError($"TechTree.GetNodeOfType: Could not get node of type {_type}");
        return new TechNode();
    }

}
