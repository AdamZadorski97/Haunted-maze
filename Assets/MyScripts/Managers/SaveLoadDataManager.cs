using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class SaveLoadDataManager : MonoBehaviour
{
    public SaveData saveData;
    public List<WeaponDataSO> weaponsDatas;
    public PlayerStatsDataSO playerDatas;
    public LevelDataSO levelDatas;
    public LevelCoinsSO levelCoinsSO;
    public List<LevelData> levelData = new List<LevelData>();
    public bool UpdateData;

    private string CheckPlayerExistLink = "http://skydomesoftware.usermd.net/HauntedMaze/HauntedMazeCheckUserExists.php";
    private string GetPlayerLink = "http://skydomesoftware.usermd.net/HauntedMaze/HauntedMazeGetPlayerSave.php";
    private string InsertPlayerLink = "http://skydomesoftware.usermd.net/HauntedMaze/HauntedMazeInsertNewPlayer.php";
    private string UpdatePlayerLink = "http://skydomesoftware.usermd.net/HauntedMaze/HauntedMazeUpdatePlayerSave.php";



    public List<string> levelNames = new List<string>();

    public enum weaponUpgradeType { damage, clip, reloadTime, shootSpeed, knockback, knockbackChance }
    public enum playerUpgradeType { hp, sprintTime, sprintReloadSpeed }

    public UnityEvent OnDataLoaded;

    private void Start()
    {
        if (UpdateData)
        {
            levelCoinsSO.RetrieveCloudData();
            weaponsDatas[0].RetrieveCloudData();
            weaponsDatas[1].RetrieveCloudData();
            playerDatas.RetrieveCloudData();
            levelDatas.RetrieveCloudData();
        }
        LoadData();
        UpdateWeaponList();
        UpdateLevelList();
        Invoke("UpdateLevelList", 0.15f);
        Invoke("UpdateWeaponList", 0.25f);
    }






    public void UpdateWeaponList()
    {
        while (saveData.upgrades.weaponDataUpgrades.Count < weaponsDatas.Count)
        {

            WeaponDataUpgrades newWeaponDataUpgrade = new WeaponDataUpgrades();
            saveData.upgrades.weaponDataUpgrades.Add(newWeaponDataUpgrade);
        }
    }

    public void UpdateLevelList()
    {

        foreach (string checkLevelName in levelNames)
        {
            bool match = false;
            foreach (LevelData levelData in saveData.upgrades.levelData)
            {
                if (levelData.levelName != checkLevelName)
                {
                    Debug.Log("don't Match: " + checkLevelName + "/" + levelData.levelName);
                    match = false;
                }
                else
                {
                    match = true;
                    break;
                }
            }
            if (match == false)
            {
                LevelData newLevelData = new LevelData();
                newLevelData.levelName = checkLevelName;
                saveData.upgrades.levelData.Add(newLevelData);
                Debug.Log("Added: " + checkLevelName);
            }
        }
    }



    public bool CheckEnoughCoins(double value, bool takeCoins = false)
    {
        if (value > GetCoins())
        {
            return false;
        }
        if (takeCoins)
        {
            TakeCoins(value);
        }
        return true;
    }

    public double GetCoins()
    {
        LoadData();
        return saveData.stats.coinsAmount;
    }

    public void TakeCoins(double value)
    {
        LoadData();
        saveData.stats.coinsAmount -= value;
        SaveData();
    }

    public void AddCoins(double value)
    {
        LoadData();
        saveData.stats.coinsAmount += value;
        SaveData();
    }

    public void SetCoins(double value)
    {
        LoadData();
        saveData.stats.coinsAmount = value;
        SaveData();
    }

    public void SetCurrentWeapon(int value)
    {
        LoadData();
        saveData.stats.currentSelectedWeapon = value;
        SaveData();
    }
    public int GetCurrentWeapon()
    {
        LoadData();
        return saveData.stats.currentSelectedWeapon;
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
            case weaponUpgradeType.shootSpeed:
                saveData.upgrades.weaponDataUpgrades[weaponID].shootSpeedTimeUpgradeLevel++;
                break;
            case weaponUpgradeType.knockback:
                saveData.upgrades.weaponDataUpgrades[weaponID].knockbackUpgradeLevel++;
                break;
            case weaponUpgradeType.knockbackChance:
                saveData.upgrades.weaponDataUpgrades[weaponID].knockbackChanceUpgradeLevel++;
                break;
        }
        Debug.Log($"Upgrade weapon:{weaponID} with upgrade {weaponUpgradeType} ");
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
            case weaponUpgradeType.shootSpeed:
                return saveData.upgrades.weaponDataUpgrades[weaponID].shootSpeedTimeUpgradeLevel;
            case weaponUpgradeType.knockback:
                return saveData.upgrades.weaponDataUpgrades[weaponID].knockbackUpgradeLevel;
            case weaponUpgradeType.knockbackChance:
                return saveData.upgrades.weaponDataUpgrades[weaponID].knockbackChanceUpgradeLevel;
        }
        return 1;
    }




    public double GetWeaponUpgradeCost(int weaponID, weaponUpgradeType weaponUpgradeType)
    {
        switch (weaponUpgradeType)
        {
            case weaponUpgradeType.damage:
                return weaponsDatas[weaponID].weaponData[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType) + 1].damageUpgradeCost;
            case weaponUpgradeType.clip:
                return weaponsDatas[weaponID].weaponData[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType) + 1].clipUpgradeCost;
            case weaponUpgradeType.reloadTime:
                return weaponsDatas[weaponID].weaponData[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType) + 1].reloadTimeUpgradeCost;
            case weaponUpgradeType.shootSpeed:
                return weaponsDatas[weaponID].weaponData[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType) + 1].shootSpeedTimeUpgradeCost;
            case weaponUpgradeType.knockback:
                return weaponsDatas[weaponID].weaponData[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType) + 1].knockbackUpgradeCost;
            case weaponUpgradeType.knockbackChance:
                return weaponsDatas[weaponID].weaponData[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType) + 1].knockbackChanceUpgradeCost;
        }
        return 0;
    }

    public void SetPlayerUpgradeLevel(playerUpgradeType playerUpgradeType)
    {
        LoadData();

        switch (playerUpgradeType)
        {
            case playerUpgradeType.hp:
                saveData.upgrades.playerDataUpgrades.hpUpgradeLevel++;
                break;
            case playerUpgradeType.sprintTime:
                saveData.upgrades.playerDataUpgrades.sprintTimeUpgradeLevel++;
                break;
            case playerUpgradeType.sprintReloadSpeed:
                saveData.upgrades.playerDataUpgrades.sprintReloadSpeedUpgradeLevel++;
                break;

        }
        SaveData();
    }

    public int GetPlayerUpgradeLevel(playerUpgradeType playerUpgradeType)
    {
        LoadData();

        switch (playerUpgradeType)
        {
            case playerUpgradeType.hp:
                return saveData.upgrades.playerDataUpgrades.hpUpgradeLevel;
            case playerUpgradeType.sprintTime:
                return saveData.upgrades.playerDataUpgrades.sprintTimeUpgradeLevel;
            case playerUpgradeType.sprintReloadSpeed:
                return saveData.upgrades.playerDataUpgrades.sprintReloadSpeedUpgradeLevel;

        }
        return 1;
    }

    public double GetPlayerUpgradeCost(playerUpgradeType playerUpgradeType)
    {
        LoadData();

        switch (playerUpgradeType)
        {
            case playerUpgradeType.hp:
                return playerDatas.playerData[GetPlayerUpgradeLevel(playerUpgradeType) + 1].hpUpgradeCost;
            case playerUpgradeType.sprintTime:
                return playerDatas.playerData[GetPlayerUpgradeLevel(playerUpgradeType) + 1].sprintTimeUpgradeCost;
            case playerUpgradeType.sprintReloadSpeed:
                return playerDatas.playerData[GetPlayerUpgradeLevel(playerUpgradeType) + 1].sprintReloadSpeedUpgradeCost;
        }
        return 1;
    }



    public double GetWeaponDamageValue(int weaponID)
    {
        return weaponsDatas[weaponID].weaponData[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.damage)].damageValue;
    }

    public double GetWeaponClipValue(int weaponID)
    {
        return weaponsDatas[weaponID].weaponData[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.clip)].clipValue;
    }

    public double GetWeaponRealoadTime(int weaponID)
    {
        return weaponsDatas[weaponID].weaponData[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.reloadTime)].reloadTimeValue;
    }

    public double GetWeaponShootSpeedTime(int weaponID)
    {
        return weaponsDatas[weaponID].weaponData[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.reloadTime)].shootSpeedTimeValue;
    }

    public double GetWeaponShootSpeedRealoadTime(int weaponID)
    {
        return weaponsDatas[weaponID].weaponData[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.shootSpeed)].shootSpeedTimeValue;
    }

    public double GetWeaponKnockbackValue(int weaponID)
    {
        return weaponsDatas[weaponID].weaponData[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.knockback)].knockbackValue;
    }
    public double GetWeaponKnockbackChanceValue(int weaponID)
    {
        return weaponsDatas[weaponID].weaponData[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.knockbackChance)].knockbackChanceValue;
    }

    public double GetPlayerHpValue()
    {
        Debug.Log(playerDatas.playerData[GetPlayerUpgradeLevel(playerUpgradeType.hp)].hpValue);
        return playerDatas.playerData[GetPlayerUpgradeLevel(playerUpgradeType.hp)].hpValue;
    }

    public double GetPlayerSprintTimeValue()
    {
        return playerDatas.playerData[GetPlayerUpgradeLevel(playerUpgradeType.sprintTime)].sprintTimeValue;
    }

    public double GetPlayerSprintReloadSpeedValue()
    {
        return playerDatas.playerData[GetPlayerUpgradeLevel(playerUpgradeType.sprintReloadSpeed)].sprintReloadSpeedValue;
    }

    public double GetMinDistanceFromPlayer(int levelID)
    {
        return levelDatas.levelData[levelID].minDistanceFromPlayer;
    }

    public double GetMaxDistanceFromPlayer(int levelID)
    {
        return levelDatas.levelData[levelID].maxDistanceFromPlayer;
    }

    public double GetSpawnRate(int levelID)
    {
        return levelDatas.levelData[levelID].spawnRate;
    }

    public double GetTimeUntilSpawnRateIncrease(int levelID)
    {
        return levelDatas.levelData[levelID].timeUntilSpawnRateIncrease;
    }
    public double GetMaxEnemies(int levelID)
    {
        return levelDatas.levelData[levelID].maxEnemies;
    }
    public double GetBossSpawnTime(int levelID)
    {
        return levelDatas.levelData[levelID].bossSpawnTime;
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
       // StartCoroutine(UpdatePlayerData(output => { }));
        Debug.Log("Update data");
    }

    public void SaveDataWithDefault()
    {
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


    public IEnumerator CheckPlayerExist(System.Action<string> onMessageReceived)
    {
        WWWForm form = new WWWForm();
        form.AddField("TableName", "HauntedMazePlayer");
        form.AddField("PlayerName", PlayerPrefs.GetString("NickName"));
        form.AddField("Save", JsonUtility.ToJson(saveData));
        UnityWebRequest www = UnityWebRequest.Post(CheckPlayerExistLink, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (onMessageReceived != null)
            {
                onMessageReceived(www.downloadHandler.text);
                string message = www.downloadHandler.text;
                if (message == "true")
                {
                    StartCoroutine(DownloadPlayerData(output => { }));
                }
                else
                {
                    StartCoroutine(InsertNewPlayerData(output => { }));
                }
            }
        }
    }


    public IEnumerator InsertNewPlayerData(System.Action<string> onMessageReceived)
    {
        WWWForm form = new WWWForm();
        form.AddField("TableName", "HauntedMazePlayer");
        form.AddField("PlayerName", PlayerPrefs.GetString("NickName"));
        form.AddField("Save", JsonUtility.ToJson(saveData));
        UnityWebRequest www = UnityWebRequest.Post(InsertPlayerLink, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (onMessageReceived != null)
            {
                onMessageReceived(www.downloadHandler.text);
                string message = www.downloadHandler.text;
                Debug.Log(message);
            }
        }
    }

    public IEnumerator UpdatePlayerData(System.Action<string> onMessageReceived)
    {
        WWWForm form = new WWWForm();
        form.AddField("TableName", "HauntedMazePlayer");
        form.AddField("PlayerName", PlayerPrefs.GetString("NickName"));
        form.AddField("Save", JsonUtility.ToJson(saveData));
        UnityWebRequest www = UnityWebRequest.Post(UpdatePlayerLink, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (onMessageReceived != null)
            {
                onMessageReceived(www.downloadHandler.text);
                string message = www.downloadHandler.text;
                // Debug.Log(message);
            }
        }
    }

    public IEnumerator DownloadPlayerData(System.Action<string> onMessageReceived)
    {
        WWWForm form = new WWWForm();
        form.AddField("TableName", "HauntedMazePlayer");
        form.AddField("PlayerName", PlayerPrefs.GetString("NickName"));
        form.AddField("Save", "Save");
        UnityWebRequest www = UnityWebRequest.Post(GetPlayerLink, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (onMessageReceived != null)
            {
                onMessageReceived(www.downloadHandler.text);
                string message = www.downloadHandler.text;
                Debug.Log(message);
                if (message != "0 results")
                {
                    saveData = JsonUtility.FromJson<SaveData>(message);
                    SaveData();
                }
            }
        }
    }
}
