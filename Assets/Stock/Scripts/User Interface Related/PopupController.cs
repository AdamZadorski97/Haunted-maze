using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class PopupController : MonoSingleton<PopupController>
{
    public static PopupController _Instance { get; private set; }

    public CanvasGroup canvasGroup;
    public float openingCanvasAnimationTime;
    public AnimationCurve openingCanvasAnimationTimeCurve;
    public float closeCanvasAnimationTime;
    public AnimationCurve closeCanvasAnimationTimeCurve;
    public TMP_Text popupTitle;
    public TMP_Text popupContents;
    public Image popupIcon;
    public SwipeController swipeController;
    private string tempConditionToClose;

  

    public void OpenPopup(string title,float titleFontSize, string constents,float constentsFontSize, Sprite icon, string conditionToClose, bool lockPlayerMovement)
    {
        PlayerController.Instance.SwitchMovement(lockPlayerMovement);
        canvasGroup.DOFade(1, openingCanvasAnimationTime).SetEase(openingCanvasAnimationTimeCurve);
        if (title != "") popupTitle.text = title;
        if (title != "") popupTitle.fontSize = titleFontSize;
        if (constents != "") popupContents.text = constents;
        if (title != "") popupContents.fontSize = constentsFontSize;
        if (icon != null) popupIcon.sprite = icon; else popupIcon.sprite = null;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        tempConditionToClose = conditionToClose;
    }

    public void ClosePopup()
    {
        PlayerController.Instance.SwitchMovement(true);
        canvasGroup.DOFade(0, closeCanvasAnimationTime).SetEase(closeCanvasAnimationTimeCurve);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        tempConditionToClose = "";
    }
    private void Update()
    {
        CheckConditions();
    }
    public void CheckConditions()
    {
        if (swipeController.SwipeLeft && tempConditionToClose == "Left")
        {
            ClosePopup();
        }
        if (swipeController.SwipeRight && tempConditionToClose == "Right")
        {
            ClosePopup();
        }
        if (swipeController.SwipeDown && tempConditionToClose == "Down")
        {
            ClosePopup();
        }
        if (swipeController.SwipeUp && tempConditionToClose == "Up")
        {
            ClosePopup();
        }
        if (swipeController.Tap && tempConditionToClose == "Tap")
        {
            ClosePopup();
        }
    }

}
