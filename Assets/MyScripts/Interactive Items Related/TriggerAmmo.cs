using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAmmo : MonoBehaviour
{
    public GameObject model;
    public BoxCollider boxCollider;
    public float respawnTime;
    public AudioClip pickupSound;
    public AudioSource audioSource;
    public void OnPickup()
    {
        audioSource.PlayOneShot(pickupSound);
        boxCollider.enabled = false;
        model.SetActive(false);
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnTime);
        boxCollider.enabled = true;
        model.SetActive(true);
    }
}
