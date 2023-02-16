using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTree : MonoBehaviour
{
    public List<TechNode> nodes;
    private List<BuffsNerfs> buffs = new List<BuffsNerfs>();
    public List<BuffsNerfs> uniqueBuffs = new List<BuffsNerfs>();
    [SerializeReference]
    public Resource sciencePoints = new Resource { allocType = AllocType.SCIENCE };
    [SerializeReference]
    public Resource money = new Resource { allocType = AllocType.MONEY };

    public void UpdateBuffs(List<BuffsNerfs> newBuffs)
    {
        foreach (var newBuff in newBuffs)
        {
            buffs.Add(newBuff);

            bool newBuffType = true;
            foreach (BuffsNerfs b in uniqueBuffs)
            {
                if (b.type == newBuff.type)
                {
                    b.amount += newBuff.amount;
                    newBuffType = false;
                    break;
                }
            }

            if (newBuffType)
            {
                uniqueBuffs.Add(new BuffsNerfs(newBuff.type, newBuff.amount));
            }
        }
    }
}
