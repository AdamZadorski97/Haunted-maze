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
    public WeaponsData weaponsData;
    public PlayerData playerData;

    private string CheckPlayerExistLink = "http://skydomesoftware.usermd.net/HauntedMaze/HauntedMazeCheckUserExists.php";
    private string GetPlayerLink = "http://skydomesoftware.usermd.net/HauntedMaze/HauntedMazeGetPlayerSave.php";
    private string InsertPlayerLink = "http://skydomesoftware.usermd.net/HauntedMaze/HauntedMazeInsertNewPlayer.php";
    private string UpdatePlayerLink = "http://skydomesoftware.usermd.net/HauntedMaze/HauntedMazeUpdatePlayerSave.php";



    public List<string> levelNames = new List<string>();

    public enum weaponUpgradeType { damage, clip, reloadTime }
    public enum playerUpgradeType { hp, sprintTime, sprintReloadSpeed }

    public UnityEvent OnDataLoaded;

    private void Start()
    {
        LoadData();
        Invoke("UpdateLevelList", 0.15f);
        Invoke("UpdateWeaponList", 0.25f);
    }






    public void UpdateWeaponList()
    {
        foreach (Weapon weapon in weaponsData.weapons)
        {
            bool match = false;
            foreach (WeaponDataUpgrades weaponDataUpgrade in saveData.upgrades.weaponDataUpgrades)
            {
                if (weaponDataUpgrade.weaponID != weapon.weaponID)
                {
                    Debug.Log("don't Match: " + weapon.weaponID + "/" + weaponDataUpgrade.weaponID);
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
                WeaponDataUpgrades newWeaponDataUpgrade = new WeaponDataUpgrades();
                newWeaponDataUpgrade.weaponID = weapon.weaponID;
                saveData.upgrades.weaponDataUpgrades.Add(newWeaponDataUpgrade);
                Debug.Log("Added: " + weapon.weaponID);
            }
        }
        SaveData();
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
        SaveData();
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




    public double GetWeaponUpgradeCost(int weaponID, weaponUpgradeType weaponUpgradeType)
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
                return playerData.hpUpgradeCost[GetPlayerUpgradeLevel(playerUpgradeType) + 1];
            case playerUpgradeType.sprintTime:
                return playerData.sprintTimeUpgradeCost[GetPlayerUpgradeLevel(playerUpgradeType) + 1];
            case playerUpgradeType.sprintReloadSpeed:
                return playerData.sprintReloadSpeedUpgradeCost[GetPlayerUpgradeLevel(playerUpgradeType) + 1];
        }
        return 1;
    }



    public double GetWeaponDamageValue(int weaponID)
    {
        return weaponsData.weapons[weaponID].damageValue[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.damage)];
    }

    public double GetWeaponClipValue(int weaponID)
    {
        return weaponsData.weapons[weaponID].clipValue[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.clip)];
    }

    public double GetWeaponRealoadTime(int weaponID)
    {
        return weaponsData.weapons[weaponID].reloadTimeValue[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.reloadTime)];
    }

    public double GetPlayerHpValue()
    {
        return playerData.hpValue[GetPlayerUpgradeLevel(playerUpgradeType.hp)];
    }

    public double GetPlayerSprintTimeValue()
    {
        return playerData.sprintTimeValue[GetPlayerUpgradeLevel(playerUpgradeType.sprintTime)];
    }

    public double GetPlayerSprintReloadSpeedValue()
    {
        return playerData.sprintReloadSpeedValue[GetPlayerUpgradeLevel(playerUpgradeType.sprintReloadSpeed)];
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
        StartCoroutine(UpdatePlayerData(output => { }));
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
                Debug.Log(message);
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
