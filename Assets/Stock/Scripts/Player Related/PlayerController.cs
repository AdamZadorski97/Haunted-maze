using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;
public class PlayerController : MonoSingleton<PlayerController>
{
    public static PlayerController _Instance { get; private set; }
    [SerializeField] private bool canMove = false;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnBackSpeed;
    [SerializeField] private AnimationCurve turnBackCurve;
    [SerializeField] private float turnSideSpeed;
    [SerializeField] private AnimationCurve turnSideCurve;
    [SerializeField] private float rotationSpeed;


    [SerializeField] private float canTurnTimer;
    [SerializeField] private float canTurnMaxTime;

    [SerializeField] private float footStepInterval;

    [SerializeField] private AudioClip footstep1;
    [SerializeField] private AudioClip footstep2;
    [SerializeField] private AudioClip turnAround;
    [SerializeField] private AudioClip shootSound;

    [SerializeField] private float minDistanceToTurn = 0.3f;
    [SerializeField] private float raycastOffset;
    [SerializeField] private SwipeController swipeController;
    [SerializeField] private LayerMask floorLayermask;
    [SerializeField] private LayerMask wallLayermask;
    [SerializeField] private LayerMask enemyLayermask;

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private ParticleSystem gunParticleSystem;
    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [HideInInspector] public bool moveLeft;
    [HideInInspector] public bool moveRight;
    [HideInInspector] public bool moveBack;
    private string inst = null;
    public void Update()
    {
        SwipeControll();
        if (canMove)
            MoveController();

    }



