using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ShopController : MonoBehaviour
{
    #region Weapon Properties
    [BoxGroup("Weapon Models")][SerializeField] private List<GameObject> weaponModels;

    [BoxGroup("Weapon Upgrade Panels")][SerializeField] private Transform CoinsPanelWeapon;
    [BoxGroup("Weapon Upgrade Panels")][SerializeField] private TMP_Text textCoinsAmountWeapon;

    [BoxGroup("Weapon Stats")][SerializeField] private TMP_Text textDamageValue;
    [BoxGroup("Weapon Stats")][SerializeField] private TMP_Text textClipValue;
    [BoxGroup("Weapon Stats")][SerializeField] private TMP_Text textReloadTimeValue;
    [BoxGroup("Weapon Stats")][SerializeField] private TMP_Text textShootSpeedTimeValue;
    [BoxGroup("Weapon Stats")][SerializeField] private TMP_Text textKnockbackValue;

    [BoxGroup("Weapon Upgrade Costs")][SerializeField] private TMP_Text textDamageUpgradeCost;
    [BoxGroup("Weapon Upgrade Costs")][SerializeField] private TMP_Text textClipUpgradeCost;
    [BoxGroup("Weapon Upgrade Costs")][SerializeField] private TMP_Text textReloadUpgradeCost;
    [BoxGroup("Weapon Upgrade Costs")][SerializeField] private TMP_Text textShootSpeedUpgradeCost;
    [BoxGroup("Weapon Upgrade Costs")][SerializeField] private TMP_Text textKnockbackUpgradeCost;

    [BoxGroup("Weapon Upgrade Levels")][SerializeField] private TMP_Text textDamageCurrentLevel;
    [BoxGroup("Weapon Upgrade Levels")][SerializeField] private TMP_Text textClipCurrentLevel;
    [BoxGroup("Weapon Upgrade Levels")][SerializeField] private TMP_Text textReloadCurrentLevel;
    [BoxGroup("Weapon Upgrade Levels")][SerializeField] private TMP_Text textShootSpeedCurrentLevel;
    [BoxGroup("Weapon Upgrade Levels")][SerializeField] private TMP_Text textKnockbackCurrentLevel;

    [BoxGroup("Weapon Upgrade Buttons")][SerializeField] private Image imageButtonDamageUpgrade;
    [BoxGroup("Weapon Upgrade Buttons")][SerializeField] private Image imageButtonClipUpgrade;
    [BoxGroup("Weapon Upgrade Buttons")][SerializeField] private Image imageButtonReloadUpgrade;
    [BoxGroup("Weapon Upgrade Buttons")][SerializeField] private Image imageButtonShootSpeedUpgrade;
    [BoxGroup("Weapon Upgrade Buttons")][SerializeField] private Image imageButtonknockBackUpgrade;

    [BoxGroup("Weapon Particle Emitters")][SerializeField] private CanvasParticleEmitter canvasParticleEmitterButtonDamageUpgrade;
    [BoxGroup("Weapon Particle Emitters")][SerializeField] private CanvasParticleEmitter canvasParticleEmitterButtonClipUpgrade;
    [BoxGroup("Weapon Particle Emitters")][SerializeField] private CanvasParticleEmitter canvasParticleEmitterButtonReloadUpgrade;
    [BoxGroup("Weapon Particle Emitters")][SerializeField] private CanvasParticleEmitter canvasParticleEmitterButtonShootSpeedUpgrade;
    [BoxGroup("Weapon Particle Emitters")][SerializeField] private CanvasParticleEmitter canvasParticleEmitterButtonKnockbackUpgrade;
    #endregion

    #region Player Properties
    [BoxGroup("Player Upgrade Panels")][SerializeField] private Transform CoinsPanelPlayer;
    [BoxGroup("Player Upgrade Panels")][SerializeField] private TMP_Text textCoinsAmountPlayer;

    [BoxGroup("Player Stats")][SerializeField] private TMP_Text textHpValue;
    [BoxGroup("Player Stats")][SerializeField] private TMP_Text textSprintTimeValue;
    [BoxGroup("Player Stats")][SerializeField] private TMP_Text textSprintReloadSpeedValue;

    [BoxGroup("Player Upgrade Costs")][SerializeField] private TMP_Text textHpUpgradeCost;
    [BoxGroup("Player Upgrade Costs")][SerializeField] private TMP_Text textSprintTimeUpgradeCost;
    [BoxGroup("Player Upgrade Costs")][SerializeField] private TMP_Text textSprintReloadSpeedUpgradeCost;

    [BoxGroup("Player Upgrade Levels")][SerializeField] private TMP_Text textHpCurrentLevel;
    [BoxGroup("Player Upgrade Levels")][SerializeField] private TMP_Text textSprintTimeCurrentLevel;
    [BoxGroup("Player Upgrade Levels")][SerializeField] private TMP_Text textSprintReloadSpeedCurrentLevel;

    [BoxGroup("Player Upgrade Buttons")][SerializeField] private Image imageButtonHpUpgrade;
    [BoxGroup("Player Upgrade Buttons")][SerializeField] private Image imageButtonSprintTimeCurrentUpgrade;
    [BoxGroup("Player Upgrade Buttons")][SerializeField] private Image imageButtonSprintReloadSpeedCurrentUpgrade;

    [BoxGroup("Player Particle Emitters")][SerializeField] private CanvasParticleEmitter canvasParticleEmitterButtonHpUpgrade;
    [BoxGroup("Player Particle Emitters")][SerializeField] private CanvasParticleEmitter canvasParticleEmitterButtonSprintTimeUpgrade;
    [BoxGroup("Player Particle Emitters")][SerializeField] private CanvasParticleEmitter canvasParticleEmitterButtonReloadSpeedCurrentUpgrade;
    #endregion

    #region General Properties
    [SerializeField] public List<double> pricesList = new List<double>();
    [SerializeField] private Sprite canUpdateSprite;
    [SerializeField] private Sprite cantUpdateSprite;
    [SerializeField] private SaveLoadDataManager saveLoadDataManager;
    [SerializeField] private int currentWeaponID;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClipUpgrade;
    [SerializeField] private AudioClip audioClipNoUpgrade;
    [SerializeField] private CanvasParticleEmitter canvasParticleEmitter;
    #endregion

    #region Button Pressed Handlers
    public void OnHpUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.playerUpgradeType.hp, CoinsPanelPlayer, audioClipUpgrade, audioClipNoUpgrade);

    public void OnSprintTimeUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.playerUpgradeType.sprintTime, CoinsPanelPlayer, audioClipUpgrade, audioClipNoUpgrade);

    public void OnSprintReloadSpeedUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed, CoinsPanelPlayer, audioClipUpgrade, audioClipNoUpgrade);

    public void OnDamageUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.weaponUpgradeType.damage, CoinsPanelWeapon, audioClipUpgrade, audioClipNoUpgrade);

    public void OnClipUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.weaponUpgradeType.clip, CoinsPanelWeapon, audioClipUpgrade, audioClipNoUpgrade);

    public void OnReloadTimeUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.weaponUpgradeType.reloadTime, CoinsPanelWeapon, audioClipUpgrade, audioClipNoUpgrade);

    public void OnShootSpeedTimeUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.weaponUpgradeType.shootSpeed, CoinsPanelWeapon, audioClipUpgrade, audioClipNoUpgrade);

    public void OnShootKnockbackUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.weaponUpgradeType.knockback, CoinsPanelWeapon, audioClipUpgrade, audioClipNoUpgrade);

    private void HandleUpgrade(SaveLoadDataManager.playerUpgradeType upgradeType, Transform coinsPanel, AudioClip successClip, AudioClip failureClip)
    {
        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetPlayerUpgradeCost(upgradeType), true))
        {
            saveLoadDataManager.SetPlayerUpgradeLevel(upgradeType);
            UpdateShopItemValues();
            audioSource.PlayOneShot(successClip);
            ParticlesBuyEffect();
        }
        else
        {
            AnimateFailure(coinsPanel, failureClip);
        }
    }

    private void HandleUpgrade(SaveLoadDataManager.weaponUpgradeType upgradeType, Transform coinsPanel, AudioClip successClip, AudioClip failureClip)
    {
        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, upgradeType), true))
        {
            saveLoadDataManager.SetWeaponUpgradeLevel(currentWeaponID, upgradeType);
            UpdateShopItemValues();
            audioSource.PlayOneShot(successClip);
            ParticlesBuyEffect();
        }
        else
        {
            AnimateFailure(coinsPanel, failureClip);
        }
    }

    private void AnimateFailure(Transform coinsPanel, AudioClip failureClip)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(coinsPanel.DOScale(Vector3.one * 1.1f, 0.25f));
        sequence.Join(coinsPanel.GetComponent<Image>().DOColor(Color.red, 0.25f));
        sequence.Append(coinsPanel.DOScale(Vector3.one, 0.25f));
        sequence.Join(coinsPanel.GetComponent<Image>().DOColor(Color.white, 0.25f));
        audioSource.PlayOneShot(failureClip);
    }
    #endregion

    public void UpdateShopItemValues()
    {
        UpdateWeaponValues();
        UpdatePlayerValues();
    }

    private void UpdateWeaponValues()
    {
        textDamageValue.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponDamageValue(currentWeaponID), "0.0");
        textDamageUpgradeCost.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage));

        textClipValue.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponClipValue(currentWeaponID));
        textClipUpgradeCost.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip));

        textReloadTimeValue.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponRealoadTime(currentWeaponID), "0.0") + "s";
        textReloadUpgradeCost.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime));

        textShootSpeedTimeValue.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponShootSpeedTime(currentWeaponID), "0.00") + "s";
        textShootSpeedUpgradeCost.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.shootSpeed));

        textKnockbackValue.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponKnockbackValue(currentWeaponID), "0.00");
        textKnockbackUpgradeCost.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.knockback));

        textCoinsAmountWeapon.text = Formatter.IdleValue(saveLoadDataManager.GetCoins());



        textDamageCurrentLevel.text = "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage);
        textClipCurrentLevel.text = "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip);
        textReloadCurrentLevel.text = "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime);
        textShootSpeedCurrentLevel.text = "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.shootSpeed);
        textKnockbackCurrentLevel.text = "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.knockback);
    }

    private void UpdatePlayerValues()
    {
        textHpValue.text = Formatter.IdleValue(saveLoadDataManager.GetPlayerHpValue(), "0");
        textHpUpgradeCost.text = Formatter.IdleValue(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.hp));
        textSprintTimeValue.text = saveLoadDataManager.GetPlayerSprintTimeValue() + "s";
        textSprintTimeUpgradeCost.text = Formatter.IdleValue(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintTime));
        textSprintReloadSpeedValue.text = Formatter.IdleValue(saveLoadDataManager.GetPlayerSprintReloadSpeedValue(), "0.0");
        textSprintReloadSpeedUpgradeCost.text = Formatter.IdleValue(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed));
       
        textCoinsAmountPlayer.text = Formatter.IdleValue(saveLoadDataManager.GetCoins());

        textHpCurrentLevel.text = "Level:" + saveLoadDataManager.GetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.hp);
        textSprintTimeCurrentLevel.text = "Level:" + saveLoadDataManager.GetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.sprintTime);
        textSprintReloadSpeedCurrentLevel.text = "Level:" + saveLoadDataManager.GetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed);
    }

    private void ParticlesBuyEffect()
    {
        Sequence particleSequence = DOTween.Sequence();
        particleSequence.AppendCallback(() => canvasParticleEmitter.EmiterRate = 20);
        particleSequence.AppendInterval(1);
        particleSequence.AppendCallback(() => canvasParticleEmitter.EmiterRate = 0);
    }

    public void GetPricesList()
    {
        pricesList = new List<double>
        {
            saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage),
            saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip),
            saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime),
            saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.shootSpeed),
            saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.knockback),
            saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.hp),
            saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintTime),
            saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed)
        };
        pricesList.Sort((x, y) => x.CompareTo(y));
    }

    public void OnNextWeaponButton()
    {
        if (currentWeaponID < saveLoadDataManager.weaponsDatas.Count - 1)
            currentWeaponID++;
        UpdateWeaponSelection();
    }

    public void OnPreviousWeaponButton()
    {
        if (currentWeaponID > 0)
            currentWeaponID--;
        UpdateWeaponSelection();
    }

    private void UpdateWeaponSelection()
    {
        GetPricesList();
        UpdateShopItemValues();
        saveLoadDataManager.SetCurrentWeapon(currentWeaponID);
        TurnOnWeaponModel();
    }

    public void TurnOnWeaponModel()
    {
        foreach (GameObject weapon in weaponModels)
        {
            weapon.SetActive(false);
        }
        weaponModels[saveLoadDataManager.GetCurrentWeapon()].SetActive(true);
    }
}