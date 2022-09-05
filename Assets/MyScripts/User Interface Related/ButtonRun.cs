using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonRun : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IUpdateSelectedHandler
{
    public bool isPressed;
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
     
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        PlayerController.Instance.StopRun();
    }

    public void OnUpdateSelected(BaseEventData data)
    {
        if (isPressed)
        {
            PlayerController.Instance.Run();
        }
        else
        {
           
        }
    }
}
