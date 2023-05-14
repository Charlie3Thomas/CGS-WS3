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
    [HideInInspector]
    public CTPolicyContainer current_policy_container;
    public CTPolicyCard[] policies_at_current_turn; // Policies show in current turn
    public CTPolicyCard first_out_policy; // Policy replaced when setting a new policy when policies = 3
    public int first_out_index = 0; // Index for first out policy

    public CTPolicyCard[] current_policies; // Purchased policies
    public GameObject[] current_policies_go; // Game objects for purchased policies
    public GameObject policySelectScreen;
    public GameObject policyExchangeScreen;
    public TMP_Text policyTextZoomed;
    [HideInInspector]
    public CTPolicyCard aboutToBePurchasedCard;
    [HideInInspector]
    public string purchasable_id = "";
    public GameObject policyViewBackButton;
    public GameObject policySelectButtons;
    public GameObject policyExchangeButtons;

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
        //SelectCurrentPolicyWithScroll();
    }

    private void Start()
    {
        Initialise();
    }
    #endregion


    #region Methods

    public void ShowAllCurrentPoliciesAtTurn()
    {
        if(current_policies_go == null)
            return;

        // Reset all policies visuals
        for (int i = 0; i < current_policies_go.Length; i++)
        {
            current_policies_go[i].SetActive(false);
        }

        if (current_policies == null)
            return;

        // Visually show current policies by setting them active
        for (int i = 0; i < current_policies_go.Length; i++)
        {
            if (current_policies[i] != null)
            {
                if (current_policies[i].ID != null)
                {
                    string req = "Scientists: " + (current_policies[i].fdist.scientist_percentage * 100).ToString() + "%"
                        + " Workers: " + (current_policies[i].fdist.worker_percentage * 100).ToString() + "%"
                        + " Farmers: " + (current_policies[i].fdist.farmer_percentage * 100).ToString() + "%"
                        + " Planners: " + (current_policies[i].fdist.planner_percentage * 100).ToString() + "%";
                    current_policies_go[i].SetActive(true);
                    current_policies_go[i].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = 
                        current_policies[i].info_text + current_policies[i].cost.GetString() + "\n\nReq:\n" + req;
                }
            }
        }

        // Blue border on set of policies that exist in current policies
        for (int i = 0; i < policy_containers.Length; i++)
        {
            policy_containers[i].borderEffect.Stop();
            for (int j = 0; j < current_policies.Length; j++)
            {
                if (policy_containers[i].GetCurrentPolicy().ID == current_policies[j].ID)
                {
                    policy_containers[i].borderEffect.Play();
                }
            }
        }

        ShowPoliciesMetReq();
    }

    public void ShowPoliciesMetReq()
    {
        // Orange border on current policies that meet requirements
        for (int i = 0; i < current_policies.Length; i++)
        {
            current_policies_go[i].transform.GetChild(0).GetComponent<CTPolicyContainer>().borderEffect.Stop();

            if (current_policies[i].fdist.worker_percentage <= GameManager._INSTANCE.GetFactionDistribution().x
                && current_policies[i].fdist.scientist_percentage <= GameManager._INSTANCE.GetFactionDistribution().y
                && current_policies[i].fdist.farmer_percentage <= GameManager._INSTANCE.GetFactionDistribution().z
                && current_policies[i].fdist.planner_percentage <= GameManager._INSTANCE.GetFactionDistribution().w)
            {
                current_policies_go[i].transform.GetChild(0).GetComponent<CTPolicyContainer>().borderEffect.Play();

            }
        }
    }

    public void PurchasePolicy()
    {
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.policyPurchasedEvent, null); //Trigger audio event
        SelectPolicy(purchasable_id);
        ShowAllCurrentPoliciesAtTurn();
        HidePolicyPopup();
    }

    public void PolicySelect(CTPolicyCard pc)
    {
        first_out_index = 0;
        purchasable_id = pc.ID;

        for (int i = 0; i < current_policies.Length; i++)
        {
            if (current_policies[i].ID == purchasable_id)
            {
                // Play sound effect indicating that we have that purchased card
                AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.policyAlreadyPurchasedEvent, null); //Trigger audio event

                return;
            }
        }

        if (current_policies[0].ID != null && current_policies[1].ID != null && current_policies[2].ID != null)
            ShowPolicyExchangeScreen();
        else
            ShowPolicyPurchaseScreen(pc);
    }

    public void ShowPolicyViewScreen(CTPolicyCard pc)
    {
        policySelectScreen.SetActive(true);
        policyTextZoomed.text = pc.info_text + pc.cost.GetString();
        policyViewBackButton.SetActive(true);
    }

    public void ShowPolicyPurchaseScreen(CTPolicyCard pc)
    {
        policySelectScreen.SetActive(true);
        policyTextZoomed.text = pc.info_text + pc.cost.GetString();
        policySelectButtons.SetActive(true);
    }

    public void ShowPolicyExchangeScreen()
    {
        policyExchangeScreen.SetActive(true);
        policyExchangeButtons.SetActive(true);
        AudioManager.Instance.StartPolicyFireLoop();
    }

    public void HidePolicyPopup()
    {
        purchasable_id = "";
        aboutToBePurchasedCard = null;
        policyTextZoomed.text = "";
        policyViewBackButton.SetActive(false);
        policySelectButtons.SetActive(false);
        policySelectScreen.SetActive(false);
        policyExchangeScreen.SetActive(false);
        policyExchangeButtons.SetActive(false);
        AudioManager.Instance.StopPolicyFireLoop();
    }

    public void LoadPoliciesAtCurrentScope(uint _turn)
    {
        // Get all set policies at current scope
        Dictionary<SetPolicy, int> s_policies = GameManager._INSTANCE.GetAllSetPoliciesInScope();
        // Get all revoked policies at current scope
        Dictionary<RevokePolicy, int> r_policies = GameManager._INSTANCE.GetAllRevokedPoliciesInScope();
        
        // Create list of policies active at current scope
        List<CTPolicyCard> policies = new List<CTPolicyCard>();

        // Add all set policies to list
        foreach (KeyValuePair<SetPolicy, int> set in s_policies)
        {
            // Add to the list
            policies.Add(set.Key.policy);

            foreach (KeyValuePair<RevokePolicy, int> rev in r_policies)
            {
                // Remove from the list if revoked
                if (set.Key.policy.ID == rev.Key.policy.ID || 
                    set.Value > _turn) // Or the policy is outside the scope of the current turn
                    policies.Remove(set.Key.policy);
            }
        }

        if (policies.Count > 3) // This should never happen, but just in case
        {
            Debug.LogError("PolicyManager.LoadPoliciesAtCurrentScope: Has exceeded three policies at current scope!");
            return;
        }

        // Reset current_policies
        current_policies = new CTPolicyCard[3];
        for (int i = 0; i < current_policies.Length; i++)
        {
            current_policies[i] = new CTPolicyCard();
        }

        // Set current policies based on scope
        for (int i = 0; i < policies.Count; i++)
        {
            current_policies[i] = new CTPolicyCard(policies[i]);
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
            policy_containers[i].SetPolicyForTurn();
            policies_at_current_turn[i] = policy_containers[i].GetCurrentPolicy();
        }

        for (int i = 0; i < policies_at_current_turn.Length; i++)
        {
            UpdatePolicyCardText(i, policies_at_current_turn[i]);
        }

        ShowAllCurrentPoliciesAtTurn();
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
                    current_policies_go[free_slot].transform.GetChild(0).GetComponent<CTPolicyContainer>().SetPolicyContainer(current_policy_container);
                    TrackApplyPolicy(current_policies[free_slot]);
                    return;
                }
                // If there is no free slot in current policies
                else
                {
                    int replace_slot = HandleNoFreeSlotCase();
                    TrackRevokePolicy(current_policies[replace_slot]);
                    current_policies[replace_slot] = policies_at_current_turn[policy];
                    current_policies_go[replace_slot].transform.GetChild(0).GetComponent<CTPolicyContainer>().SetPolicyContainer(current_policy_container);
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
    public void Initialise()
    {
        current_policy_container = new CTPolicyContainer();
        current_policies = new CTPolicyCard[3];
        for (int i = 0; i < current_policies.Length; i++)
        {
            current_policies_go[i].SetActive(false);
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
            if (current_policies[i] == null)
                return false;

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

    private void TrackApplyPolicy(CTPolicyCard _p)
    {
        Debug.Log(_p.ID);
        GameManager._INSTANCE.ApplyPolicy((int)GameManager._INSTANCE.GetTurn().turn, _p);
        //Debug.Log("PolicyManager.TrackApplyPolicy");
    }

    private void TrackRevokePolicy(CTPolicyCard _p)
    {
        Debug.Log(_p.ID);
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

    //private void SelectCurrentPolicyWithScroll()
    //{
    //    if (current_policies.Length > 0)
    //    {
    //        if (scroll.y > 0f)
    //        {
    //            first_out_index = (first_out_index + 1) % current_policies.Length;
    //            first_out_policy = current_policies[first_out_index];
    //        }
    //        else if (scroll.y < 0f)
    //        {
    //            first_out_index--;
    //            if (first_out_index < 0)
    //                first_out_index = current_policies.Length - 1;
    //        }
    //
    //        first_out_policy = current_policies[first_out_index];
    //    }
    //}
    #endregion
}
