using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTriggerController : MonoBehaviour
{
    public string title;
    public string constents;
    public Sprite icon;
    public string conditionToClose;
    public bool lockPlayerMovement;

    public void OnHintTriggerEnter()
    {
        PopupController.Instance.OpenPopup(title, constents, icon, conditionToClose, lockPlayerMovement);
        Destroy(this.gameObject);
    }
}
