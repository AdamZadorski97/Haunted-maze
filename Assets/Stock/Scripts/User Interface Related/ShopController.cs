using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopController : MonoBehaviour
{
    [SerializeField] private TMP_Text textDamageValue;
    [SerializeField] private TMP_Text textClipValue;
    [SerializeField] private TMP_Text textReloadTimeValue;

    [SerializeField] private TMP_Text textDamageUpgradeCost;
    [SerializeField] private TMP_Text textClipUpgradeCost;
    [SerializeField] private TMP_Text textReloadUpgradeCost;

    [SerializeField] private SaveLoadDataManager saveLoadDataManager;
    [SerializeField] private int currentWeaponID;
    public void OnDamageUpgradeButtonPressed()
    {
        saveLoadDataManager.SetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage);
        UpdateShopItemValues();
    }

    public void OnClipUpgradeButtonPressed()
    {
        saveLoadDataManager.SetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip);
        UpdateShopItemValues();
    }

    public void OnReloadTimeUpgradeButtonPressed()
    {
        saveLoadDataManager.SetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime);
        UpdateShopItemValues();
    }

    public void UpdateShopItemValues()
    {
        textDamageValue.text = saveLoadDataManager.GetWeaponDamageValue(currentWeaponID).ToString();
        textDamageUpgradeCost.text = saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage).ToString();
        textClipValue.text = saveLoadDataManager.GetWeaponClipValue(currentWeaponID).ToString();
        textClipUpgradeCost.text = saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip).ToString();
        textClipValue.text = saveLoadDataManager.GetWeaponRealoadTime(currentWeaponID).ToString();
        textReloadUpgradeCost.text = saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime).ToString();
    }
}
