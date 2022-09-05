using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]

public class PlayerData : ScriptableObject
{
    public List<double> hpValue = new List<double>();
    public List<double> hpUpgradeCost = new List<double>();
    public List<double> sprintTimeValue = new List<double>();
    public List<double> sprintTimeUpgradeCost = new List<double>();
    public List<double> sprintReloadSpeedValue = new List<double>();
    public List<double> sprintReloadSpeedUpgradeCost = new List<double>();
}