    public void Shoot()
    {
        cinemachineImpulseSource.GenerateImpulse();
        gunParticleSystem.Play();
        gunAnimator.SetTrigger("Shoot"+ Random.Range(1,4));
        audioSource.PlayOneShot(shootSound);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1000, enemyLayermask))
        {
            if (hit.transform.GetComponent<EnemyController>())
            {
                hit.transform.GetComponent<EnemyController>().OnDie();
            }
        }

        Debug.Log("Miss Shot");
    }



    public void SwipeControll()
    {
        if (swipeController.SwipeLeft)
        {
            Debug.Log("SwipeLeft");
            StopCoroutine(inst);
            inst = "Left";
            StartCoroutine(RememberLastSwipe("Left"));
        }

        if (swipeController.SwipeRight)
        {
            Debug.Log("SwipeRight");
            StopCoroutine(inst);
            inst = "Right";
            StartCoroutine(RememberLastSwipe("Right"));
        }

        if (swipeController.SwipeUp)
        {
            Debug.Log("SwipeUp");
            StopCoroutine(inst);
            inst = "Up";
            StartCoroutine(RememberLastSwipe("Up"));
        }

        if (swipeController.SwipeDown)
        {
            Debug.Log("SwipeDown");
            StopCoroutine(inst);
            inst = "Down";
            StartCoroutine(RememberLastSwipe("Down"));
        }

        if (swipeController.Tap)
        {
            StopCoroutine(inst);
            swipeController.tap = false;
            Shoot();
        }

    }
    IEnumerator RememberLastSwipe(string direction)
    {
        moveLeft = false;
        moveRight = false;
        moveBack = false;
        Debug.Log("Swipe Direction " + direction);
        if (direction == "Left")
        {
            moveLeft = true;
        }
        if (direction == "Right")
        {
            moveRight = true;
        }
        if (direction == "Up")
        {

        }
        if (direction == "Down")
        {
            moveBack = true;
        }
        yield return new WaitForSeconds(1);
    }




    private void Start()
    {
        SnapToGround();
        StartCoroutine(Footstep());
    }
    public void StopPlaySound()
    {
        StopAllCoroutines();
    }
    IEnumerator Footstep()
    {
        yield return new WaitForSeconds(footStepInterval);
        audioSource.PlayOneShot(footstep1);
        yield return new WaitForSeconds(footStepInterval);
        audioSource.PlayOneShot(footstep2);
        StartCoroutine(Footstep());
    }

    public bool CanTurn()
    {
        canTurnTimer += Time.deltaTime;
        if (canTurnTimer > canTurnMaxTime)
            return true;
        else
            return false;

    }

    public void SnapToGround()
    {
        RaycastHit groundHit;
        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(Vector3.down), out groundHit, Mathf.Infinity, floorLayermask))
        {
            transform.position = groundHit.transform.position + new Vector3(groundHit.transform.GetComponent<BoxCollider>().center.x, navMeshAgent.baseOffset, groundHit.transform.GetComponent<BoxCollider>().center.z);


            navMeshAgent.enabled = true;
            canMove = true;
        }
    }

    public void SwitchMovement(bool state)
    {
        canMove = state;
        navMeshAgent.enabled = state;
    }



    public bool wallRaycast()
    {
        if (GetHitPointDistance(frontMiddleRaycastStarPoint, frontMiddleRaycastStarPoint.TransformDirection(Vector3.forward), 75, Color.blue) < minDistanceToTurn)
        {
            return true;
        }


        return false;
    }




    public Transform frontMiddleRaycastStarPoint;
    public Transform frontLeftRaycastStarPoint;
    public Transform frontRightRaycastStarPoint;
    public Transform backLeftRaycastStarPoint;
    public Transform backRightRaycastStarPoint;

    public void MoveController()
    {
        if (CanTurn())
        {
            if (moveLeft && CheckCanTurnLeft())
            {
                moveLeft = false;
                moveRight = false;
                moveBack = false;
                Debug.Log("can turn Left");
                canTurnTimer = 0.2f;
                var sequence = DOTween.Sequence();
                sequence.AppendCallback(() => canMove = false);
                sequence.Append(transform.DOLocalRotate(new Vector3(0, -90, 0), turnSideSpeed, RotateMode.LocalAxisAdd).SetEase(turnSideCurve));
                sequence.AppendCallback(() => canMove = true);
                audioSource.PlayOneShot(turnAround);
                return;
            }
            if (moveRight && CheckCanTurnRight())
            {
                moveLeft = false;
                moveRight = false;
                moveBack = false;
                Debug.Log("can turn Left");
                canTurnTimer = 0.2f;
                var sequence = DOTween.Sequence();
                sequence.AppendCallback(() => canMove = false);
                sequence.Append(transform.DOLocalRotate(new Vector3(0, 90, 0), turnSideSpeed, RotateMode.LocalAxisAdd).SetEase(turnSideCurve));
                sequence.AppendCallback(() => canMove = true);
                audioSource.PlayOneShot(turnAround);
                return;
            }

            if (moveBack)
            {
                //  if (!CheckCanTurnLeft() && !CheckCanTurnRight())
                {
                    moveLeft = false;
                    moveRight = false;
                    moveBack = false;
                    Debug.Log("can turn Back");
                    canTurnTimer = 0.2f;
                    var sequence = DOTween.Sequence();
                    sequence.AppendCallback(() => canMove = false);
                    sequence.Append(transform.DOLocalRotate(new Vector3(0, 180, 0), turnBackSpeed, RotateMode.LocalAxisAdd).SetEase(turnBackCurve));
                    sequence.AppendCallback(() => canMove = true);
                    audioSource.PlayOneShot(turnAround);
                    return;
                }
            }

        }
        if (!wallRaycast())
            navMeshAgent.Move(transform.forward * moveSpeed * Time.deltaTime);
    }


    private bool CheckCanTurnLeft()
    {
        if (GetHitPointDistance(frontLeftRaycastStarPoint, frontLeftRaycastStarPoint.TransformDirection(Vector3.left), 50, Color.red) > 2
            &&
            GetHitPointDistance(backLeftRaycastStarPoint, backLeftRaycastStarPoint.TransformDirection(Vector3.left), 50, Color.red) > 2)
        {

            return true;
        }
        return false;
    }

    private bool CheckCanTurnRight()
    {
        if (GetHitPointDistance(frontRightRaycastStarPoint, frontRightRaycastStarPoint.TransformDirection(Vector3.right), 50, Color.green) > 2
             &&
             GetHitPointDistance(backRightRaycastStarPoint, backRightRaycastStarPoint.TransformDirection(Vector3.right), 50, Color.green) > 2)
        {
            return true;
        }
        return false;
    }




    public float GetHitPointDistance(Transform startPosition, Vector3 direction, float rayLenght, Color raycastColor)
    {
        Debug.DrawRay(startPosition.position, direction * rayLenght, raycastColor);

        RaycastHit hit;
        if (Physics.Raycast(startPosition.position, direction, out hit, Mathf.Infinity, wallLayermask))
        {
            return hit.distance;
        }
        else
        {
            return 100;
        }
    }


    public void SwitchPlayerCameraFalse()
    {
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0;
    }
    public void SwitchPlayerCameraTrue()
    {
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 1;
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 1;
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0;
    }
    public void SwitchNavMeshAgent(bool state)
    {
        navMeshAgent.enabled = state;
    }
}
