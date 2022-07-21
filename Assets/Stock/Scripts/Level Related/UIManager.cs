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
    public Image reloadButtonImage;
    public Color defaultButtonColor;
    public Color alertLowAmmoButtonColor;
    public Color defaultTextColor;
    public Color alertTextColor;

    public void UpdateUI()
    {
        UpdateAmmunition();
        UpdateCurrentPoints();
        UpdateCurrentPointsMultipler();
    }

    public void UpdateAmmunition()
    {
        textCurrentAmmunition.text = $"{LevelManager.Instance.dataManager.GetAmmunitionInMagazine()}/{LevelManager.Instance.dataManager.GetLeftAmmunition()}";

        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.Append(textCurrentAmmunition.transform.DOScale(Vector3.one * 1.1f, 0.1f));
        scaleSequence.Append(textCurrentAmmunition.transform.DOScale(Vector3.one, 0.1f));

        if (LevelManager.Instance.dataManager.GetAmmunitionInMagazine() < 2)
        {
            reloadButtonImage.color = alertLowAmmoButtonColor;
            textCurrentAmmunition.color = alertTextColor;
        }
        else
        {
            reloadButtonImage.color = defaultButtonColor;
            textCurrentAmmunition.color = defaultTextColor;
        }
    }
    public void UpdateCurrentPoints()
    {
        textCurrentPoints.text = $"{LevelManager.Instance.dataManager.GetCurrentCollectedPoints()}/{LevelManager.Instance.dataManager.GetLevelPointsAmount()}";
        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.Append(textCurrentPoints.transform.DOScale(Vector3.one * 1.1f, 0.1f));
        scaleSequence.Append(textCurrentPoints.transform.DOScale(Vector3.one, 0.1f));
    }
    public void UpdateCurrentPointsMultipler()
    {
        textCurrentPointsMultipler.text = LevelManager.Instance.dataManager.GetCoinMultipler().ToString();
    }
}
