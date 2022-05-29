using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsTrigger : MonoBehaviour
{
    public bool isDownTrigger;
    public StairsController stairsController;
    public int destinationFloor;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerPicker>() && stairsController.isActive)
        {
            SwitchFloor();
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

    public void SwitchFloor()
    {
        for (int i = 0; i < 5; i++)
        {
            LevelManager.Instance.HideFloor(i);
        }
       LevelManager.Instance.ShowFloor(destinationFloor);
    }
}
