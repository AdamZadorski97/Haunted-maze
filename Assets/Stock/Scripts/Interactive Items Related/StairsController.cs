using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class StairsController : MonoBehaviour
{
    [SerializeField] private AnimationCurve moveDownSpeedCurve;
    [SerializeField] private AnimationCurve moveUpSpeedCurve;
    [SerializeField] private float timeToReachDestination = 2;
    [SerializeField] private float timeToActivateTrigger = 0.1f;
    [SerializeField] private Transform TriggerDown;
    [SerializeField] private Transform TriggerUp;
    public bool isActive;

    public void OnTriggerUpEnter(Transform objectToTween)
    {

        objectToTween.DOMove(TriggerDown.position, timeToReachDestination).SetEase(moveDownSpeedCurve);
        StartCoroutine(DeactiveTime(objectToTween.gameObject));
    }
    public void OnTriggerDownEnter(Transform objectToTween)
    {

        objectToTween.DOMove(TriggerUp.position, timeToReachDestination).SetEase(moveUpSpeedCurve);
        StartCoroutine(DeactiveTime(objectToTween.gameObject));
    }

    IEnumerator DeactiveTime(GameObject objectToTween)
    {
        if (objectToTween.GetComponent<PlayerController>())
        {
            objectToTween.GetComponent<PlayerController>().navMeshAgent.enabled = false;
            objectToTween.GetComponent<PlayerController>().enabled = false;
            PlayerController.Instance.SwitchPlayerCameraFalse();
        }


        isActive = false;
        yield return new WaitForSeconds(timeToReachDestination + timeToActivateTrigger);
        isActive = true;

        if (objectToTween.GetComponent<PlayerController>())
        {
            objectToTween.GetComponent<PlayerController>().navMeshAgent.enabled = true;
            objectToTween.GetComponent<PlayerController>().enabled = true;
            PlayerController.Instance.SwitchPlayerCameraTrue();
        }
    }





}
