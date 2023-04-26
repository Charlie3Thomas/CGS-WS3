using CT.Lookup;
using System;
using System.Collections.Generic;
using UnityEngine;


public class CTPolicyCard : MonoBehaviour
{
    public string ID;
    public string info_text;
    public CTCost cost = new CTCost();
    public SetFactionDistribution fdist = new SetFactionDistribution();
    public Dictionary<BuffsNerfsType, float> buff_nerf_scale = new Dictionary<BuffsNerfsType, float>();
    public Dictionary<BuffsNerfsType, bool> buffs = new Dictionary<BuffsNerfsType, bool>();
    public Dictionary<BuffsNerfsType, bool> debuffs = new Dictionary<BuffsNerfsType, bool>();

    private void Awake()
    {
        PolicyGen.GeneratePolicy(this);
    }

    public void PlayShowSound()
    {
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.policyCardShowEvent, null);
    }

    public void PlayHideSound()
    {

    }

    public void SetName(string _name)
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

    public void SetBuffs(Dictionary<BuffsNerfsType, bool> _buffs)
    {
        buffs = _buffs;
    }

    public void SetDebuffs(Dictionary<BuffsNerfsType, bool> _debuffs)
    {
        debuffs = _debuffs;
    }

    public void SetDegrees(Dictionary<BuffsNerfsType, float> _degree)
    {
        buff_nerf_scale = _degree;
    }
}