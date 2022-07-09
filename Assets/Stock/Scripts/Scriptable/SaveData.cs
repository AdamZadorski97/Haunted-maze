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
    public float coinsAmount;
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





