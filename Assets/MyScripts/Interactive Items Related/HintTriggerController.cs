using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class HintTriggerController : MonoBehaviour
{
    public string title = "Example Title";
    public float titleFontSize;
    public string constents = "Example Constents";
    public float constentsFontSize;
    public Sprite icon;
    [InfoBox("Left, Right, Down, Up, Tap")]
    public string conditionToClose = "Left";
    public bool lockPlayerMovement = true;
    public bool destroyAfterEnter = true;
    public UnityEvent OnEnter;
    

    private void Start()
    {
        if(PlayerPrefs.HasKey(gameObject.name))
        {
            LevelManager.Instance.enemySpawner.hintTriggerControllers.Remove(this);
            Destroy(gameObject);
        }
    }



    public void OnHintTriggerEnter()
    {
        PlayerPrefs.SetString(gameObject.name, "Done");
        PopupController.Instance.OpenPopup(title, titleFontSize, constents, constentsFontSize, icon, conditionToClose, !lockPlayerMovement);
        OnEnter.Invoke();


        if (destroyAfterEnter)
        {
            LevelManager.Instance.enemySpawner.hintTriggerControllers.Remove(this);
            Destroy(this.gameObject);
        }
    }
    public void Unlock(string move)
    {
        if(move == "left")
        {
            PlayerController.Instance.canMoveLeft = true;
        }
        if (move == "right")
        {
            PlayerController.Instance.canMoveRight = true;
        }
        if (move == "back")
        {
            PlayerController.Instance.canMoveBack = true;
        }
    }
    public void Lock(string move)
    {
        if (move == "left")
        {
            PlayerController.Instance.canMoveLeft = false;
        }
        if (move == "right")
        {
            PlayerController.Instance.canMoveRight = false;
        }
        if (move == "back")
        {
            PlayerController.Instance.canMoveBack = false;
        }
    }

}
