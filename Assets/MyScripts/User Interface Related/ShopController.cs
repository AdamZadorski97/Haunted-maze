using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class ShopController : MonoBehaviour
{
    #region weapon properties
    [SerializeField] private Transform CoinsPanelWeapon;
    [SerializeField] private TMP_Text textCoinsAmountWeapon;

    [SerializeField] private TMP_Text textDamageValue;
    [SerializeField] private TMP_Text textClipValue;
    [SerializeField] private TMP_Text textReloadTimeValue;

    [SerializeField] private TMP_Text textDamageUpgradeCost;
    [SerializeField] private TMP_Text textClipUpgradeCost;
    [SerializeField] private TMP_Text textReloadUpgradeCost;

    [SerializeField] private TMP_Text textDamageCurrentLevel;
    [SerializeField] private TMP_Text textClipCurrentLevel;
    [SerializeField] private TMP_Text textReloadCurrentLevel;

    [SerializeField] private Image imageButtonDamageUpgrade;
    [SerializeField] private Image imageButtonClipUpgrade;
    [SerializeField] private Image imageButtonReloadUpgrade;

    [SerializeField] private CanvasParticleEmitter canvasParticleEmitterButtonDamageUpgrade;
    [SerializeField] private CanvasParticleEmitter canvasParticleEmitterButtonClipUpgrade;
    [SerializeField] private CanvasParticleEmitter canvasParticleEmitterButtonReloadUpgrade;
    #endregion



    [SerializeField] private Transform CoinsPanelPlayer;
    [SerializeField] private TMP_Text textCoinsAmountPlayer;

    [SerializeField] private TMP_Text textHpValue;
    [SerializeField] private TMP_Text textSprintTimeValue;
    [SerializeField] private TMP_Text textSprintReloadSpeedValue;

    [SerializeField] private TMP_Text textHpUpgradeCost;
    [SerializeField] private TMP_Text textSprintTimeUpgradeCost;
    [SerializeField] private TMP_Text textSprintReloadSpeedUpgradeCost;

    [SerializeField] private TMP_Text textHpCurrentLevel;
    [SerializeField] private TMP_Text textSprintTimeCurrentLevel;
    [SerializeField] private TMP_Text textSprintReloadSpeedCurrentLevel;

    [SerializeField] private Image imageButtonHpUpgrade;
    [SerializeField] private Image imageButtonSprintTimeCurrentUpgrade;
    [SerializeField] private Image imageButtonSprintReloadSpeedCurrentUpgrade;

    [SerializeField] private CanvasParticleEmitter canvasParticleEmitterButtonHpUpgrade;
    [SerializeField] private CanvasParticleEmitter canvasParticleEmitterButtonSprintTimeUpgrade;
    [SerializeField] private CanvasParticleEmitter canvasParticleEmitterButtonReloadSpeedCurrentUpgrade;



    public List<double> pricesList = new List<double>();





    [SerializeField] private Sprite canUpdateSprite;
    [SerializeField] private Sprite cantUpdateSprite;

    [SerializeField] private SaveLoadDataManager saveLoadDataManager;
    [SerializeField] private int currentWeaponID;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClipUpgrade;
    [SerializeField] private AudioClip audioClipNoUpgrade;

    [SerializeField] private CanvasParticleEmitter canvasParticleEmitter;


    private void OnEnable()
    {
        UpdateShopItemValues();
    }

    public void OnHpUpgradeButtonPressed()
    {
        Debug.Log("reload Cost:" + saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.hp));
        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.hp), true))
        {

            saveLoadDataManager.SetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.hp);
            UpdateShopItemValues();
            audioSource.PlayOneShot(audioClipUpgrade);
            ParticlesBuyEffect();
        }
        else
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(CoinsPanelPlayer.DOScale(Vector3.one * 1.1f, 0.25f));
            sequence.Join(CoinsPanelPlayer.GetComponent<Image>().DOColor(Color.red, 0.25f));
            sequence.Append(CoinsPanelPlayer.DOScale(Vector3.one, 0.25f));
            sequence.Join(CoinsPanelPlayer.GetComponent<Image>().DOColor(Color.white, 0.25f));
            audioSource.PlayOneShot(audioClipNoUpgrade);
        }
    }

    public void OnSprintTimeUpgradeButtonPressed()
    {
        Debug.Log("reload Cost:" + saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintTime));
        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintTime), true))
        {

            saveLoadDataManager.SetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.sprintTime);
            UpdateShopItemValues();
            audioSource.PlayOneShot(audioClipUpgrade);
            ParticlesBuyEffect();
        }
        else
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(CoinsPanelPlayer.DOScale(Vector3.one * 1.1f, 0.25f));
            sequence.Join(CoinsPanelPlayer.GetComponent<Image>().DOColor(Color.red, 0.25f));
            sequence.Append(CoinsPanelPlayer.DOScale(Vector3.one, 0.25f));
            sequence.Join(CoinsPanelPlayer.GetComponent<Image>().DOColor(Color.white, 0.25f));
            audioSource.PlayOneShot(audioClipNoUpgrade);
        }
    }

    public void OnSprintReloadSpeedUpgradeButtonPressed()
    {
        Debug.Log("reload Cost:" + saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed));
        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed), true))
        {

            saveLoadDataManager.SetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed);
            UpdateShopItemValues();
            audioSource.PlayOneShot(audioClipUpgrade);
            ParticlesBuyEffect();
        }
        else
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(CoinsPanelPlayer.DOScale(Vector3.one * 1.1f, 0.25f));
            sequence.Join(CoinsPanelPlayer.GetComponent<Image>().DOColor(Color.red, 0.25f));
            sequence.Append(CoinsPanelPlayer.DOScale(Vector3.one, 0.25f));
            sequence.Join(CoinsPanelPlayer.GetComponent<Image>().DOColor(Color.white, 0.25f));
            audioSource.PlayOneShot(audioClipNoUpgrade);
        }
    }



    public void OnDamageUpgradeButtonPressed()
    {
        Debug.Log("reload Cost:" + saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage));
        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage), true))
        {
         
            saveLoadDataManager.SetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage);
            UpdateShopItemValues();
            audioSource.PlayOneShot(audioClipUpgrade);
            ParticlesBuyEffect();
        }
        else
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(CoinsPanelWeapon.DOScale(Vector3.one * 1.1f, 0.25f));
            sequence.Join(CoinsPanelWeapon.GetComponent<Image>().DOColor(Color.red, 0.25f));
            sequence.Append(CoinsPanelWeapon.DOScale(Vector3.one, 0.25f));
            sequence.Join(CoinsPanelWeapon.GetComponent<Image>().DOColor(Color.white, 0.25f));
            audioSource.PlayOneShot(audioClipNoUpgrade);
        }
    }



    public void OnClipUpgradeButtonPressed()
    {
        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip), true))
        {
          


            saveLoadDataManager.SetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip);
            UpdateShopItemValues();
            audioSource.PlayOneShot(audioClipUpgrade);
            ParticlesBuyEffect();
        }
        else
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(CoinsPanelWeapon.DOScale(Vector3.one * 1.1f, 0.25f));
            sequence.Join(CoinsPanelWeapon.GetComponent<Image>().DOColor(Color.red, 0.25f));
            sequence.Append(CoinsPanelWeapon.DOScale(Vector3.one, 0.25f));
            sequence.Join(CoinsPanelWeapon.GetComponent<Image>().DOColor(Color.white, 0.25f));
            audioSource.PlayOneShot(audioClipNoUpgrade);
        }
    }

    public void OnReloadTimeUpgradeButtonPressed()
    {
        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime), true))
        {
     
            saveLoadDataManager.SetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime);
            UpdateShopItemValues();
            audioSource.PlayOneShot(audioClipUpgrade);
            ParticlesBuyEffect();
        }
        else
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(CoinsPanelWeapon.DOScale(Vector3.one * 1.1f, 0.25f));
            sequence.Join(CoinsPanelWeapon.GetComponent<Image>().DOColor(Color.red, 0.25f));
            sequence.Append(CoinsPanelWeapon.DOScale(Vector3.one, 0.25f));
            sequence.Join(CoinsPanelWeapon.GetComponent<Image>().DOColor(Color.white, 0.25f));
            audioSource.PlayOneShot(audioClipNoUpgrade);
        }
    }

    public void UpdateShopItemValues()
    {
        textDamageValue.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponDamageValue(currentWeaponID), "0.0");
        textDamageUpgradeCost.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage));
        textClipValue.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponClipValue(currentWeaponID));
        textClipUpgradeCost.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip));
        textReloadTimeValue.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponRealoadTime(currentWeaponID), "0.0")+"s";
        textReloadUpgradeCost.text = Formatter.IdleValue(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime));
        textCoinsAmountWeapon.text = Formatter.IdleValue(saveLoadDataManager.GetCoins());


        textDamageCurrentLevel.text = "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage).ToString();
        textClipCurrentLevel.text = "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip).ToString();
        textReloadCurrentLevel.text = "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime).ToString();


        textHpValue.text = Formatter.IdleValue(saveLoadDataManager.GetPlayerHpValue(), "0");
        textHpUpgradeCost.text = Formatter.IdleValue(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.hp));

        textSprintTimeValue.text = saveLoadDataManager.GetPlayerSprintTimeValue().ToString()+"s";
        textSprintTimeUpgradeCost.text = Formatter.IdleValue(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintTime));

        textSprintReloadSpeedValue.text = Formatter.IdleValue(saveLoadDataManager.GetPlayerSprintReloadSpeedValue(),"0.0");
        textSprintReloadSpeedUpgradeCost.text = Formatter.IdleValue(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed));
        textCoinsAmountPlayer.text = Formatter.IdleValue(saveLoadDataManager.GetCoins());

        textHpCurrentLevel.text = "Level:" + saveLoadDataManager.GetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.hp).ToString();
        textSprintTimeCurrentLevel.text = "Level:" + saveLoadDataManager.GetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.sprintTime).ToString();
        textSprintReloadSpeedCurrentLevel.text = "Level:" + saveLoadDataManager.GetPlayerUpgradeLevel(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed).ToString();




        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip)))
        {
            canvasParticleEmitterButtonClipUpgrade.canvasRenderer.SetColor(Color.green);
            imageButtonClipUpgrade.sprite = canUpdateSprite;
        }
        else
        {
            canvasParticleEmitterButtonClipUpgrade.canvasRenderer.SetColor(Color.red);
            imageButtonClipUpgrade.sprite = cantUpdateSprite;
        }

        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage)))
        {
            canvasParticleEmitterButtonDamageUpgrade.canvasRenderer.SetColor(Color.green);
            imageButtonDamageUpgrade.sprite = canUpdateSprite;
        }
        else
        {
            canvasParticleEmitterButtonDamageUpgrade.canvasRenderer.SetColor(Color.red);
            imageButtonDamageUpgrade.sprite = cantUpdateSprite;
        }


        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime)))
        {
            canvasParticleEmitterButtonReloadUpgrade.canvasRenderer.SetColor(Color.green);
            imageButtonReloadUpgrade.sprite = canUpdateSprite;
        }
        else
        {
            canvasParticleEmitterButtonReloadUpgrade.canvasRenderer.SetColor(Color.red);
            imageButtonReloadUpgrade.sprite = cantUpdateSprite;
        }

        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.hp)))
        {
            canvasParticleEmitterButtonHpUpgrade.canvasRenderer.SetColor(Color.green);
            imageButtonHpUpgrade.sprite = canUpdateSprite;
        }
        else
        {
            canvasParticleEmitterButtonHpUpgrade.canvasRenderer.SetColor(Color.red);
            imageButtonHpUpgrade.sprite = cantUpdateSprite;
        }

        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintTime)))
        {
            canvasParticleEmitterButtonSprintTimeUpgrade.canvasRenderer.SetColor(Color.green);
            imageButtonSprintTimeCurrentUpgrade.sprite = canUpdateSprite;
        }
        else
        {
            canvasParticleEmitterButtonSprintTimeUpgrade.canvasRenderer.SetColor(Color.red);
            imageButtonSprintTimeCurrentUpgrade.sprite = cantUpdateSprite;
        }

        if (saveLoadDataManager.CheckEnoughCoins(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed)))
        {
            canvasParticleEmitterButtonReloadSpeedCurrentUpgrade.canvasRenderer.SetColor(Color.green);
            imageButtonSprintReloadSpeedCurrentUpgrade.sprite = canUpdateSprite;
        }
        else
        {
            canvasParticleEmitterButtonReloadSpeedCurrentUpgrade.canvasRenderer.SetColor(Color.red);
            imageButtonSprintReloadSpeedCurrentUpgrade.sprite = cantUpdateSprite;
        }
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
        pricesList = new List<double>();
        pricesList.Add(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage));
        pricesList.Add(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip));
        pricesList.Add(saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime));
        pricesList.Add(saveLoadDataManager.GetPlayerUpgradeCost( SaveLoadDataManager.playerUpgradeType.hp));
        pricesList.Add(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintTime));
        pricesList.Add(saveLoadDataManager.GetPlayerUpgradeCost(SaveLoadDataManager.playerUpgradeType.sprintReloadSpeed));
        pricesList.Sort((x, y) => x.CompareTo(y));
    }
}
