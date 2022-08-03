using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerPicker : MonoBehaviour
{
    public Transform pickerPoint;
    public AudioClip pickupSound;
    public AudioSource audioSource;
    public int points;
    public PlayerController playerController;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private CoinPicker coinPicker;



    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EndSceneController>())
        {
            other.GetComponent<EndSceneController>().EndScene();
        }
        if (other.GetComponent<HintTriggerController>())
        {
            other.GetComponent<HintTriggerController>().OnHintTriggerEnter();
        }
        if (other.GetComponent<TriggerMagnes>())
        {
            other.GetComponent<TriggerMagnes>().OnPickup();
            Sequence magnesSequence = DOTween.Sequence();
            magnesSequence.AppendCallback(() => playerController.SwitchMagnesParticles(true));
            magnesSequence.AppendCallback(() => coinPicker.ChangeColliderSize(true));
            magnesSequence.AppendInterval(10);
            magnesSequence.AppendCallback(() => playerController.SwitchMagnesParticles(false));
            magnesSequence.AppendCallback(() => coinPicker.ChangeColliderSize(false));
        }

        if(other.GetComponent<TriggerAmmo>())
        {
            other.GetComponent<TriggerAmmo>().OnPickup();
            LevelManager.Instance.dataManager.AddAmmunition();
        }




    }
}
