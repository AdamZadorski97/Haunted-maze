using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TMP_Text textCurrentAmmunition;
    public TMP_Text textCurrentPoints;
    public TMP_Text textCurrentPointsMultipler;

    public void UpdateUI()
    {
        UpdateAmmunition();
        UpdateCurrentPoints();
        UpdateCurrentPointsMultipler();
    }

    public void UpdateAmmunition()
    {
        textCurrentAmmunition.text = LevelManager.Instance.dataManager.GetCurrentAmmuniton().ToString();
    }
    public void UpdateCurrentPoints()
    {
        textCurrentPoints.text = $"{LevelManager.Instance.dataManager.GetCurrentCollectedPoints()}/{LevelManager.Instance.dataManager.GetLevelPointsAmount()}";
    }
    public void UpdateCurrentPointsMultipler()
    {
        textCurrentPointsMultipler.text = LevelManager.Instance.dataManager.GetCurrentPointsPointsMultipler().ToString();
    }
}
