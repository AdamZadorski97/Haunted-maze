using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsTrigger : MonoBehaviour
{
    public bool isDownTrigger;
    public StairsController stairsController;



    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<PlayerPicker>())
            if (isDownTrigger)
            {
                stairsController.OnTriggerDownEnter(other.transform.GetComponent<PlayerPicker>().playerController.transform);
            }

            else
            {
                stairsController.OnTriggerUpEnter(other.transform.GetComponent<PlayerPicker>().playerController.transform);
            }
    }
}
