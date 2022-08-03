using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class ShopController : MonoBehaviour
{

    [SerializeField] private Transform CoinsPanel;
    [SerializeField] private TMP_Text textCoinsAmount;

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
            sequence.Append(CoinsPanel.DOScale(Vector3.one * 1.1f, 0.25f));
            sequence.Join(CoinsPanel.GetComponent<Image>().DOColor(Color.red, 0.25f));
            sequence.Append(CoinsPanel.DOScale(Vector3.one, 0.25f));
            sequence.Join(CoinsPanel.GetComponent<Image>().DOColor(Color.white, 0.25f));
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
            sequence.Append(CoinsPanel.DOScale(Vector3.one * 1.1f, 0.25f));
            sequence.Join(CoinsPanel.GetComponent<Image>().DOColor(Color.red, 0.25f));
            sequence.Append(CoinsPanel.DOScale(Vector3.one, 0.25f));
            sequence.Join(CoinsPanel.GetComponent<Image>().DOColor(Color.white, 0.25f));
            audioSource.PlayOneShot(audioClipNoUpgrade);
        }
    }

    public void OnReloadTimeUpgradeButtonPressed()
    {
        Debug.Log("reload Cost:" + saveLoadDataManager.GetWeaponUpgradeCost(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime));
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
            sequence.Append(CoinsPanel.DOScale(Vector3.one * 1.1f, 0.25f));
            sequence.Join(CoinsPanel.GetComponent<Image>().DOColor(Color.red, 0.25f));
            sequence.Append(CoinsPanel.DOScale(Vector3.one, 0.25f));
            sequence.Join(CoinsPanel.GetComponent<Image>().DOColor(Color.white, 0.25f));
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

        textDamageCurrentLevel.text = "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.damage).ToString();
        textClipCurrentLevel.text = "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.clip).ToString();
        textReloadCurrentLevel.text = "Level:" + saveLoadDataManager.GetWeaponUpgradeLevel(currentWeaponID, SaveLoadDataManager.weaponUpgradeType.reloadTime).ToString();

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


    }

    private void ParticlesBuyEffect()
    {
        Sequence particleSequence = DOTween.Sequence();
        particleSequence.AppendCallback(() => canvasParticleEmitter.EmiterRate = 20);
        particleSequence.AppendInterval(1);
        particleSequence.AppendCallback(() => canvasParticleEmitter.EmiterRate = 0);
    }
}
