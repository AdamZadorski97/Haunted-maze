using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class HintTriggerController : MonoBehaviour
{
    public string title = "Example Title";
    public string constents = "Example Constents";
    public Sprite icon;
    [InfoBox("Left, Right, Down, Up, Tap")]
    public string conditionToClose = "Left";
    public bool lockPlayerMovement = true;
    public bool destroyAfterEnter = true;
    public void OnHintTriggerEnter()
    {
        PopupController.Instance.OpenPopup(title, constents, icon, conditionToClose, !lockPlayerMovement);
        if (destroyAfterEnter)
        {
            Destroy(this.gameObject);
        }

    }
}
