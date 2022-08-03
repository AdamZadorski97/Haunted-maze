using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    public TMP_Text textCurrentAmmunition;
    public TMP_Text textCurrentPoints;
    public TMP_Text textCurrentPointsMultipler;

    public CanvasGroup topPanel;
    public CanvasGroup bottomPanel;

    public Sprite noAmmoIcon;
    public Image reloadButtonImage;
    public Image reloadButtonImageIcon;
    public Color defaultButtonColor;
    public Color alertLowAmmoButtonColor;
    public Color defaultTextColor;
    public Color alertTextColor;

    public Image imageSlideTimer;
    public Image imageJumpTimer;
    public Image imageReloadTimer;
    public Image imageRunTimer;
    public Image blackScreen;

    private void Start()
    {
        blackScreen.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(0.25f);
        sequence.Append(blackScreen.DOColor(new Vector4(0, 0, 0, 0), 0.25f));
        sequence.AppendCallback(() => blackScreen.gameObject.SetActive(false));

    }
    public void HighlitghtAction(Image image, bool enable)
    {
        if (enable)
        {
            image.fillAmount = 1;
        }
        else
        {
            if (image.fillAmount == 1)
            {
                image.fillAmount = 0;
            }
        }
    }

    public void ButtonTimer(Image imageTimer, float time)
    {
        imageTimer.fillAmount = 1;
        imageTimer.DOFillAmount(0, time);
    }


    public void UpdateUI()
    {
        UpdateAmmunition();
        UpdateCurrentPoints();
        UpdateCurrentPointsMultipler();
    }

    public void UpdateAmmunition()
    {
        textCurrentAmmunition.text = $"{LevelManager.Instance.dataManager.AmmunitionInMagazine}/{LevelManager.Instance.dataManager.AmmunitionLeft}";

        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.Append(textCurrentAmmunition.transform.DOScale(Vector3.one * 1.1f, 0.1f));
        scaleSequence.Append(textCurrentAmmunition.transform.DOScale(Vector3.one, 0.1f));



        if (LevelManager.Instance.dataManager.AmmunitionLeft <= 0 && LevelManager.Instance.dataManager.AmmunitionInMagazine <= 0)
        {
            reloadButtonImageIcon.sprite = noAmmoIcon;
            return;
        }

        if (LevelManager.Instance.dataManager.AmmunitionInMagazine < 2)
        {
            textCurrentAmmunition.color = alertTextColor;
        }

        else
        {
            textCurrentAmmunition.color = defaultTextColor;
        }
    }

    public void UpdateCurrentPoints()
    {
        textCurrentPoints.text = $"{LevelManager.Instance.dataManager.CollectedPoints}/{LevelManager.Instance.dataManager.AllLevelPointsAmount}";
        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.Append(textCurrentPoints.transform.DOScale(Vector3.one * 1.1f, 0.1f));
        scaleSequence.Append(textCurrentPoints.transform.DOScale(Vector3.one, 0.1f));
    }

    public void UpdateCurrentPointsMultipler()
    {
        textCurrentPointsMultipler.text = LevelManager.Instance.dataManager.GetCoinMultipler().ToString();
    }
}
