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
    public RankingManager rankingManager;
    public TMP_Text textRankingTotalScore;
    public CanvasGroup claimx2Canvas;

    public GameObject ranking;

    public Button buttonClaimx2;
    public Button buttonRanking;
    public Button buttonTryAgain;
    public Button buttonUpgradePlayer;
    public Transform tutorialMask;

    public int totalStore;
    public void OnEnable()
    {
        Sequence gameOverSequence = DOTween.Sequence();
        gameOverSequence.AppendInterval(1);
        gameOverSequence.Append(canvasGroup.DOFade(1, 0.75f));
        gameOverSequence.AppendCallback(() => canvasGroup.interactable = true);
        gameOverSequence.AppendCallback(() => canvasGroup.blocksRaycasts = true);

        totalStore = (int)(LevelManager.Instance.dataManager.CurrentPointsMultiplied * LevelManager.Instance.dataManager.GetKillMultipler());

        textKilledOponents.text = $"{(int)(LevelManager.Instance.dataManager.CurrentPointsMultiplied * LevelManager.Instance.dataManager.GetKillMultipler() - LevelManager.Instance.dataManager.CurrentPointsMultiplied)}";
        textCollectedCoins.text = LevelManager.Instance.dataManager.CurrentPointsMultiplied.ToString();
        textTotalScore.text = $"{totalStore}";
        textRankingTotalScore.text = $"{totalStore}";
        LevelManager.Instance.dataManager.saveLoadDataManager.AddCoins(totalStore);
        LevelManager.Instance.dataManager.saveLoadDataManager.SetLevelTopScore(LevelManager.Instance.dataManager.LevelName, totalStore);
        rankingManager.CheckRanking();
        rankingManager.myScore = totalStore;
        CheckWeaponUpgraded();
    }

    public void CheckWeaponUpgraded()
    {
        if (!PlayerPrefs.HasKey("WeaponUpgraded"))
        {
            tutorialMask.gameObject.SetActive(true);
            buttonClaimx2.interactable = false;
            buttonRanking.interactable = false;
            buttonTryAgain.interactable = false;
            buttonUpgradePlayer.interactable = true;
            buttonUpgradePlayer.transform.SetParent(tutorialMask);
            tutorialMask.GetComponent<Image>().DOColor(new Vector4(0,0,0,0.8f), 1);

            buttonClaimx2.GetComponent<Image>().color = new Vector4(1, 1, 1, 0.2f);
            buttonRanking.GetComponent<Image>().color = new Vector4(1, 1, 1, 0.2f);
            buttonTryAgain.GetComponent<Image>().color = new Vector4(1, 1, 1, 0.2f);



            var sequence = DOTween.Sequence();
            sequence.Append(buttonUpgradePlayer.transform.DOScale(Vector3.one * 1.1f, 0.5f));
            sequence.Append(buttonUpgradePlayer.transform.DOScale(Vector3.one, 0.5f));
            sequence.SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void OnShopButton()
    {
        PlayerPrefs.SetString("WeaponUpgraded", "Yes");
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
        LevelManager.Instance.dataManager.saveLoadDataManager.SetLevelTopScore(LevelManager.Instance.dataManager.LevelName, totalStore * 2);
        totalStore *= 2;
        textTotalScore.text = $"{totalStore}";
        claimx2Canvas.interactable = false;
        claimx2Canvas.alpha = 0.3f;
        claimx2Canvas.blocksRaycasts = false;
        rankingManager.CheckRanking();
    }

    public void OnRankingButton()
    {
        ranking.SetActive(true);
    }
    public void CloseRanking()
    {
        ranking.SetActive(false);
    }
}
