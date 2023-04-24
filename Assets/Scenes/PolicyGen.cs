using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using UnityEngine;
using CT.Lookup;
using System.Configuration;
using FMOD.Studio;
using CT;
using Unity.VisualScripting;

public class PolicyGen : MonoBehaviour
{
    private CTPolicyCard policy = new CTPolicyCard();

    private void Start()
    {
        GeneratePolicy();
    }

    private void Update()
    {
        GeneratePolicy();
    }

    private void GeneratePolicy()
    {
        policy.SetCost(GenerateCost());
        policy.SetRequirements(GenerateRequirements());
        policy.SetBuffs(GenerateBuffs());
        policy.SetDebuffs(GenerateDebuffs());
        policy.SetDegrees(GenerateDegree());
        policy.SetName(GeneratePolicyTitle());
    }

    private CTCost GenerateCost()
    {
        bool[] t = RandomBools(4, 4);

        return new CTCost(
            (UnityEngine.Random.Range(1, 21) * 250) * Convert.ToInt32(t[0]),
            (UnityEngine.Random.Range(1, 21) * 250) * Convert.ToInt32(t[1]),
            (UnityEngine.Random.Range(1, 21) * 250) * Convert.ToInt32(t[2]),
            (UnityEngine.Random.Range(1, 21) * 250) * Convert.ToInt32(t[3]));
    }

    private SetFactionDistribution GenerateRequirements()
    {
        bool[] t = RandomBools(4, 4);

        Vector4 spread = GeneratePopulationSpread();

        spread *= (UnityEngine.Random.Range(5, 11) * 0.1f);

        return new SetFactionDistribution(
            spread.x * Convert.ToInt32(t[0]), // workers
            spread.y * Convert.ToInt32(t[1]), // scientists
            spread.z * Convert.ToInt32(t[2]), // farmers
            spread.w * Convert.ToInt32(t[3]));  // planners
    }

    private Dictionary<BuffsNerfsType, bool> GenerateBuffs()
    {
        Dictionary<BuffsNerfsType, bool> buff_dict = new Dictionary<BuffsNerfsType, bool>();

        bool[] buffs = new bool[10];
        buffs = RandomBools(10, 2);

        for (int i = 0; i < buffs.Length; i++)
        {
            BuffsNerfsType[] a = (BuffsNerfsType[])Enum.GetValues(typeof(BuffsNerfsType));
            if (buffs[i] == true)
            {
                if (!buff_dict.ContainsKey(a[i]))
                    buff_dict.Add(a[i], buffs[i]);
            }
        }

        return buff_dict;

    }
    private Dictionary<BuffsNerfsType, bool> GenerateDebuffs()
    {
        Dictionary<BuffsNerfsType, bool> debuff_dict = new Dictionary<BuffsNerfsType, bool>();

        bool[] nerfs = new bool[10];
        nerfs = RandomBools(10, 2);

        for (int i = 0; i < nerfs.Length; i++)
        {
            BuffsNerfsType[] a = (BuffsNerfsType[])Enum.GetValues(typeof(BuffsNerfsType));
            if (nerfs[i] == true)
            {
                if (!debuff_dict.ContainsKey(a[i]))
                    debuff_dict.Add(a[i], nerfs[i]);
            }
        }

        return debuff_dict;
    }

    private Dictionary<BuffsNerfsType, float> GenerateDegree()
    {
        Dictionary<BuffsNerfsType, float> degree = new Dictionary<BuffsNerfsType, float>();

        foreach (KeyValuePair<BuffsNerfsType, bool> kvp in policy.buffs)
        {
            if (!degree.ContainsKey(kvp.Key))
                degree.Add(kvp.Key, UnityEngine.Random.Range(DataSheet.policy_card_min_scale, DataSheet.policy_card_max_scale));
        }

        foreach (KeyValuePair<BuffsNerfsType, bool> kvp in policy.debuffs)
        {
            if (!degree.ContainsKey(kvp.Key))
                degree.Add(kvp.Key, UnityEngine.Random.Range(DataSheet.policy_card_min_scale, DataSheet.policy_card_max_scale) * -1);
        }

        return degree;
    }


