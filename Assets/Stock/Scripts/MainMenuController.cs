using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject ChooseLevel;
    public AudioClip ButtonAudioClip;
    public AudioSource audioSource;

    public void OpenMainMenu()
    {
        MainMenu.SetActive(true);
        ChooseLevel.SetActive(false);
        audioSource.PlayOneShot(ButtonAudioClip);
    }
    public void OpenChooseLevel()
    {
        MainMenu.SetActive(false);
        ChooseLevel.SetActive(true);
        audioSource.PlayOneShot(ButtonAudioClip);
    }

    public void OpenTutorialLevel()
    {
        SceneManager.LoadScene("World_01_Level_01");
        audioSource.PlayOneShot(ButtonAudioClip);
    }

    public void OpenLiblaryLevel()
    {
        SceneManager.LoadScene("World_01_Level_02");
        audioSource.PlayOneShot(ButtonAudioClip);
    }
}
