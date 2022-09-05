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
    public GameObject ShopPlayer;
    public GameObject LoginPanel;
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
    public TMP_InputField inputTextNickName;
    public SaveLoadDataManager saveLoadDataManager;

    public GameObject weaponsParrent;
    public GameObject charactersParrent;

    private void Start()
    {

        Debug.Log(Formatter.IdleValue(100000000000));


        CheckNickname();
        if (PlayerPrefs.HasKey("ShouldOpenShop"))
        {
            if (PlayerPrefs.GetString("ShouldOpenShop") == "Yes")
            {
                OpenShop();
                PlayerPrefs.SetString("ShouldOpenShop", "No");
            }
        }
    }
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
        saveLoadDataManager.SetQualitySettings(currentQualitySettings);
    }

    public void OpenMainMenu()
    {
        MainMenu.SetActive(true);
        ChooseLevel.SetActive(false);
        Options.SetActive(false);
        Shop.SetActive(false);
        ShopPlayer.SetActive(false);
        audioSource.PlayOneShot(backToMenuAudioClip);
    }

    public void OpenOptions()
    {
        currentQualitySettings = saveLoadDataManager.GetQualitySettings();
        textQuality.text = qualityName[currentQualitySettings];
        MainMenu.SetActive(false);
        ChooseLevel.SetActive(false);
        Options.SetActive(true);
        Shop.SetActive(false);
        ShopPlayer.SetActive(false);
        audioSource.PlayOneShot(optionsAudioClip);
    }

    public void OpenChooseLevel()
    {
        MainMenu.SetActive(false);
        ChooseLevel.SetActive(true);
        Options.SetActive(false);
        Shop.SetActive(false);
        ShopPlayer.SetActive(false);
        audioSource.PlayOneShot(choseLevelAudioClip);
    }

    public void OpenShopPlayer()
    {
        weaponsParrent.SetActive(false);
        charactersParrent.SetActive(true);
        MainMenu.SetActive(false);
        ChooseLevel.SetActive(false);
        Options.SetActive(false);
        Shop.SetActive(false);
        ShopPlayer.SetActive(true);
        shopController.UpdateShopItemValues();
        audioSource.PlayOneShot(choseLevelAudioClip);
    }

    public void OpenShop()
    {
        weaponsParrent.SetActive(true);
        charactersParrent.SetActive(false);
        MainMenu.SetActive(false);
        ChooseLevel.SetActive(false);
        Options.SetActive(false);
        Shop.SetActive(true);
        ShopPlayer.SetActive(false);
        shopController.UpdateShopItemValues();
        audioSource.PlayOneShot(choseLevelAudioClip);
    }

    public void OpenLiblaryLevel()
    {
        SceneManager.LoadScene("01.Museum New");
        audioSource.PlayOneShot(choseLevelAudioClip);
    }

    public void SetupNickname()
    {
        if (inputTextNickName.text != "")
        {
            PlayerPrefs.SetString("NickName", inputTextNickName.text);
            saveLoadDataManager.StartCoroutine(saveLoadDataManager.CheckPlayerExist(output => { }));
            LoginPanel.SetActive(false);
        }
    }

    public void CheckNickname()
    {
        if (PlayerPrefs.HasKey("NickName"))
        {
            saveLoadDataManager.StartCoroutine(saveLoadDataManager.CheckPlayerExist(output => { }));
            return;
        }
        else
        {
            LoginPanel.SetActive(true);
        }
    }
}
