using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommonSystem : MonoBehaviour, IPointerClickHandler
{
    public static ItemSlot SelectedSlot = null;


    public void OnPointerClick(PointerEventData eventData)
    {
        ClearSelectedSlot();
    }


    public static void ClearSelectedSlot()
    {
        if (SelectedSlot != null)
        {
            SelectedSlot.CancelClicked();
            SelectedSlot = null;
        }
    }
}
