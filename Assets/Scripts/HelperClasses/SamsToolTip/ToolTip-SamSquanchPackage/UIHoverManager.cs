using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHoverManager : MonoBehaviour
{
    public static UIHoverManager instance => m_instance;
    private static UIHoverManager m_instance;
    //This script utilises actions and pointer call backs to implement hover functionality, utilised by the tooltip system

    public TextMeshProUGUI tipText;
    public RectTransform tipWindow;

    public static Action<string, Vector2> OnMouseHover; //Action for mouse hover, will provide the message (string) and position to display (Vector2)
    public static Action OnLoseFocus; //Action for when to not display tooltip 


    //Subscribe to events
    protected void OnEnable()
    {
        
        OnMouseHover += ShowTip;
        OnLoseFocus += HideTip;
    }

    //Un-subscribe to events (MUST BE DONE TO AVOID NULL REFERENCES OR DATA LEAKS)
    protected void OnDisable()
    {
        //Un-subscribe to mouseover and lose focus events
        OnMouseHover -= ShowTip;
        OnLoseFocus -= HideTip;
    }
    // Start is called before the first frame update
    protected void Start()
    {
        m_instance = this;
        //Default tip state to no tip showing
        HideTip();
    }

    public void ShowTip(string tip, Vector2 mousePos)
    {
        tipText.text = tip;

        //Ternary operator to insure the pixel size is not above 200
        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200 : tipText.preferredWidth, tipText.preferredHeight);

        tipWindow.gameObject.SetActive(true);
        tipWindow.transform.position = new Vector2(mousePos.x + tipWindow.sizeDelta.x * 2, mousePos.y);
    }

    protected void HideTip()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);
    }
    
}
