using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject ChooseLevel;
    public GameObject Options;
    public GameObject Shop;
    public AudioClip backToMenuAudioClip;
    public AudioClip choseLevelAudioClip;
    public AudioClip optionsAudioClip;
    public AudioClip storeAudioClip;
    public AudioClip changeSettings;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ShopController shopController;
  public List<string> qualityName = new List<string>();
    public int currentQualitySettings;
    public TMP_Text textQuality;


    public void OnSliderChanged()
    {
        audioSource.PlayOneShot(changeSettings);
    }

    public void OnQualityButtonPressed()
    {
        audioSource.PlayOneShot(changeSettings);
        if (currentQualitySettings < 2)
        {
            currentQualitySettings++;
        }
        else
        {
            currentQualitySettings = 0;
        }
        textQuality.text = qualityName[currentQualitySettings];
    }

    public void OpenMainMenu()
    {
        MainMenu.SetActive(true);
        ChooseLevel.SetActive(false);
        Options.SetActive(false);
        Shop.SetActive(false);
        audioSource.PlayOneShot(backToMenuAudioClip);
    }

    public void OpenOptions()
    {
        MainMenu.SetActive(false);
        ChooseLevel.SetActive(false);
        Options.SetActive(true);
        Shop.SetActive(false);
        audioSource.PlayOneShot(optionsAudioClip);
    }

    public void OpenChooseLevel()
    {
        MainMenu.SetActive(false);
        ChooseLevel.SetActive(true);
        Options.SetActive(false);
        Shop.SetActive(false);
        audioSource.PlayOneShot(choseLevelAudioClip);
    }

    public void OpenShop()
    {
        MainMenu.SetActive(false);
        ChooseLevel.SetActive(false);
        Options.SetActive(false);
        Shop.SetActive(true);
        shopController.UpdateShopItemValues();
        audioSource.PlayOneShot(choseLevelAudioClip);
    }

    public void OpenLiblaryLevel()
    {
        SceneManager.LoadScene("01.Museum New");
        audioSource.PlayOneShot(choseLevelAudioClip);
    }
}
