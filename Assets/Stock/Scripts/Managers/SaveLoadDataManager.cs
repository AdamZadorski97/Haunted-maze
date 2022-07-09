using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadDataManager : MonoBehaviour
{
    public SaveData saveData;
    public WeaponsData weaponsData;
    public enum weaponUpgradeType { damage, clip, reloadTime }
    private void Start()
    {
        LoadData();
    }



    public void SetWeaponUpgradeLevel(int weaponID, weaponUpgradeType weaponUpgradeType)
    {
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
                return weaponsData.weapons[weaponID].damageUpgradeCost[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType)+1];
            case weaponUpgradeType.clip:
                return weaponsData.weapons[weaponID].clipUpgradeCost[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType)+1];
            case weaponUpgradeType.reloadTime:
                return weaponsData.weapons[weaponID].reloadTimeUpgradeCost[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType)+1];
        }
        return 0;
    }

    public float GetWeaponDamageValue(int weaponID)
    {
        return weaponsData.weapons[weaponID].damageValue[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.damage)];
    }

    public float GetWeaponClipValue(int weaponID)
    {
        return weaponsData.weapons[weaponID].damageValue[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.clip)];
    }

    public float GetWeaponRealoadTime(int weaponID)
    {
        return weaponsData.weapons[weaponID].damageValue[GetWeaponUpgradeLevel(weaponID, weaponUpgradeType.reloadTime)];
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

    [Button]
    public void LoadData()
    {
        string subDir = Path.Combine(Application.persistentDataPath, "Saves", "Data");
        Directory.CreateDirectory(subDir);
        string messagepath = Path.Combine(subDir, "SaveData" + ".json");

        if (!File.Exists(messagepath))
        {
            SaveData();
        }
        else
        {
            string jsonString = File.ReadAllText(messagepath);
            saveData = JsonUtility.FromJson<SaveData>(jsonString);
        }
    }
}
