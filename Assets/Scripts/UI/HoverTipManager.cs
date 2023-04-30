using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HoverTipManager : MonoBehaviour
{
    public TextMeshProUGUI tipText;
    public RectTransform tipWindow;

    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFOcus;

    private void Start()
    {
        HideTip();

    }

    private void ShowTip(string tip, Vector2 mousePos)
    {
        tipText.text = tip;
        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200 : tipText.preferredWidth, tipText.preferredHeight);

        tipWindow.gameObject.SetActive(true);
        tipWindow.transform.position = new Vector2(mousePos.x + tipWindow.sizeDelta.x * 2,mousePos.y);

    }

    private void HideTip()
    {

        tipText.text = "TOOL_TIP_MISSING";
        tipWindow.gameObject.SetActive(false);

    }
    //Subscription Functions

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFOcus += HideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFOcus -= HideTip;
    }


}
