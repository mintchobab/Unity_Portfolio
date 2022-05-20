using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickTouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Joystick joystick;


    public void OnPointerDown(PointerEventData eventData)
    {
        joystick.gameObject.SetActive(true);
        joystick.transform.position = eventData.position;

        joystick.BeginDrag(eventData);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.EndDrag(eventData);

        joystick.gameObject.SetActive(false);
    }
}
