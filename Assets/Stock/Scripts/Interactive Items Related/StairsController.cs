using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class StairsController : MonoBehaviour
{
    public Transform TriggerDown;
    public Transform TriggerUp;
    public float activeTime;
    public bool isActive;

    public void OnTriggerUpEnter(Transform objectToTween)
    {
        if (isActive)
        {
            objectToTween.DOMove(TriggerDown.position, 1);
            StartCoroutine(DeactiveTime(objectToTween.gameObject));
        }
           
    }
    public void OnTriggerDownEnter(Transform objectToTween)
    {
        if (isActive)
        {
            objectToTween.DOMove(TriggerUp.position, 1);
            StartCoroutine(DeactiveTime(objectToTween.gameObject));
         
        }
          
    }

    IEnumerator DeactiveTime(GameObject objectToTween)
    {
        if (objectToTween.GetComponent<PlayerController>())
        {
            objectToTween.GetComponent<PlayerController>().navMeshAgent.enabled = false;
            objectToTween.GetComponent<PlayerController>().enabled = false;
        }


        isActive = false;
        yield return new WaitForSeconds(1.01f);
        isActive = true;

        if (objectToTween.GetComponent<PlayerController>())
        {
            objectToTween.GetComponent<PlayerController>().navMeshAgent.enabled = true;
            objectToTween.GetComponent<PlayerController>().enabled = true;
        }
    }
}
