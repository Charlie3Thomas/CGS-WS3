using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class PolicyManager : MonoBehaviour
{
    public static PolicyManager instance;

    private int policyCardIndex = 0;
    private int numOfPolicies = 7;
    public GameObject policyCardPrefab;
    public CTPolicyCard currentSelectedPolicy;
    public List<CTPolicyCard> policyList;
    public List<CTPolicyCard> currentPolicies;

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
        policyList = new List<CTPolicyCard>();
        currentPolicies = new List<CTPolicyCard>();

        NewPolicySet();
    }

    private void Update()
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
        
        // Generate policies and add to list
        for (int i = 0; i < numOfPolicies; i++)
        {
            policyList.Add(ComputerController.Instance.policyCards[i].GetComponent<CTPolicyCard>());
        }

        // Clear current cards from computer controller
        int index = 0;
        foreach (GameObject g in ComputerController.Instance.policyCards)
        {
            CTPolicyCard pc = g.GetComponent<CTPolicyCard>();

            pc = policyList[index];

            UpdatePolicyCardText(index, pc);

            index++;
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

        // Loop through all cards and find corresponding ID
        for (int i = 0; i < numOfPolicies; i++)
        {
            if (_ID == policyList[i].ID)
            {
                currentPolicies.Add(policyList[i]);

                PolicyGen.GeneratePolicy(policyList[i]);

                UpdatePolicyCardText(i, policyList[i]);
            }
        }

        yield return null;
    }

    private void UpdatePolicyCardText(int _index, CTPolicyCard _pc)
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
