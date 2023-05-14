using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowScoreMenu : MonoBehaviour
{
    public static ShowScoreMenu INSTANCE;

    [SerializeField]GameObject menu1;
    [SerializeField] GameObject menu2;
    // Start is called before the first frame update
    private void Awake()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void DisplayScoreMenuOption()
    {
        menu1.SetActive(true);
    }
    
    public void DisplayScoreAndContinueOption()
    {
        menu2.SetActive(true);
    }
}
