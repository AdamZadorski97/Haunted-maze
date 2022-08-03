using UnityEngine;
using System;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "CoinsProportiesData", menuName = "ScriptableObjects/CoinsProportiesData", order = 1)]

[Serializable]
public class CoinsProportiesData : ScriptableObject
{
    public List<int> coinMultiplers = new List<int>();
    public List<Texture> coinTexture = new List<Texture>();
    public List<string> coinMultiplersString = new List<string>();
    public List<Color> coinMultiplersPlateColor = new List<Color>();
    public List<Color> coinMultiplersFrameColor = new List<Color>();
    public List<Color> coinMultiplersTextColor = new List<Color>();
}
