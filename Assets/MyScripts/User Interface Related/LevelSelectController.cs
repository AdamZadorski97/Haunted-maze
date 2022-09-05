using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelSelectController : MonoBehaviour
{
    [SerializeField] private TMP_Text textCoinsAmount;
    [SerializeField] private SaveLoadDataManager saveLoadDataManager;

    private void OnEnable()
    {
        UpdateShopItemValues();
    }

    public void UpdateShopItemValues()
    {
        textCoinsAmount.text = Formatter.IdleValue(saveLoadDataManager.GetCoins());
    }
}