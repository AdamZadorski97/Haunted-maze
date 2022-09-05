using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerEnemyTrigger : MonoBehaviour
{
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private AudioClip ghostSound;
    [SerializeField] private AudioClip zombieDieScreamOnSurviwe;
    [SerializeField] private AudioClip endGameSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIManager uIManager;


    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyController>())
        {
            if(playerController.currentHp>1)
            {
                playerController.ShootEffect();
                other.GetComponent<EnemyController>().OnDie();
                playerController.currentHp--;
                LevelManager.Instance.uIManager.UpdateHP();
                audioSource.PlayOneShot(zombieDieScreamOnSurviwe, 0.5f);
            }
            else
            {

                gameOverCanvas.gameObject.SetActive(true);
                audioSource.PlayOneShot(ghostSound, 0.5f);
                audioSource.PlayOneShot(endGameSound, 0.5f);
                uIManager.bottomPanel.gameObject.SetActive(false);
                uIManager.topPanel.gameObject.SetActive(false);
                playerController.StopAllCoroutines();
                playerController.enabled = false;
                playerController.cameraPivot.DOLocalMoveY(0.3f, 0.5f);
                playerController.cinemachineVirtualCamera.LookAt = other.GetComponent<EnemyController>().head;
                other.GetComponent<EnemyController>().animator.SetTrigger("Attack");
                other.GetComponent<EnemyController>().navMeshAgent.enabled = false;
                Vector3 targetPostition = new Vector3(transform.position.x,
                                           other.transform.position.y,
                                           transform.position.z);
                other.transform.LookAt(targetPostition);
            }
        }

        if (other.GetComponent<TriggerSlimeBall>())
        {
            LevelManager.Instance.uIManager.SlimeVfx();
            playerController.moveSpeed *= 0.1f;
        }
    }
}
