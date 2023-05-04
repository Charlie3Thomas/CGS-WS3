using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CT.Lookup;
using System.Runtime.InteropServices;

public static class PolicyGen
{
    private static uint seed = 0;
    private static double scramble = 0;
    private static string id = "";

    public static void GeneratePolicy(CTPolicyCard _pc, string _name, int _i)
    {
        float x = _i;
        x *= (Mathf.PI * 7000);
        _pc.ID = Guid.NewGuid().ToString();
        seed = (uint)CTSeed.RandFromSeed((uint)_i * (uint)x, _name).Next();
        scramble = CTSeed.RandFromSeed(1, _name).NextDouble();
        id = _name;

        _pc.SetCost(GenerateCost());
        _pc.SetRequirements(GenerateRequirements());
        _pc.SetBuffs(GenerateBuffs());
        _pc.SetDebuffs(GenerateDebuffs());
        _pc.SetDegrees(GenerateDegree(_pc));
        _pc.SetText(GeneratePolicyTitle(_pc));
    }

    private static CTCost GenerateCost()
    {
        bool[] t = RandomBools(4, 4);

        //return new CTCost(
        //    (UnityEngine.Random.Range(1, 21) * 250) * Convert.ToInt32(t[0]),
        //    (UnityEngine.Random.Range(1, 21) * 250) * Convert.ToInt32(t[1]),
        //    (UnityEngine.Random.Range(1, 21) * 250) * Convert.ToInt32(t[2]),
        //    (UnityEngine.Random.Range(1, 21) * 250) * Convert.ToInt32(t[3]));

        return new CTCost(
            (CTSeed.RandFromSeed(seed + 0, $"{id}{"PGGC0"}").Next(1, 21) * 250) * Convert.ToInt32(t[0]),
            (CTSeed.RandFromSeed(seed + 1, $"{id}{"PGGC1"}").Next(1, 21) * 250) * Convert.ToInt32(t[1]),
            (CTSeed.RandFromSeed(seed + 2, $"{id}{"PGGC2"}").Next(1, 21) * 250) * Convert.ToInt32(t[2]),
            (CTSeed.RandFromSeed(seed + 3, $"{id}{"PGGC3"}").Next(1, 21) * 250) * Convert.ToInt32(t[3]));
    }

    private static SetFactionDistribution GenerateRequirements()
    {
        bool[] t = RandomBools(4, 4);

        Vector4 spread = GeneratePopulationSpread();

        //spread *= (UnityEngine.Random.Range(5, 11) * 0.1f);

        spread *= (CTSeed.RandFromSeed(seed, id).Next(5, 11) * 0.1f);

        return new SetFactionDistribution(
            spread.x * Convert.ToInt32(t[0]), // workers
            spread.y * Convert.ToInt32(t[1]), // scientists
            spread.z * Convert.ToInt32(t[2]), // farmers
            spread.w * Convert.ToInt32(t[3]));  // planners
    }

