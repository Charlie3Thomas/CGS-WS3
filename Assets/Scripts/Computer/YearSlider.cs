using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearSlider : MonoBehaviour
{
    void OnMouseOver()
    {
        CustomCursor.Instance.OnHoverOverHorizontalSlider();
    }
    private void OnMouseExit()
    {
        CustomCursor.Instance.SetDefaultCursor();
    }
}
