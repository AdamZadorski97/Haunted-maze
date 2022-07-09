using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopController : MonoBehaviour
{
    [SerializeField] private TMP_Text textCoinsAmount;

    [SerializeField] private TMP_Text textDamageValue;
    [SerializeField] private TMP_Text textClipValue;
    [SerializeField] private TMP_Text textReloadTimeValue;

    [SerializeField] private TMP_Text textDamageUpgradeCost;
    [SerializeField] private TMP_Text textClipUpgradeCost;
    [SerializeField] private TMP_Text textReloadUpgradeCost;

    [SerializeField] private SaveLoadDataManager saveLoadDataManager;
    [SerializeField] private int currentWeaponID;

    [SerializeField] private AudioSource audioSource;
   [SerializeField] private AudioClip audioClipUpgrade;
    [SerializeField] private AudioClip audioClipNoUpgrade;

    private void Start()
    {
        UpdateShopItemValues();
    }

    public void OnDamageUpgradeButtonPressed()
    {
        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage)))
        {
            saveLoadDataManager.SetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage);
            UpdateShopItemValues();
            audioSource.PlayOneShot(audioClipUpgrade);
        }
        else
        {
            audioSource.PlayOneShot(audioClipNoUpgrade);
        }
    }

    public void OnClipUpgradeButtonPressed()
    {
        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip)))
        {
            saveLoadDataManager.SetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip);
            UpdateShopItemValues();
            audioSource.PlayOneShot(audioClipUpgrade);
        }
        else
        {
            audioSource.PlayOneShot(audioClipNoUpgrade);
        }
    }

    public void OnReloadTimeUpgradeButtonPressed()
    {
        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime)))
        {
            saveLoadDataManager.SetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime);
            UpdateShopItemValues();
            audioSource.PlayOneShot(audioClipUpgrade);
        }
        else
        {
            audioSource.PlayOneShot(audioClipNoUpgrade);
        }
    }

    public void UpdateShopItemValues()
    {
        textDamageValue.text = saveLoadDataManager.GetWeaponDamageValue(currentWeaponID).ToString();
        textDamageUpgradeCost.text = saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage).ToString();
        textClipValue.text = saveLoadDataManager.GetWeaponClipValue(currentWeaponID).ToString();
        textClipUpgradeCost.text = saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip).ToString();
        textReloadTimeValue.text = saveLoadDataManager.GetWeaponRealoadTime(currentWeaponID).ToString();
        textReloadUpgradeCost.text = saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime).ToString();
        textCoinsAmount.text = saveLoadDataManager.GetCoins().ToString();
    }
}
