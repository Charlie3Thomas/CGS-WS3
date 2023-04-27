using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CT.Lookup;
using CT;

[System.Serializable]
public class CTPolicyContainer : MonoBehaviour
{
    public int index;
    [SerializeField] private CTPolicyCard current_policy;
    [SerializeField] public CTPolicyCard[] policies;

    private void Start()
    {
        current_policy = new CTPolicyCard();

        policies = new CTPolicyCard[DataSheet.turns_number];
        for (int i = 0; i < policies.Length; i++)
        {
            policies[i] = new CTPolicyCard();
            PolicyGen.GeneratePolicy(policies[i]);
        }

        SetPolicyForTurn();
    }

    public void SetPolicyForTurn()
    {
        current_policy = policies[GameManager._INSTANCE.GetTurn().turn];
    }

    public CTPolicyCard GetCurrentPolicy()
    {
        return current_policy;
    }


    public void PlayShowSound()
    {
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.policyCardShowEvent, null);
    }

    public void PlayHideSound()
    {

    }
}
