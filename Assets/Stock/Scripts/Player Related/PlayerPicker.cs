using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerPicker : MonoBehaviour
{
    public GameObject GameOverCanvas;
    public Transform pickerPoint;
    public AudioClip pickupSound;
    public AudioClip ghostSound;
    public AudioClip endGameSound;
    public AudioSource audioSource;
    public int points;
    public PlayerController playerController;
    public float pointLightIntencity;
    public float pointLightIntencityTime;
    public AnimationCurve pointLightIntencityCurve;
    [SerializeField] private Light moneyPointLight;
    [SerializeField] private UIManager uIManager;
    public void OnTriggerEnter(Collider other)
    {
     

        if (other.GetComponent<PickablePoint>())
        {
            other.GetComponent<PickablePoint>().OnInteractivePointPickup();
            audioSource.PlayOneShot(pickupSound, 0.5f);
            LevelManager.Instance.dataManager.SetPoint();
            Sequence pointSequence = DOTween.Sequence();
            pointSequence.Append(moneyPointLight.DOIntensity(pointLightIntencity, pointLightIntencityTime).SetEase(pointLightIntencityCurve));
            pointSequence.Append(moneyPointLight.DOIntensity(0, pointLightIntencityTime).SetEase(pointLightIntencityCurve));
        }

        if(other.GetComponent<EnemyController>())
        {
            LevelManager.Instance.dataManager.saveLoadDataManager.AddCoins((int)(LevelManager.Instance.dataManager.CurrentPointsMultiplied * LevelManager.Instance.dataManager.GetKillMultipler()));
            GameOverCanvas.gameObject.SetActive(true);
            audioSource.PlayOneShot(ghostSound, 0.5f);
            audioSource.PlayOneShot(endGameSound, 0.5f);
          //  uIManager.bottomPanel.DOFade(0, 0.5f);
           // uIManager.topPanel.DOFade(0, 0.5f);

            uIManager.bottomPanel.gameObject.SetActive(false);
            uIManager.topPanel.gameObject.SetActive(false);


            playerController.StopAllCoroutines();
            playerController.enabled = false;
            playerController.cinemachineVirtualCamera.LookAt = other.GetComponent<EnemyController>().head;
        }

        if (other.GetComponent<EndSceneController>())
        {
            other.GetComponent<EndSceneController>().EndScene();
        }
        if (other.GetComponent<HintTriggerController>())
        {
            other.GetComponent<HintTriggerController>().OnHintTriggerEnter();
        }
    }
}
