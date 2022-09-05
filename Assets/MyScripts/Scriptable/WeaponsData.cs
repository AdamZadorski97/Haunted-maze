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
    public int weaponID;
    public string weaponName;
    public string weaponDescription;
    public List<double> damageValue = new List<double>();
    public List<double> damageUpgradeCost = new List<double>();
    public List<double> clipValue = new List<double>();
    public List<double> clipUpgradeCost = new List<double>();
    public List<double> reloadTimeValue = new List<double>();
    public List<double> reloadTimeUpgradeCost = new List<double>();
}

