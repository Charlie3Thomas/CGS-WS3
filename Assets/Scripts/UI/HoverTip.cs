using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string tipToShow;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(DelayTime());
        Debug.Log("Hovered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {   
        
        StopAllCoroutines();
        HoverTipManager.OnMouseLoseFOcus();
        Debug.Log("Hover Exit");
    }

    private void ShowToolTipText()
    {
        HoverTipManager.OnMouseHover(tipToShow, Input.mousePosition);
    }

    private IEnumerator DelayTime()
    {
        yield return new WaitForSeconds(0.2f);
        ShowToolTipText();

    }

}
