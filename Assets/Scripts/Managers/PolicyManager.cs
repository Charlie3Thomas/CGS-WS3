using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;
using CT.Lookup;
using CT;

public class PolicyManager : MonoBehaviour
{
    public static PolicyManager instance;

    private int policyCardIndex = 0;
    private static readonly int numOfPolicies = 7;
    public CTPolicyCard currentSelectedPolicy;
    public List<CTPolicyCard> policyList;
    public List<CTPolicyCard> currentPolicies;

    [SerializeField] private List<CTPolicyContainer> policyContainerList;

    private CTPolicyCard[][] cards = new CTPolicyCard[numOfPolicies][];

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
        for (int i = 0; i < numOfPolicies; i++)
        {
            cards[i] = new CTPolicyCard[DataSheet.turns_number];

            for (int j = 0; j < DataSheet.turns_number; j++)
            {
                cards[i][j] = new CTPolicyCard();
                PolicyGen.GeneratePolicy(cards[i][j]);
            }
        }

        policyList = new List<CTPolicyCard>();
        currentPolicies = new List<CTPolicyCard>();

        NewPolicySet();
    }

    private void Update()
    {
        SelectCurrentPolicyWithScroll();

    }

    private void SelectCurrentPolicyWithScroll()
    {
        if (currentPolicies.Count > 0)
        {
            if (scroll.y > 0f)
            {
                policyCardIndex = (policyCardIndex + 1) % currentPolicies.Count;
                currentSelectedPolicy = currentPolicies[policyCardIndex];
            }
            else if (scroll.y < 0f)
            {
                policyCardIndex--;
                if (policyCardIndex < 0)
                    policyCardIndex = currentPolicies.Count - 1;
            }

            currentSelectedPolicy = currentPolicies[policyCardIndex];
        }
    }

    /// <summary>
    /// Generates seven policy cards
    /// </summary>
    public void NewPolicySet()
    {
        policyList.Clear();

        for (int i = 0; i < cards.Length; i++)
        {
            for (int j = 0; j < cards[i].Length; j++)
            {
                policyContainerList[i].policies.Add(cards[i][j]);
            }
        }

        for (int i = 0; i < policyContainerList.Count; i++)
        {
            policyContainerList[i].SetPolicyForCurrentTurn(GameManager._INSTANCE.GetTurn().turn);
            policyList.Add(policyContainerList[i].GetCurrentPolicy());
            UpdatePolicyCardText(i, policyContainerList[i].GetCurrentPolicy());
        }
    }

    public void ReplacePolicyCard(string _ID)
    {
        StartCoroutine(Replace(_ID));
    }

    private IEnumerator Replace(string _ID)
    {
        if (currentPolicies.Count > 2)
            currentPolicies.Remove(currentSelectedPolicy);

        // Loop through each CTPolicyContainer
        for (int i = 0; i < policyContainerList.Count; i++)
        {
            // Check if the current policy for the container matches the ID of the chosen card
            if (policyContainerList[i].GetCurrentPolicy().ID == _ID)
            {
                // If there is a match, replace the policy in the container
                currentPolicies.Add(new CTPolicyCard(policyContainerList[i].GetCurrentPolicy()));

                PolicyGen.GeneratePolicy(policyContainerList[i].policies[(int)GameManager._INSTANCE.GetTurn().turn]);

                policyContainerList[i].SetPolicyForCurrentTurn(GameManager._INSTANCE.GetTurn().turn);

                policyList[i] = policyContainerList[i].policies[(int)GameManager._INSTANCE.GetTurn().turn];

                UpdatePolicyCardText(i, policyList[i]);
            }
        }

        yield return null;
    }

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

    #endregion
}
