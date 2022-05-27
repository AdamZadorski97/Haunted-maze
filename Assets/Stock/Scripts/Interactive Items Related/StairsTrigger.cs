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
            HideFloor(i);
        }
        ShowFloor(destinationFloor);
    }
    private void ShowFloor(int number)
    {
        NewWallController[] components = GameObject.FindObjectsOfType<NewWallController>();
        foreach (var item in components)
        {
            if (item.transform.position.y == number * 3)
            {
                foreach (GameObject mapLine in item.mapLines)
                {
                        mapLine.SetActive(true);
                }
            }

        }
    }
    private void HideFloor(int number)
    {
        NewWallController[] components = GameObject.FindObjectsOfType<NewWallController>();
        foreach (var item in components)
        {
            if (item.transform.position.y == number * 3)
            {
                foreach (GameObject mapLine in item.mapLines)
                {
                    mapLine.SetActive(false);
                }
            }

        }
    }
}