    private string GeneratePolicyTitle()
    {
        string s = "";

        if (policy.cost.population > 0)
        {
            s = $"{s}Sacrifice!";
        }

        // Buff Mid
        foreach (KeyValuePair<BuffsNerfsType, bool> kvp in policy.buffs)
        {
            if (kvp.Value == true)
            {
                // Buff Prefix
                float degree = policy.buff_nerf_scale[kvp.Key] / DataSheet.policy_card_max_scale;
                degree = Mathf.Abs(degree);

                int index = -1;

                if (degree < 0.25f)
                    index = 0;
                else if (degree < 0.5f)
                    index = 1;
                else if (degree < 0.75f)
                    index = 2;
                else if (degree <= 1.0f)
                    index = 3;

                s = $"{s} {DataSheet.policy_degree_prefixes[index]}";

                // Buff Mid
                s = $"{s} {DataSheet.policy_type[kvp.Key]}";
            }
        }

        // Buff suffix
        s = $"{s} {DataSheet.policy_buff_suffixes[UnityEngine.Random.Range(0, DataSheet.policy_buff_suffixes.Length)]}";

        // String start new line
        s = $"{s}\n";

        // Nerfs section of the string
        foreach (KeyValuePair<BuffsNerfsType, bool> kvp in policy.debuffs)
        {
            if (kvp.Value == true)
            {
                // Nerf Prefix
                s = $"{s} {DataSheet.policy_degree_prefixes[1]}";

                // Nerf Mid
                s = $"{s} {DataSheet.policy_type[kvp.Key]}";
            }
        }

        // Nerf suffix
        s = $"{s} {DataSheet.policy_nerf_suffixes[UnityEngine.Random.Range(0, DataSheet.policy_nerf_suffixes.Length)]}";

        Debug.Log(s);

        return s;
    }

    private bool[] RandomBools(int _options, int _n)
    {
        bool[] bools = new bool[_options];

        for (int i = 0; i < _options; i++)
        {
            bools[i] = false;
        }

        int total = UnityEngine.Random.Range(1, _n + 1);

        int index = 0;
        while (index < total)
        {
            int r = UnityEngine.Random.Range(0, _options);

            if (bools[r] == false)
            {
                bools[r] = true;
                index++;
            }
        }

        return bools;
    }

    private Vector4 GeneratePopulationSpread()
    {
        System.Random rand = new System.Random();

        float[] floats = { 0, 0, 0, 0 };

        floats[0] = (float)rand.NextDouble();
        floats[1] = (float)rand.NextDouble();
        floats[2] = (float)rand.NextDouble();
        floats[3] = (float)rand.NextDouble();

        float sum = floats[0] + floats[1] + floats[2] + floats[3];

        floats[0] = Mathf.Round(floats[0] / sum * 10f) / 10f;
        floats[1] = Mathf.Round(floats[1] / sum * 10f) / 10f;
        floats[2] = Mathf.Round(floats[2] / sum * 10f) / 10f;
        floats[3] = Mathf.Round(floats[3] / sum * 10f) / 10f;

        float adjusted_sum = floats[0] + floats[1] + floats[2] + floats[3];

        float adjustment = (adjusted_sum > 1.0f) ? -0.1f : 0.1f;

        if (adjusted_sum != 1.0f)
        {
            // If sum is less than 1.0f
            if (adjusted_sum < 1.0f)
            {
                float lowest = floats.Min();
                for (int i = 0; i < floats.Length; i++)
                {
                    if (floats[i] == lowest)
                    {
                        floats[i] += 0.1f;
                        break;
                    }
                }
            }
            else // If sum is greater than 1.0f
            {
                float highest = floats.Max();
                for (int i = 0; i < floats.Length; i++)
                {
                    if (floats[i] == highest)
                    {
                        floats[i] -= 0.1f;
                        break;
                    }
                }
            }

            adjusted_sum = floats[0] + floats[1] + floats[2] + floats[3];

        }
        return new Vector4(floats[0], floats[1], floats[2], floats[3]);
    }
}

public class CTPolicyCard
{
    public string name = "";
    public CTCost cost = new CTCost();
    public SetFactionDistribution fdist = new SetFactionDistribution();
    public Dictionary<BuffsNerfsType, float> buff_nerf_scale = new Dictionary<BuffsNerfsType, float>();
    public Dictionary<BuffsNerfsType, bool> buffs = new Dictionary<BuffsNerfsType, bool>();
    public Dictionary<BuffsNerfsType, bool> debuffs = new Dictionary<BuffsNerfsType, bool>();

    public void SetName(string _name)
    {
        name = _name;
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
