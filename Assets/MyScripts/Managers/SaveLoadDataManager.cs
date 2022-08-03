using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadDataManager : MonoBehaviour
{
    public SaveData saveData;
    public WeaponsData weaponsData;

    public List<string> levelNames = new List<string>();

    public enum weaponUpgradeType { damage, clip, reloadTime }
    private void Start()
    {
        LoadData();
        UpdateLevelList();
    }

    private void UpdateLevelList()
    {
        foreach (string checkLevelName in levelNames)
        {
            foreach (LevelData levelData in saveData.upgrades.levelData)
            {
                if (levelData.levelName == checkLevelName)
                {
                    return;
                }
            }
            LevelData newLevelData = new LevelData();
            newLevelData.levelName = checkLevelName;
            saveData.upgrades.levelData.Add(newLevelData);
        }
        SaveData();
    }



    public bool CheckEnoughCoins(float value, bool takeCoins = false)
    {
        if (value > GetCoins())
        {
            Debug.Log($"don't have enouth Coins  Coins: {GetCoins()}  value: {value}");
            return false;
        }
        Debug.Log($"You have enouth Coins  Coins: {GetCoins()}  value: {value}");
        if (takeCoins)
        {
            TakeCoins(value);
        }
        return true;
    }

    public float GetCoins()
    {
        LoadData();
        return saveData.stats.coinsAmount;
    }

    public void TakeCoins(float value)
    {
        LoadData();
        saveData.stats.coinsAmount -= value;
        SaveData();
    }

    public void AddCoins(float value)
    {
        LoadData();
        saveData.stats.coinsAmount += value;
        SaveData();
    }

    public void SetCoins(float value)
    {
        LoadData();
        saveData.stats.coinsAmount = value;
        SaveData();
    }

    public void SetQualitySettings(int value)
    {
        LoadData();
        saveData.settings.graphicValue = value;
        SaveData();
    }
    public int GetQualitySettings()
    {
        LoadData();
        return saveData.settings.graphicValue;
    }

    public void SetWeaponUpgradeLevel(int weaponID, weaponUpgradeType weaponUpgradeType)
    {
        LoadData();
        switch (weaponUpgradeType)
        {
            case weaponUpgradeType.damage:
                saveData.upgrades.weaponDataUpgrades[weaponID].damageUpgradeLevel++;
                break;
            case weaponUpgradeType.clip:
                saveData.upgrades.weaponDataUpgrades[weaponID].clipUpgradeLevel++;
                break;
            case weaponUpgradeType.reloadTime:
                saveData.upgrades.weaponDataUpgrades[weaponID].reloadTimeUpgradeLevel++;
                break;
        }
        SaveData();
    }

    public int GetWeaponUpgradeLevel(int weaponID, weaponUpgradeType weaponUpgradeType)
    {
        LoadData();

        switch (weaponUpgradeType)
        {
            case weaponUpgradeType.damage:
                return saveData.upgrades.weaponDataUpgrades[weaponID].damageUpgradeLevel;
            case weaponUpgradeType.clip:
                return saveData.upgrades.weaponDataUpgrades[weaponID].clipUpgradeLevel;
            case weaponUpgradeType.reloadTime:
                return saveData.upgrades.weaponDataUpgrades[weaponID].reloadTimeUpgradeLevel;
        }
        return 1;
    }

    public float GetWeaponUpgradeCost(int weaponID, weaponUpgradeType weaponUpgradeType)
    {
        switch (weaponUpgradeType)
        {
            case weaponUpgradeType.damage:
                return weaponsData.weapons[weaponID].damageUpgradeCost[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType) + 1];
            case weaponUpgradeType.clip:
                return weaponsData.weapons[weaponID].clipUpgradeCost[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType) + 1];
            case weaponUpgradeType.reloadTime:
                return weaponsData.weapons[weaponID].reloadTimeUpgradeCost[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType) + 1];
        }
        return 0;
    }

    public float GetWeaponDamageValue(int weaponID)
    {
        return weaponsData.weapons[weaponID].damageValue[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.damage)];
    }

    public float GetWeaponClipValue(int weaponID)
    {
        return weaponsData.weapons[weaponID].clipValue[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.clip)];
    }

    public float GetWeaponRealoadTime(int weaponID)
    {
        return weaponsData.weapons[weaponID].reloadTimeValue[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.reloadTime)];
    }

    public int GetLevelPrestigeLevel(string levelName)
    {
        foreach (LevelData levelData in saveData.upgrades.levelData)
        {
            if (levelData.levelName == levelName)
                return levelData.levelPrestigeLevel;

        }
        return 0;
    }

    public void AddLevelPrestigeLevel(string levelName)
    {
        foreach (LevelData levelData in saveData.upgrades.levelData)
        {
            if (levelData.levelName == levelName)
            {
                levelData.levelPrestigeLevel++;
                SaveData();
                return;
            }
        }

    }

    public int SetLevelTopScore(string levelName, int topScore)
    {
        foreach (LevelData levelData in saveData.upgrades.levelData)
        {
            if (levelData.levelName == levelName)
            {
                levelData.topScore = topScore;
                SaveData();
            }
            else
                return 0;
        }
        return 0;
    }

    public int GetLevelTopScore(string levelName)
    {
        foreach (LevelData levelData in saveData.upgrades.levelData)
        {
            if (levelData.levelName == levelName)
                return levelData.topScore;
            else
                return 0;

        }
        return 0;
    }

    [Button]
    public void SaveData()
    {
        string subDir = Path.Combine(Application.persistentDataPath, "Saves", "Data");
        Directory.CreateDirectory(subDir);
        string messagepath = Path.Combine(subDir, "SaveData" + ".json");
        string jsonSaveString = JsonUtility.ToJson(saveData);
        File.WriteAllText(messagepath, jsonSaveString);
    }

    public void SaveDataWithDefault()
    {
        WeaponDataUpgrades weaponDataUpgrades = new WeaponDataUpgrades();
        LevelData levelData = new LevelData();
        saveData.upgrades.weaponDataUpgrades.Add(weaponDataUpgrades);
        saveData.upgrades.levelData.Add(levelData);


        string subDir = Path.Combine(Application.persistentDataPath, "Saves", "Data");
        Directory.CreateDirectory(subDir);
        string messagepath = Path.Combine(subDir, "SaveData" + ".json");
        string jsonSaveString = JsonUtility.ToJson(saveData);
        File.WriteAllText(messagepath, jsonSaveString);
    }



    [Button]
    public void LoadData()
    {
        string subDir = Path.Combine(Application.persistentDataPath, "Saves", "Data");
        string messagepath = Path.Combine(subDir, "SaveData" + ".json");

        if (!File.Exists(messagepath))
        {
            SaveDataWithDefault();
        }
        else
        {
            string jsonString = File.ReadAllText(messagepath);
            saveData = JsonUtility.FromJson<SaveData>(jsonString);
        }
    }

    [Button]
    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
        string subDir = Path.Combine(Application.persistentDataPath);
        if (Directory.Exists(subDir)) { Directory.Delete(subDir, true); }
    }
}
