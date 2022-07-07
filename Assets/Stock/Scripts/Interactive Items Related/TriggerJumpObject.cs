using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerJumpObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (!other.GetComponent<PlayerController>().isInJumpState)
            {
                other.GetComponent<PlayerController>().OnJumpObstacleHit();
            }

            else
            {

            }

        }
    }
}
