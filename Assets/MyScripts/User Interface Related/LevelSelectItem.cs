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
    [SerializeField] private AudioClip upgradeLevelSound;
    [SerializeField] private AudioClip canUupgradeLevelSound;
    [SerializeField] private AudioSource audioSource;
    public string levelName;
    public string prestigeMultipler;
    public string topScore;

 
    public void OnEnable()
    {
        UpdateUI();
    }

    public void OnButtonBuyPrestige()
    {
        if(saveLoadDataManager.CheckEnoughCoins(levelProportiesData.levelPrestigeCost[saveLoadDataManager.GetLevelPrestigeLevel(levelName) + 1], true))
        {
            audioSource.PlayOneShot(upgradeLevelSound);
            saveLoadDataManager.AddLevelPrestigeLevel(levelName);
        }
        else
        {
            audioSource.PlayOneShot(canUupgradeLevelSound);
        }
        UpdateUI();
    }
    public void UpdateUI()
    {
        textTopScore.text = $"Top Score: {Formatter.IdleValue( saveLoadDataManager.GetLevelTopScore(levelName))}";
        textPrestigeLevel.text = $"{Formatter.IdleValue(coinsProportiesData.coinMultiplers[saveLoadDataManager.GetLevelPrestigeLevel(levelName)+1])}x";
        textPrestigeUpgradeCost.text = $"{Formatter.IdleValue(levelProportiesData.levelPrestigeCost[saveLoadDataManager.GetLevelPrestigeLevel(levelName) + 1])}";
        levelSelectController.UpdateShopItemValues();
    }
}
