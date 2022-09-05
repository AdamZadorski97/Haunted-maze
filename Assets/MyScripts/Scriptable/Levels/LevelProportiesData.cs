using UnityEngine;
using System;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "LevelProportiesData", menuName = "ScriptableObjects/LevelProportiesData", order = 1)]

[Serializable]
public class LevelProportiesData : ScriptableObject
{
    public List<int> levelPrestigeCost = new List<int>();
}
