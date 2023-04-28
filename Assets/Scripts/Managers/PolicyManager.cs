using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;
using CT.Lookup;
using CT;
using CT.Data.Changes;
using UnityEditor;
using UnityEngine.ProBuilder.MeshOperations;

public class PolicyManager : MonoBehaviour
{
    public static PolicyManager instance;

    private static readonly int policies_per_turn = 7; // Number of policies shown per turn
    private CTPolicyContainer[] policy_containers; // Container objects for policies
    public CTPolicyCard[] policies_at_current_turn; // Policies show in current turn
    public CTPolicyCard first_out_policy; // Policy replaced when setting a new policy when policies = 3
    private int first_out_index = 0; // Index for first out policy

    public CTPolicyCard[] current_policies; // Purchased policies

    private Vector2 scroll;

    #region UnityMethods
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SubscribeInputs();
    }

    private void OnDisable()
    {
        UnsubscribeInputs();
    }

    private void OnDestroy()
    {
        UnsubscribeInputs();
    }

    private void Update()
    {
        SelectCurrentPolicyWithScroll();
    }

    private void Start()
    {
        Initialise();
    }
    #endregion


    #region Methods
    /// <summary>
    /// Generates seven policy cards
    /// </summary>
    public void LoadPoliciesForTurn()
    {
        policies_at_current_turn = new CTPolicyCard[policies_per_turn];

        // Loop through all policy containers
        for (int i = 0; i < policy_containers.Length; i++)
        {
            policy_containers[i].SetPolicyForTurn();
            policies_at_current_turn[i] = policy_containers[i].GetCurrentPolicy();
        }

        for (int i = 0; i < policies_at_current_turn.Length; i++)
        {
            UpdatePolicyCardText(i, policies_at_current_turn[i]);
        }
    }

    public void SelectPolicy(string _ID)
    {
        // For each selectable policy
        for (int policy = 0; policy < policies_at_current_turn.Length; policy++)
        {
            // If the selected policy ID matches a selectable policy ID in the current turn
            if (policies_at_current_turn[policy].ID == _ID)
            {
                // If selected ID is already owned
                if (AlreadyHavePolicy(_ID))
                    return;

                // If there is a free slot in current policies
                int free_slot = FindFreeSlot();
                if (free_slot != -1)
                {
                    current_policies[free_slot] = policies_at_current_turn[policy];
                    TrackApplyPolicy(current_policies[free_slot]);
                    return;
                }
                // If there is no free slot in current policies
                else
                {
                    int replace_slot = HandleNoFreeSlotCase();
                    current_policies[replace_slot] = policies_at_current_turn[policy];
                    TrackApplyPolicy(current_policies[replace_slot]);
                    return;
                }
            }
        }
    }

    public void UpdatePolicyCardText(int _index, CTPolicyCard _pc)
    {
        ComputerController.Instance.pCardTexts[_index].text = _pc.info_text;

        ComputerController.Instance.pCardTexts[_index].text =
                $"{ComputerController.Instance.pCardTexts[_index].text}" +
                $"    {_pc.cost.GetString()}";
    }

    #endregion


    #region Utility
    private void Initialise()
    {
        current_policies = new CTPolicyCard[3];
        for (int i = 0; i < current_policies.Length; i++)
        {
            current_policies[i] = new CTPolicyCard();
        }

        policies_at_current_turn = new CTPolicyCard[policies_per_turn];
        for (int i = 0; i < policies_at_current_turn.Length; i++)
        {
            policies_at_current_turn[i] = new CTPolicyCard();
        }

        policy_containers = new CTPolicyContainer[policies_per_turn];
        for (int i = 0; i < policy_containers.Length; i++)
        {
            policy_containers[i] = ComputerController.Instance.policyCards[i].GetComponent<CTPolicyContainer>();
            policy_containers[i].index = i;
        }

        LoadPoliciesForTurn();
    }

    private bool AlreadyHavePolicy(string _ID)
    {
        for (int i = 0; i < current_policies.Length; i++)
        {
            if (current_policies[i].ID == _ID)
            {
                return true;
            }
        }

        return false;
    }

    private int FindFreeSlot()
    {
        for (int slot = 0; slot < current_policies.Length; slot++)
        {
            if (current_policies[slot].ID == null)
            {
                return slot;
            }
        }

        return -1;
    }

    private int HandleNoFreeSlotCase()
    {
        // Placeholder functionality
        return first_out_index;
    }

    private void ReplacePolicy(int _index)
    {
        // Replace policy in CTPolicyContainer.policies[CURRENT_TURN]
        PolicyGen.GeneratePolicy(policy_containers[_index].policies[(int)GameManager._INSTANCE.GetTurn().turn]);

        policy_containers[_index].SetPolicyForTurn();

        //// Replace policy at current turn
        //PolicyGen.GeneratePolicy(policies_at_current_turn[_index]);

        //// Replace policy in all_policy_cards
        //all_policy_cards[_index][(int)GameManager._INSTANCE.GetTurn().turn] = policies_at_current_turn[_index];

        UpdatePolicyCardText(_index, policies_at_current_turn[_index]);
    }

    private void TrackApplyPolicy(CTPolicyCard _p)
    {
        GameManager._INSTANCE.ApplyPolicy((int)GameManager._INSTANCE.GetTurn().turn, _p);
        Debug.Log("PolicyManager.TrackApplyPolicy");
    }

    private void TrackRevokePolicy(CTPolicyCard _p)
    {
        GameManager._INSTANCE.RevokePolicy((int)GameManager._INSTANCE.GetTurn().turn, _p);
    }

    #endregion


    #region Input
    private void ScrollInput(InputAction.CallbackContext context)
    {
        scroll = context.ReadValue<Vector2>();
    }

    private void SubscribeInputs()
    {
        InputManager.onScroll += ScrollInput;
    }

    private void UnsubscribeInputs()
    {
        InputManager.onScroll -= ScrollInput;
    }

    private void SelectCurrentPolicyWithScroll()
    {
        if (current_policies.Length > 0)
        {
            if (scroll.y > 0f)
            {
                first_out_index = (first_out_index + 1) % current_policies.Length;
                first_out_policy = current_policies[first_out_index];
            }
            else if (scroll.y < 0f)
            {
                first_out_index--;
                if (first_out_index < 0)
                    first_out_index = current_policies.Length - 1;
            }

            first_out_policy = current_policies[first_out_index];
        }
    }
    #endregion
}
