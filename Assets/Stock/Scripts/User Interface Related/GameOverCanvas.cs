using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverCanvas : MonoBehaviour
{
    public InterstitialAd interstitial;

    public void OnEnable()
    {
        interstitial.ShowAd();
    }
    public void OnQuitButton()
    {
        Application.Quit();
    }
    public void OnRestartButton()
    {

        SceneManager.LoadScene(0);
    }
}
