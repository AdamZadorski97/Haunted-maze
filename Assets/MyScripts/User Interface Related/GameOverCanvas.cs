using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;
public class GameOverCanvas : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public InterstitialAd interstitial;
    public Text textCollectedCoins;
    public Text textTotalScore;
    public Text textKilledOponents;
    public Action rewardedAdAction;

    public CanvasGroup claimx2Canvas;

    public int totalStore;
    public void OnEnable()
    {
        canvasGroup.DOFade(1, 0.75f);
        totalStore = (int)(LevelManager.Instance.dataManager.CurrentPointsMultiplied * LevelManager.Instance.dataManager.GetKillMultipler());

        textKilledOponents.text = LevelManager.Instance.dataManager.CurrentKilledUnits.ToString();
        textCollectedCoins.text = LevelManager.Instance.dataManager.AllCollectedPoints.ToString();
        textTotalScore.text = $"{totalStore}";
        LevelManager.Instance.dataManager.saveLoadDataManager.AddCoins(totalStore);
    }

    public void OnShopButton()
    {
        PlayerPrefs.SetString("ShouldOpenShop", "Yes");
        SceneManager.LoadScene(0);
    }

    public void OnQuitButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene("01.Museum New");
    }

    public void OnAdsButton()
    {
        interstitial.ShowAd(OnRewarded);
    }
    public void OnRewarded()
    {
        LevelManager.Instance.dataManager.saveLoadDataManager.AddCoins(totalStore);
        totalStore *= 2;
        textTotalScore.text = $"{totalStore}";
        claimx2Canvas.interactable = false;
        claimx2Canvas.alpha = 0.3f;
        claimx2Canvas.blocksRaycasts = false;
    }
}
