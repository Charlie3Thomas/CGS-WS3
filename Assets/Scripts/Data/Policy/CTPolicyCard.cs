using CT.Lookup;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CTPolicyCard
{

    public CTPolicyCard() { }

    public CTPolicyCard(CTPolicyCard _pc) 
    { 
        this.ID = _pc.ID;
        this.info_text = _pc.info_text;
        this.cost = _pc.cost;
        this.fdist = _pc.fdist;
        this.buff_nerf_scale = _pc.buff_nerf_scale;
        this.buffs = _pc.buffs;
        this.debuffs = _pc.debuffs;
    }


    public string ID;
    public string info_text;
    public CTCost cost = new CTCost();
    public SetFactionDistribution fdist = new SetFactionDistribution();
    public Dictionary<BuffsNerfsType, float> buff_nerf_scale = new Dictionary<BuffsNerfsType, float>();
    public Dictionary<BuffsNerfsType, bool> buffs = new Dictionary<BuffsNerfsType, bool>();
    public Dictionary<BuffsNerfsType, bool> debuffs = new Dictionary<BuffsNerfsType, bool>();

    public void CopyPolicyCard(CTPolicyCard _pc)
    {
        _pc.ID = this.ID;
        _pc.info_text = this.info_text;
        _pc.cost = this.cost;
        _pc.fdist = this.fdist;
        _pc.buff_nerf_scale = this.buff_nerf_scale;
        _pc.buffs = this.buffs;
        _pc.debuffs = this.debuffs;
    }

    public void SetID(string _ID)
    {
        this.ID = _ID;
    }

    public void SetText(string _name)
    {
        info_text = _name;
    }

    public void SetCost(CTCost _cost)
    {
        cost = _cost;
    }

    public void SetRequirements(SetFactionDistribution _fdist)
    {
        fdist = _fdist;
    }

    public void SetDegrees(Dictionary<BuffsNerfsType, float> _degree)
    {
        buff_nerf_scale = _degree;
    }

    public void SetBuffs(Dictionary<BuffsNerfsType, bool> _buffs)
    {
        buffs = _buffs;
    }

    public void SetDebuffs(Dictionary<BuffsNerfsType, bool> _debuffs)
    {
        debuffs = _debuffs;
    }
}