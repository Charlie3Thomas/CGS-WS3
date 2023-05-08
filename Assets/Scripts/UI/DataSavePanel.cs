using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataSavePanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button closeButton;

    void Start()
    {
        saveButton.onClick.AddListener(onSaveButtonClicked);
        loadButton.onClick.AddListener(onLoadButtonClicked);
        closeButton.onClick.AddListener(() => panel.SetActive(false));
    }
    
    private void onSaveButtonClicked()
    {
        DataSaveLoadManager.Instance.Save();
    }
    private void onLoadButtonClicked()
    {
        DataSaveLoadManager.Instance.Load();
    }
    public void Show()
    {
        panel.SetActive(true);
    }
}
