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
    Sequence tutorialSequence;


    public void OpenPopup(string title, float titleFontSize, string constents, float constentsFontSize, Sprite icon, string conditionToClose, bool lockPlayerMovement)
    {
        PlayerController.Instance.SwitchMovement(lockPlayerMovement);

        tutorialSequence = DOTween.Sequence();

        tutorialSequence.Append(canvasGroup.DOFade(1, openingCanvasAnimationTime).SetEase(openingCanvasAnimationTimeCurve));
        tutorialSequence.AppendCallback(() => Time.timeScale = 0.0001f);


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
        if (tutorialSequence != null) tutorialSequence.Kill();

        PlayerController.Instance.SwitchMovement(true);
        canvasGroup.DOFade(0, closeCanvasAnimationTime).SetEase(closeCanvasAnimationTimeCurve);

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        tempConditionToClose = "";
        Time.timeScale = 1f;
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
        if (PlayerController.Instance.isInShootState && tempConditionToClose == "Shoot")
        {
            ClosePopup();
        }
        if (PlayerController.Instance.isInJumpState && tempConditionToClose == "Jump")
        {
            ClosePopup();
        }
        if (PlayerController.Instance.isInSlideState && tempConditionToClose == "Slide")
        {
            ClosePopup();
        }
        if (PlayerController.Instance.isReloading && tempConditionToClose == "Reload")
        {
            ClosePopup();
        }
    }

}
