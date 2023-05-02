using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



//Maybe inherit this class for Nation UI hover ability, then get different components from Object: E.g. Name, GDP, Pykrete Production
public class HoverTipUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    protected float timeToWait = 0.5f;
    [SerializeField] private string dataToShow;


    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
        
    }

    public void OnPointerExit(PointerEventData eventData)
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
