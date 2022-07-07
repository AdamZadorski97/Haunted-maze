using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSlideObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>())
        {
            if (!other.GetComponent<PlayerController>().isInSlideState)
            {
                other.GetComponent<PlayerController>().OnSlideObstacleHit();
            }
           
            else
            {

            }
          
        }
    }
}
