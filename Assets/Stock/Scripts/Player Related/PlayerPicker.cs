using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerPicker : MonoBehaviour
{
    public GameObject GameOverCanvas;
    public TMP_Text counterText;
    public Transform pickerPoint;
    public AudioClip pickupSound;
    public AudioSource audioSource;
    public int points;
    public PlayerController playerController;
    public void OnTriggerEnter(Collider other)
    {
     

        if (other.GetComponent<PickablePoint>())
        {
            points++;
            audioSource.PlayOneShot(pickupSound);
            other.transform.DOMove(pickerPoint.position, 0.2f);
            other.transform.DOScale(Vector3.zero, 0.2f);
            Destroy(other.gameObject, 0.2f);
            counterText.text = $"{points}/470";
        }

        if(other.GetComponent<EnemyController>())
        {
            GameOverCanvas.gameObject.SetActive(true);
            playerController.StopAllCoroutines();
            playerController.enabled = false;
        }

        if (other.GetComponent<EndSceneController>())
        {
            other.GetComponent<EndSceneController>().EndScene();
        }

    }
}
