using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
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

    private void Start()
    {
        if(PlayerPrefs.HasKey(gameObject.name))
        {
            Destroy(gameObject);
        }
    }



    public void OnHintTriggerEnter()
    {
        Time.timeScale = 0.01f;
        PlayerPrefs.SetString(gameObject.name, "Done");
        PopupController.Instance.OpenPopup(title, titleFontSize, constents, constentsFontSize, icon, conditionToClose, !lockPlayerMovement);
        if (destroyAfterEnter)
        {
            Destroy(this.gameObject);
        }
    }
}
