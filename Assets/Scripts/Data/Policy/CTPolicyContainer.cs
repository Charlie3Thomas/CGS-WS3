using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTPolicyContainer : MonoBehaviour
{
    [SerializeField] private CTPolicyCard current_policy;
    [SerializeField] public List<CTPolicyCard> policies;

    public void SetPolicyForCurrentTurn(uint _turn)
    {
        current_policy = policies[(int)_turn];
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