    private static Dictionary<BuffsNerfsType, bool> GenerateBuffs()
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
                {
                    buff_dict.Add(a[i], buffs[i]);
                    //Debug.Log(a[i]);
                }
            }
        }

        return buff_dict;

    }
    private static Dictionary<BuffsNerfsType, bool> GenerateDebuffs()
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
                {
                    debuff_dict.Add(a[i], nerfs[i]);
                    //Debug.Log(a[i]);
                }
            }
        }

        return debuff_dict;
    }

    private static Dictionary<BuffsNerfsType, float> GenerateDegree(CTPolicyCard _p)
    {
        Dictionary<BuffsNerfsType, float> degree = new Dictionary<BuffsNerfsType, float>();

        uint x = 0;
        foreach (KeyValuePair<BuffsNerfsType, bool> kvp in _p.buffs)
        {
            //if (!degree.ContainsKey(kvp.Key))
            //    degree.Add(kvp.Key, UnityEngine.Random.Range(DataSheet.policy_card_min_scale, DataSheet.policy_card_max_scale));
            if (!degree.ContainsKey(kvp.Key))
                degree.Add(kvp.Key, (float)CTSeed.RandFromSeed(seed + x, $"{id}{"PGGD0"}").NextDouble() * (DataSheet.POLICY_CARD_MAX_SCALE - DataSheet.POLICY_CARD_MIN_SCALE));
            x++;
        }

        uint y = 0;
        foreach (KeyValuePair<BuffsNerfsType, bool> kvp in _p.debuffs)
        {
            //if (!degree.ContainsKey(kvp.Key))
            //    degree.Add(kvp.Key, UnityEngine.Random.Range(DataSheet.policy_card_min_scale, DataSheet.policy_card_max_scale) * -1);
            if (!degree.ContainsKey(kvp.Key))
                degree.Add(kvp.Key, (float)CTSeed.RandFromSeed(seed + y, $"{id}{"PGGD1"}").NextDouble() * (DataSheet.POLICY_CARD_MAX_SCALE - DataSheet.POLICY_CARD_MIN_SCALE) * -1);
            y++;
        }

        return degree;
    }


    private static string GeneratePolicyTitle(CTPolicyCard _p)
    {
        string s = "";

        if (_p.cost.population > 0)
        {
            s = $"{s}Sacrifice!\n";
        }

        s = $"{s}Buffs:\n";

        // Buff Mid
        uint x = 0;
        foreach (KeyValuePair<BuffsNerfsType, bool> kvp in _p.buffs)
        {
            if (kvp.Value == true)
            {
                // Buff Prefix
                float degree = _p.buff_nerf_scale[kvp.Key] / DataSheet.POLICY_CARD_MAX_SCALE;
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

                s = $"{s}    {DataSheet.policy_degree_prefixes[index]}";

                // Buff Mid
                s = $"{s} {DataSheet.policy_type[kvp.Key]}";

                // Buff suffix
                //s = $"{s} {DataSheet.policy_buff_suffixes[UnityEngine.Random.Range(0, DataSheet.policy_buff_suffixes.Length)]}\n";
                s = $"{s} {DataSheet.policy_buff_suffixes[CTSeed.RandFromSeed(seed + x, $"{id}{"PGGPT0"}").Next(DataSheet.policy_buff_suffixes.Length)]}\n";
            }
            x++;
        }



        // String start new line
        s = $"{s}\n";

        s = $"{s}Nerfs:\n";

        // Nerfs section of the string
        uint y = 0;
        foreach (KeyValuePair<BuffsNerfsType, bool> kvp in _p.debuffs)
        {
            if (kvp.Value == true)
            {
                // Nerf Prefix
                s = $"{s}    {DataSheet.policy_degree_prefixes[1]}";

                // Nerf Mid
                s = $"{s} {DataSheet.policy_type[kvp.Key]}";

                // Nerf suffix
                s = $"{s} {DataSheet.policy_nerf_suffixes[CTSeed.RandFromSeed(seed + y, $"{id}{"PGGPT1"}").Next(0, DataSheet.policy_nerf_suffixes.Length)]}\n";
            }
        }

        s = $"{s}\nCosts:\n";

        //Debug.Log(s);

        return s;
    }

    private static bool[] RandomBools(int _options, int _n)
    {
        bool[] bools = new bool[_options];

        for (int i = 0; i < _options; i++)
        {
            bools[i] = false;
        }

        //int total = UnityEngine.Random.Range(1, _n + 1);
        int total = CTSeed.RandFromSeed(seed + (uint)_n + (uint)scramble + (uint)_options, $"{id}{"PGRB0"}{scramble}").Next(1, _n + 1);

        int index = 0;
        uint loops = 0;
        while (index < total)
        {
            int r = CTSeed.RandFromSeed(seed + loops + (uint)scramble, $"{id}{"PGRB1"}{scramble}").Next(0, _options);

            if (bools[r] == false)
            {
                bools[r] = true;
                index++;
            }

            loops++;
        }

        scramble++;
        scramble *= Mathf.PI / 2;

        return bools;
    }

    private static Vector4 GeneratePopulationSpread()
    {
        //System.Random rand = new System.Random();

        float[] floats = { 0, 0, 0, 0 };

        floats[0] = (float)CTSeed.RandFromSeed(seed + 0, $"{id}{"PGGPS0"}").NextDouble();
        floats[1] = (float)CTSeed.RandFromSeed(seed + 1, $"{id}{"PGGPS1"}").NextDouble();
        floats[2] = (float)CTSeed.RandFromSeed(seed + 2, $"{id}{"PGGPS2"}").NextDouble();
        floats[3] = (float)CTSeed.RandFromSeed(seed + 3, $"{id}{"PGGPS3"}").NextDouble();

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