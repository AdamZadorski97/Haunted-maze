using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class StairsController : MonoBehaviour
{
    public AnimationCurve moveDownSpeedCurve;
    public AnimationCurve moveUpSpeedCurve;
    public float timeToReachDestination = 2;
    public Transform TriggerDown;
    public Transform TriggerUp;
    public bool isActive;
    public float timeToActivateTrigger = 0.1f;
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
            SwitchPlayerCameraFalse();
        }


        isActive = false;
        yield return new WaitForSeconds(timeToReachDestination+ timeToActivateTrigger);
        isActive = true;

        if (objectToTween.GetComponent<PlayerController>())
        {
            objectToTween.GetComponent<PlayerController>().navMeshAgent.enabled = true;
            objectToTween.GetComponent<PlayerController>().enabled = true;
            SwitchPlayerCameraTrue();
        }
    }




    public void SwitchPlayerCameraFalse()
    {
        PlayerController.Instance.cinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
        PlayerController.Instance.cinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
        PlayerController.Instance.cinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0;
    }
    public void SwitchPlayerCameraTrue()
    {
        PlayerController.Instance.cinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 1;
        PlayerController.Instance.cinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 1;
        PlayerController.Instance.cinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0;
    }
}
