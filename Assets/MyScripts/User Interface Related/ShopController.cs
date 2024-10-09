using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ShopController : MonoBehaviour
{
    [BoxGroup("Current Coins")]
    [SerializeField] private Transform coinsPanel;
    [SerializeField] private TMP_Text textCoinsAmount;
    [SerializeField] private TMP_Text freeCoinsText;
    #region Weapon Properties
    [BoxGroup("Weapon Models")]
    [SerializeField] private List<GameObject> weaponModels;



    [Header("Weapon Upgrade Items")]
    [SerializeField] private UpgradeItem damageUpgrade;
    [SerializeField] private UpgradeItem clipUpgrade;
    [SerializeField] private UpgradeItem reloadTimeUpgrade;
    [SerializeField] private UpgradeItem shootSpeedUpgrade;
    [SerializeField] private UpgradeItem knockbackUpgrade;
    [SerializeField] private UpgradeItem knockbackChanceUpgrade;
    [SerializeField] private UpgradeItem criticalChanceUpgrade;
    [SerializeField] private UpgradeItem criticalFactorUpgrade;
    #endregion

    #region Player Properties



    [Header("Player Upgrade Items")]
    [SerializeField] private UpgradeItem hpUpgrade;
    [SerializeField] private UpgradeItem sprintTimeUpgrade;
    [SerializeField] private UpgradeItem sprintReloadSpeedUpgrade;
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
    public void OnHpUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.playerUpgradeType.hp, coinsPanel, audioClipUpgrade, audioClipNoUpgrade);

    public void OnSprintTimeUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.playerUpgradeType.sprintTime, coinsPanel, audioClipUpgrade, audioClipNoUpgrade);

    public void OnSprintReloadSpeedUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed, coinsPanel, audioClipUpgrade, audioClipNoUpgrade);

    public void OnDamageUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.weaponUpgradeType.damage, coinsPanel, audioClipUpgrade, audioClipNoUpgrade);

    public void OnClipUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.weaponUpgradeType.clip, coinsPanel, audioClipUpgrade, audioClipNoUpgrade);

    public void OnReloadTimeUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.weaponUpgradeType.reloadTime, coinsPanel, audioClipUpgrade, audioClipNoUpgrade);

    public void OnShootSpeedTimeUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.weaponUpgradeType.shootSpeed, coinsPanel, audioClipUpgrade, audioClipNoUpgrade);

    public void OnShootKnockbackUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.weaponUpgradeType.knockback, coinsPanel, audioClipUpgrade, audioClipNoUpgrade);

    public void OnShootKnockbackChanceUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.weaponUpgradeType.knockbackChance, coinsPanel, audioClipUpgrade, audioClipNoUpgrade);

    public void OnShootCriticalChanceUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.weaponUpgradeType.criticalChance, coinsPanel, audioClipUpgrade, audioClipNoUpgrade);

    public void OnShootCriticalFactorUpgradeButtonPressed() => HandleUpgrade(SaveLoadDataManager.weaponUpgradeType.criticalFactor, coinsPanel, audioClipUpgrade, audioClipNoUpgrade);
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
        GetPricesList();
        freeCoinsText.text = Formatter.IdleValue(pricesList[0] * 4);
    }

    public void UpdateWeaponValues()
    {
        bool canUpgradeDamage = saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage), false);
        damageUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetWeaponDamageValue(currentWeaponID), "0.0"),
            Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage)),
            "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage),
            canUpgradeDamage
        );
        damageUpgrade.SetUpgradeAction(OnDamageUpgradeButtonPressed);

        bool canUpgradeClip = saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip), false);
        clipUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetWeaponClipValue(currentWeaponID)),
            Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip)),
            "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip),
            canUpgradeClip
        );
        clipUpgrade.SetUpgradeAction(OnClipUpgradeButtonPressed);

        bool canUpgradeReloadTime = saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime), false);
        reloadTimeUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetWeaponRealoadTime(currentWeaponID), "0.0") + "s",
            Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime)),
            "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime),
            canUpgradeReloadTime
        );
        reloadTimeUpgrade.SetUpgradeAction(OnReloadTimeUpgradeButtonPressed);

        bool canUpgradeShootSpeed = saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.shootSpeed), false);
        shootSpeedUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetWeaponShootSpeedTime(currentWeaponID), "0.00") + "s",
            Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.shootSpeed)),
            "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.shootSpeed),
            canUpgradeShootSpeed
        );
        shootSpeedUpgrade.SetUpgradeAction(OnShootSpeedTimeUpgradeButtonPressed);

        bool canUpgradeKnockback = saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.knockback), false);
        knockbackUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetWeaponKnockbackValue(currentWeaponID), "0.00"),
            Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.knockback)),
            "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.knockback),
            canUpgradeKnockback
        );
        knockbackUpgrade.SetUpgradeAction(OnShootKnockbackUpgradeButtonPressed);

        bool canUpgradeKnockbackChance = saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.knockbackChance), false);
        knockbackChanceUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetWeaponKnockbackChanceValue(currentWeaponID), "0") + "%",
            Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.knockbackChance)),
            "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.knockbackChance),
            canUpgradeKnockbackChance
        );
        knockbackChanceUpgrade.SetUpgradeAction(OnShootKnockbackChanceUpgradeButtonPressed);

        bool canUpgradeCriticalChance = saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.criticalChance), false);
        criticalChanceUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetWeaponCriticalChanceValue(currentWeaponID), "0") + "%",
            Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.criticalChance)),
            "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.criticalChance),
            canUpgradeCriticalChance
        );
        criticalChanceUpgrade.SetUpgradeAction(OnShootCriticalChanceUpgradeButtonPressed);

        bool canUpgradeCriticalFactor = saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.criticalFactor), false);
        criticalFactorUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetWeaponCriticalFactorValue(currentWeaponID)) + "%",
            Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.criticalFactor)),
            "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.criticalFactor),
            canUpgradeCriticalFactor
        );
        criticalFactorUpgrade.SetUpgradeAction(OnShootCriticalFactorUpgradeButtonPressed);

        textCoinsAmount.text = Formatter.IdleValue(saveLoadDataManager.GetCoins());
    }

    public void UpdatePlayerValues()
    {
        bool canUpgradeHp = saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.hp), false);
        hpUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetPlayerHpValue(), "0"),
            Formatter.IdleValue(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.hp)),
            "Level:" + saveLoadDataManager.GetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.hp),
            canUpgradeHp
        );
        hpUpgrade.SetUpgradeAction(OnHpUpgradeButtonPressed);

        bool canUpgradeSprintTime = saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintTime), false);
        sprintTimeUpgrade.SetUpgradeValues(
            saveLoadDataManager.GetPlayerSprintTimeValue() + "s",
            Formatter.IdleValue(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintTime)),
            "Level:" + saveLoadDataManager.GetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.sprintTime),
            canUpgradeSprintTime
        );
        sprintTimeUpgrade.SetUpgradeAction(OnSprintTimeUpgradeButtonPressed);

        bool canUpgradeSprintReloadSpeed = saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed), false);
        sprintReloadSpeedUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetPlayerSprintReloadSpeedValue(), "0.0"),
            Formatter.IdleValue(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed)),
            "Level:" + saveLoadDataManager.GetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed),
            canUpgradeSprintReloadSpeed
        );
        sprintReloadSpeedUpgrade.SetUpgradeAction(OnSprintReloadSpeedUpgradeButtonPressed);

        textCoinsAmount.text = Formatter.IdleValue(saveLoadDataManager.GetCoins());
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
            saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.knockbackChance),
            saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.criticalChance),
            saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.criticalFactor),
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
