using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponsData : ScriptableObject
{
    public List<Weapon> weapons = new List<Weapon>();
}
[Serializable]
public class Weapon
{
    public string weaponID;
    public string weaponName;
    public string weaponDescription;
    public List<float> damageValue = new List<float>();
    public List<float> damageUpgradeCost = new List<float>();
    public List<float> clipValue = new List<float>();
    public List<float> clipUpgradeCost = new List<float>();
    public List<float> reloadTimeValue = new List<float>();
    public List<float> reloadTimeUpgradeCost = new List<float>();
}

