using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public GameObject player;
    public CinemachineBrain cinemachineBrain;


    public void TurnOnPlayer()
    {
        player.SetActive(true);
        cinemachineBrain.enabled = true;
    }
}
