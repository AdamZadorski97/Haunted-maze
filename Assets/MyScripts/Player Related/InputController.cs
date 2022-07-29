using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool leftButton;
    public bool rightButton;
    public bool backButton;
    public PlayerController playerController;
    public bool useKeyboard;

    void Update()
    {
        if (useKeyboard)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                playerController.moveLeft = true;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                playerController.moveRight = true;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                playerController.moveBack = true;
            }
            if (!Input.GetKey(KeyCode.LeftArrow))
            {
                playerController.moveLeft = false;
            }
            if (!Input.GetKey(KeyCode.RightArrow))
            {
                playerController.moveRight = false;
            }
            if (!Input.GetKey(KeyCode.DownArrow))
            {
                playerController.moveBack = false;
            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (leftButton || Input.GetKey(KeyCode.LeftArrow))
        {
            playerController.moveLeft = true;
        }
        if (rightButton || Input.GetKey(KeyCode.RightArrow))
        {
            playerController.moveRight = true;
        }
        if (backButton || Input.GetKey(KeyCode.DownArrow))
        {
            playerController.moveBack = true;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (leftButton || !Input.GetKey(KeyCode.LeftArrow))
        {
            playerController.moveLeft = false;
        }
        if (rightButton || !Input.GetKey(KeyCode.RightArrow))
        {
            playerController.moveRight = false;
        }
        if (backButton || !Input.GetKey(KeyCode.DownArrow))
        {
            playerController.moveBack = false;
        }
    }
}
