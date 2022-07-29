using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelSelectItem : MonoBehaviour
{
    [SerializeField] private TMP_Text textTopScore;
    [SerializeField] private TMP_Text textPrestigeLevel;
    [SerializeField] private TMP_Text textPrestigeUpgradeCost;
    [SerializeField] private LevelSelectController levelSelectController;
    [SerializeField] private SaveLoadDataManager saveLoadDataManager;
    [SerializeField] private CoinsProportiesData coinsProportiesData;
    [SerializeField] private LevelProportiesData levelProportiesData;
    public string levelName;
    public string prestigeMultipler;
    public string topScore;

 
    public void OnEnable()
    {
        UpdateUI();
    }

    public void OnButtonBuyPrestige()
    {
        if(saveLoadDataManager.CheckEnoughCoins(levelProportiesData.levelPrestigeCost[saveLoadDataManager.GetLevelPrestigeLevel(levelName) + 1]))
        {
            saveLoadDataManager.AddLevelPrestigeLevel(levelName);
        }
        UpdateUI();
    }
    public void UpdateUI()
    {
        textTopScore.text = $"Top Score: {saveLoadDataManager.GetLevelTopScore(levelName)}";
        textPrestigeLevel.text = $"{coinsProportiesData.coinMultiplersString[saveLoadDataManager.GetLevelPrestigeLevel(levelName)]}x";
        textPrestigeUpgradeCost.text = $"{levelProportiesData.levelPrestigeCost[saveLoadDataManager.GetLevelPrestigeLevel(levelName) + 1]}";
        levelSelectController.UpdateShopItemValues();
    }
}
