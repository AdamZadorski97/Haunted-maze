using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObjects/SaveData", order = 1)]
[Serializable]
public class SaveData
{
    public Stats stats;
    public Settings settings;
    public Upgrades upgrades;
}

[Serializable]
public class Stats
{
    public Double coinsAmount;
}

[Serializable]
public class Settings
{
    public float fxValue;
    public float musicValue;
    public int graphicValue;
}

[Serializable]
public class Upgrades
{
    public List<WeaponDataUpgrades> weaponDataUpgrades = new List<WeaponDataUpgrades>();
    public PlayerDataUpgrades playerDataUpgrades;
    public List<LevelData> levelData = new List<LevelData>();
}

[Serializable]
public class WeaponDataUpgrades
{
    public int currentSelectedWeapon;
    public int weaponID;
    public int damageUpgradeLevel;
    public int clipUpgradeLevel;
    public int reloadTimeUpgradeLevel;
}

[Serializable]
public class PlayerDataUpgrades
{
    public int hpUpgradeLevel;
    public int sprintTimeUpgradeLevel;
    public int sprintReloadSpeedUpgradeLevel;
}

[Serializable]
public class LevelData
{
    public string levelName;
    public int topScore;
    public int levelPrestigeLevel;
}

