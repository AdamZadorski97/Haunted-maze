using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class GameOverCanvas : MonoBehaviour
{
    public InterstitialAd interstitial;

    public Text textCollectedCoins;
    public Text textTotalScore;
    public Text textKilledOponents;
    public void OnEnable()
    {

        textKilledOponents.text = LevelManager.Instance.dataManager.CurrentKilledUnits.ToString();
        textCollectedCoins.text = LevelManager.Instance.dataManager.AllCollectedPoints.ToString();
        textTotalScore.text = $"{(int)(LevelManager.Instance.dataManager.CurrentPointsMultiplied * LevelManager.Instance.dataManager.GetKillMultipler())}";
      //  interstitial.ShowAd();
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
}
