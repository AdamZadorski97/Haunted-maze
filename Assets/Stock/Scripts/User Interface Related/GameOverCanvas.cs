using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOverCanvas : MonoBehaviour
{
    public InterstitialAd interstitial;
    public TMP_Text textScore;
    public void OnEnable()
    {
        textScore.text = $"{LevelManager.Instance.dataManager.GetCurrentPointsMultiplied()}";
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
