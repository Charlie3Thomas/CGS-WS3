using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

public class PolicyCardExchange : MonoBehaviour
{
    public enum CardState
    {
        First,
        Second,
        Third,
        Purchasable
    }

    public CardState cs;
    public VisualEffect[] othersEffects;
    public VisualEffect effect;
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
        CTPolicyCard ctp = new CTPolicyCard();

        if(cs == CardState.First)
        {
            foreach (var e in othersEffects)
            {
                e.Stop();
            }
            effect.Play();
            PolicyManager.instance.first_out_index = (int)cs;
        }

        if (cs == CardState.Purchasable)
            ctp = PolicyManager.instance.aboutToBePurchasedCard;
        else
            ctp = PolicyManager.instance.current_policies[(int)cs];

        text.text = ctp.info_text + ctp.cost.GetString();
    }

    private void OnDisable()
    {
        effect.Stop();
    }

    private void OnMouseDown()
    {
        foreach (var e in othersEffects)
        {
            e.Stop();
        }
        effect.Play();
        PolicyManager.instance.first_out_index = (int)cs;
    }
}
