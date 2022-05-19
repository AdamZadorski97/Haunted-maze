using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    public static LevelManager _Instance { get; private set; }
    
    public int currentLevelMoneyCollected;
    public int moneyToUnlockKey;
    public GameObject NextLevelKey;




    public void OnMoneyCollect(int value)
    {
        currentLevelMoneyCollected += value;

        if (moneyToUnlockKey >= currentLevelMoneyCollected)
        {
            UnlockNextLevelKey();
        }
    }

    public void UnlockNextLevelKey()
    {
        NextLevelKey.gameObject.SetActive(true);
    }
}
