using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
        }


        isActive = false;
        yield return new WaitForSeconds(timeToReachDestination+ timeToActivateTrigger);
        isActive = true;

        if (objectToTween.GetComponent<PlayerController>())
        {
            objectToTween.GetComponent<PlayerController>().navMeshAgent.enabled = true;
            objectToTween.GetComponent<PlayerController>().enabled = true;
        }
    }
}
