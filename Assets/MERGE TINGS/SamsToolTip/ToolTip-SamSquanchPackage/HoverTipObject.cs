using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



//Maybe inherit this class for Nation UI hover ability, then get different components from Object: E.g. Name, GDP, Pykrete Production
public class HoverTipObject : MonoBehaviour
{
    
    [SerializeField] protected float timeToWait = 0.1f;
    [SerializeField] private string dataToShow;


    public void OnMouseEnter()
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
        
    }

    public void OnMouseExit()
    {
        StopAllCoroutines();
        UIHoverManager.OnLoseFocus();
    }

    public virtual void ShowMessage(string _dataToShow)
    {
        
        UIHoverManager.OnMouseHover(_dataToShow, Input.mousePosition);
    }

    protected IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);

        ShowMessage(dataToShow);
    }

}
