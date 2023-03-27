using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public enum ButtonType
{
    NONE,
    OK,
    PROCEED,
    CANCEL,
}
public class PopupNotificationManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Button proceedButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Animator panelAnimator;

    private Action buttonCallback;

    private static PopupNotificationManager _instance;

    public static PopupNotificationManager Instance
    {
        get
        {
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance == null || _instance != this)
            _instance = this;
    }

    void Start()
    {
        Show("MyTitle", "MyContent", ButtonType.OK);
    }

    public void Show(string title, string content, ButtonType type, Action callBack = null)
    {
        //play animation show
        panelAnimator.Play("Show");
        proceedButton.onClick.RemoveAllListeners();
        proceedButton.onClick.AddListener(onButtonClick);
        titleText.text = title;
        contentText.text = content;
        switch(type)
        {
            case ButtonType.OK:
                buttonText.text = "Ok";
                break;
            case ButtonType.PROCEED:
                buttonText.text = "Proceed";
                break;
            case ButtonType.CANCEL:
                buttonText.text = "Cancel";
                break;
            case ButtonType.NONE:
                //Autohide
                break;
        }
        if(callBack != null)
        {
            buttonCallback = callBack;
        }
    }

    private void hide()
    {
        Debug.Log("HIDE");
        panelAnimator.Play("Hide");
    }

    private void onButtonClick()
    {
        buttonCallback?.Invoke();   
        hide();
    }
}
