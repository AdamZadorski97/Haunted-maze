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
        damageUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetWeaponDamageValue(currentWeaponID), "0.0"),
            Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage)),
            "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage)
        );
        damageUpgrade.SetUpgradeAction(OnDamageUpgradeButtonPressed);  // Assign action to button

        clipUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetWeaponClipValue(currentWeaponID)),
            Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip)),
            "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip)
        );
        clipUpgrade.SetUpgradeAction(OnClipUpgradeButtonPressed);

        reloadTimeUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetWeaponRealoadTime(currentWeaponID), "0.0") + "s",
            Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime)),
            "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime)
        );
        reloadTimeUpgrade.SetUpgradeAction(OnReloadTimeUpgradeButtonPressed);

        shootSpeedUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetWeaponShootSpeedTime(currentWeaponID), "0.00") + "s",
            Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.shootSpeed)),
            "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.shootSpeed)
        );
        shootSpeedUpgrade.SetUpgradeAction(OnShootSpeedTimeUpgradeButtonPressed);

        knockbackUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetWeaponKnockbackValue(currentWeaponID), "0.00"),
            Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.knockback)),
            "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.knockback)
        );
        knockbackUpgrade.SetUpgradeAction(OnShootKnockbackUpgradeButtonPressed);

        knockbackChanceUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetWeaponKnockbackChanceValue(currentWeaponID), "0") + "%",
            Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.knockbackChance)),
            "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.knockbackChance)
        );
        knockbackChanceUpgrade.SetUpgradeAction(OnShootKnockbackChanceUpgradeButtonPressed);

        criticalChanceUpgrade.SetUpgradeValues(
     Formatter.IdleValue(saveLoadDataManager.GetWeaponCriticalChanceValue(currentWeaponID), "0") + "%",
     Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.criticalChance)),
     "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.criticalChance)
        );
        criticalChanceUpgrade.SetUpgradeAction(OnShootCriticalChanceUpgradeButtonPressed);

        textCoinsAmount.text = Formatter.IdleValue(saveLoadDataManager.GetCoins());
    }

    public void UpdatePlayerValues()
    {
        hpUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetPlayerHpValue(), "0"),
            Formatter.IdleValue(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.hp)),
            "Level:" + saveLoadDataManager.GetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.hp)
        );
        hpUpgrade.SetUpgradeAction(OnHpUpgradeButtonPressed);

        sprintTimeUpgrade.SetUpgradeValues(
            saveLoadDataManager.GetPlayerSprintTimeValue() + "s",
            Formatter.IdleValue(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintTime)),
            "Level:" + saveLoadDataManager.GetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.sprintTime)
        );
        sprintTimeUpgrade.SetUpgradeAction(OnSprintTimeUpgradeButtonPressed);

        sprintReloadSpeedUpgrade.SetUpgradeValues(
            Formatter.IdleValue(saveLoadDataManager.GetPlayerSprintReloadSpeedValue(), "0.0"),
            Formatter.IdleValue(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed)),
            "Level:" + saveLoadDataManager.GetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed)
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
