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
    public AudioSource audioSource;
    public int points;
    public PlayerController playerController;
    public float pointLightIntencity;
    public float pointLightIntencityTime;
    public AnimationCurve pointLightIntencityCurve;
    [SerializeField] private Light moneyPointLight;
    public void OnTriggerEnter(Collider other)
    {
     

        if (other.GetComponent<PickablePoint>())
        {
            other.GetComponent<PickablePoint>().OnInteractivePointPickup();
            audioSource.PlayOneShot(pickupSound);
            LevelManager.Instance.dataManager.SetPoint();
            Sequence pointSequence = DOTween.Sequence();
            pointSequence.Append(moneyPointLight.DOIntensity(pointLightIntencity, pointLightIntencityTime).SetEase(pointLightIntencityCurve));
            pointSequence.Append(moneyPointLight.DOIntensity(0, pointLightIntencityTime).SetEase(pointLightIntencityCurve));
        }

        if(other.GetComponent<EnemyController>())
        {
            LevelManager.Instance.dataManager.saveLoadDataManager.AddCoins(LevelManager.Instance.dataManager.currentPointsMultiplied);
            GameOverCanvas.gameObject.SetActive(true);
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
