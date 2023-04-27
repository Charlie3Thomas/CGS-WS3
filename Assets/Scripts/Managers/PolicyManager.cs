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

    private void Start()
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

    private void Update()
    {
        SelectCurrentPolicyWithScroll();

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

    /// <summary>
    /// Generates seven policy cards
    /// </summary>
    public void LoadPoliciesForTurn()
    {
        policies_at_current_turn = new CTPolicyCard[policies_per_turn];

        // Loop through all policy containers
        for (int i = 0; i < policy_containers.Length; i++)
        {
            policies_at_current_turn[i] = policy_containers[i].GetCurrentPolicy();
        }

        for (int i = 0; i < policies_at_current_turn.Length; i++)
        {
            UpdatePolicyCardText(i, policies_at_current_turn[i]);
        }

        // Loop through all policy containers and initialise
        //for (int i = 0; i < policy_containers.Length; i++)
        //{
        //    policy_containers[i].Initialise();
        //    policy_containers[i].SetPolicyForTurn(GameManager._INSTANCE.GetTurn().turn);

        //}

        //for (int i = 0; i < policy_containers.Length; i++)
        //{
        //    policy_containers[i].InitialiseAtTurn(GameManager._INSTANCE.GetTurn().turn);
        //    policies_at_current_turn[i] = policy_containers[i].GetCurrentPolicy();
        //    UpdatePolicyCardText(i, policy_containers[i].GetCurrentPolicy());
        //}

        //for (int i = 0; i < all_policy_cards.Length; i++)
        //{
        //    for (int j = 0; j < all_policy_cards[i].Length; j++)
        //    {
        //        policy_containers[i].policies[j] = (all_policy_cards[i][j]);
        //    }
        //}

    }

    public void SelectPolicy(string _ID)
    {
        bool current_policies_has_free_slot = false;

        for (int i = 0; i < policies_at_current_turn.Length; i++)
        {
            if (policies_at_current_turn[i].ID == _ID)
            {
                if (AlreadyHavePolicy(_ID))
                    return;

                for (int j = 0; j < current_policies.Length; j++)
                {
                    if (current_policies[j].ID == null)
                    {
                        current_policies[j] = policies_at_current_turn[i];
                        TrackApplyPolicy(current_policies[j]);
                        //ReplacePolicy(i);
                        current_policies_has_free_slot = true;

                        return;
                    }

                    if (!current_policies_has_free_slot)
                    {
                        for (int x = 0; x < current_policies.Length; x++)
                        {
                            if (current_policies[x].ID == null)
                            {
                                current_policies[x] = policies_at_current_turn[i];
                                TrackApplyPolicy(current_policies[x]);

                                return;
                            }
                        }

                        current_policies[HandleNoFreeSlotCase()] = policies_at_current_turn[i];
                        TrackApplyPolicy(current_policies[HandleNoFreeSlotCase()]);
                        //ReplacePolicy(i);

                        return;
                    }
                }
            }
        }
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

    //public void ReplacePolicyCard(string _ID)
    //{
    //    StartCoroutine(Replace(_ID));
    //}

    //private IEnumerator Replace(string _ID)
    //{
    //    if (currentPolicies.Count > 2)
    //    {
    //        TrackRevokePolicy(currentSelectedPolicy);
    //        currentPolicies.Remove(currentSelectedPolicy);
    //    }

    //    // Loop through each CTPolicyContainer
    //    for (int i = 0; i < policyContainerList.Count; i++)
    //    {
    //        // Check if the current policy for the container matches the ID of the chosen card
    //        if (policyContainerList[i].GetCurrentPolicy().ID == _ID)
    //        {
    //            // If there is a match, replace the policy in the container               

    //            currentPolicies.Add(new CTPolicyCard(policyContainerList[i].GetCurrentPolicy()));

    //            PolicyGen.GeneratePolicy(policyContainerList[i].policies[(int)GameManager._INSTANCE.GetTurn().turn]);

    //            policyContainerList[i].SetPolicyForCurrentTurn(GameManager._INSTANCE.GetTurn().turn);

    //            policyList[i] = policyContainerList[i].policies[(int)GameManager._INSTANCE.GetTurn().turn];

    //            UpdatePolicyCardText(i, policyList[i]);
    //        }
    //    }

    //    yield return null;
    //}

    public void UpdatePolicyCardText(int _index, CTPolicyCard _pc)
    {
        ComputerController.Instance.pCardTexts[_index].text = _pc.info_text;

        ComputerController.Instance.pCardTexts[_index].text =
                $"{ComputerController.Instance.pCardTexts[_index].text}" +
                $"    {_pc.cost.GetString()}";
    }

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
}
