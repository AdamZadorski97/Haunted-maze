using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;
public class PlayerController : MonoSingleton<PlayerController>
{
    public static PlayerController _Instance { get; private set; }
    public bool canMove = false;
    public float speed;
    public float rotationSpeed;
    public NavMeshAgent navMeshAgent;
    public bool moveLeft;
    public bool moveRight;
    public bool moveBack;
    public float canTurnTimer;
    public float canTurnMaxTime;
    public float interval;
    public AudioSource audioSource;
    public AudioClip footstep1;
    public AudioClip footstep2;
    public AudioClip turnAround;
    public float minDistanceToTurn = 0.3f;
    public float raycastOffset;
    public LayerMask floorLayermask;
    public SwipeController swipeController;
    string inst = null;
    public LayerMask wallLayermask;
    public void Update()
    {
        SwipeControll();
        if(canMove)
        MoveController(); 
        
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
            inst = "Tap";
            StartCoroutine(RememberLastSwipe("Tap"));
        }
         
    }
    IEnumerator  RememberLastSwipe(string direction)
    {
        moveLeft = false;
        moveRight = false;
        moveBack = false;
        Debug.Log("Swipe Direction " + direction);
        if (direction == "Left")
        {
            moveLeft = true;
        }
        if(direction == "Right")
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
        yield return new WaitForSeconds(interval);
        audioSource.PlayOneShot(footstep1);
        yield return new WaitForSeconds(interval);
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
        if (Physics.Raycast(transform.position + new Vector3(0,1,0), transform.TransformDirection(Vector3.down), out groundHit, Mathf.Infinity, floorLayermask))
        {
            transform.position = groundHit.transform.position + new Vector3(groundHit.transform.GetComponent<BoxCollider>().center.x,navMeshAgent.baseOffset, groundHit.transform.GetComponent<BoxCollider>().center.z);
            
            
            navMeshAgent.enabled = true;
            canMove = true;
        }
    }

    public void SwitchMovement(bool state)
    {
        canMove = state;
        navMeshAgent.enabled = state;
    }



    public void Move()
    {
        if (CanTurn())
        {
            RaycastHit forwardHit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out forwardHit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * forwardHit.distance, Color.red);

                if (forwardHit.distance < minDistanceToTurn)
                {
                    RaycastHit RightRaycast;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out RightRaycast, Mathf.Infinity, wallLayermask)) { }

                    RaycastHit LeftRaycast;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out LeftRaycast, Mathf.Infinity, wallLayermask)) { }

                    if (LeftRaycast.distance < minDistanceToTurn && RightRaycast.distance < minDistanceToTurn)
                    {
                        canTurnTimer = 0.2f;
                        transform.Rotate(new Vector3(0, 180, 0), Space.World);
                        audioSource.PlayOneShot(turnAround);
                        return;
                    }

                    if (LeftRaycast.distance > 2f)
                    {
                        canTurnTimer = 0.2f;
                        audioSource.PlayOneShot(turnAround);
                        transform.Rotate(new Vector3(0, -90, 0), Space.World);
                        return;
                    }

                    if (RightRaycast.distance > 2f)
                    {
                        canTurnTimer = 0.2f;
                        audioSource.PlayOneShot(turnAround);
                        transform.Rotate(new Vector3(0, 90, 0), Space.World);
                        return;
                    }

                    return;
                }
            }
        }
        if (CanTurn())
        {
            RaycastHit hit1;

            if (Physics.Raycast(transform.position + transform.forward * (raycastOffset), transform.TransformDirection(Vector3.left), out hit1, Mathf.Infinity, wallLayermask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hit1.distance, Color.yellow);
                if (moveLeft)
                    if (hit1.distance > 2f)
                    {
                        canTurnTimer = 0;
                        var turnSequence = DOTween.Sequence();
                        turnSequence.AppendInterval(0.035f);
                        turnSequence.AppendCallback(() =>
                        {
                            audioSource.PlayOneShot(turnAround);
                            transform.Rotate(new Vector3(0, -90, 0), Space.World);
                            return;
                        });

                    }
            }


            RaycastHit hit2;

            if (Physics.Raycast(transform.position + transform.forward * (raycastOffset), transform.TransformDirection(Vector3.right), out hit2, Mathf.Infinity, wallLayermask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit2.distance, Color.yellow);
                if (moveRight)
                    if (hit2.distance > 2f)
                    {
                        canTurnTimer = 0;
                        var turnSequence = DOTween.Sequence();
                        turnSequence.AppendInterval(0.02f);
                        turnSequence.AppendCallback(() =>
                        {
                            audioSource.PlayOneShot(turnAround);
                            transform.Rotate(new Vector3(0, 90, 0), Space.World);
                            return;
                        });
                    }
            }

            RaycastHit hitBack;
            if (moveBack)
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hitBack, Mathf.Infinity, wallLayermask))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * hitBack.distance, Color.yellow);

                    if (hitBack.distance > 2f)
                    {
                        canTurnTimer = 0;
                        var turnSequence = DOTween.Sequence();
                        turnSequence.AppendInterval(0.02f);
                        turnSequence.AppendCallback(() =>
                        {
                            audioSource.PlayOneShot(turnAround);
                            transform.Rotate(new Vector3(0, 180, 0), Space.World);
                            return;
                        });
                    }
                }
        }

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


            if (GetHitPointDistance(frontMiddleRaycastStarPoint, frontMiddleRaycastStarPoint.TransformDirection(Vector3.forward), 50, Color.blue) < minDistanceToTurn)
            {
                Debug.Log("Wall in front");

                if (CheckCanTurnLeft())
                {
                    Debug.Log("can turn Left");
                    canTurnTimer = 0.2f;
                    transform.Rotate(new Vector3(0, -90, 0), Space.World);
                    audioSource.PlayOneShot(turnAround);
                    return;
                }
                if (CheckCanTurnRight())
                {
                    Debug.Log("can turn Right");
                    canTurnTimer = 0.2f;
                    audioSource.PlayOneShot(turnAround);
                    transform.Rotate(new Vector3(0, 90, 0), Space.World);
                    return;
                }

                if (!CheckCanTurnLeft() && !CheckCanTurnRight())
                {
                    Debug.Log("can turn Back");
                    canTurnTimer = 0.2f;
                    transform.Rotate(new Vector3(0, 180, 0), Space.World);
                    audioSource.PlayOneShot(turnAround);
                    return;
                }
                Debug.Log("can' turn");

            }

            if (moveLeft && CheckCanTurnLeft())
            {
                moveLeft = false;
                moveRight = false;
                moveBack = false;
                Debug.Log("can turn Left");
                canTurnTimer = 0.2f;
                transform.Rotate(new Vector3(0, -90, 0), Space.World);
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
                transform.Rotate(new Vector3(0, 90, 0), Space.World);
                audioSource.PlayOneShot(turnAround);
                return;
            }

            if (moveBack)
            {
                if (!CheckCanTurnLeft() && !CheckCanTurnRight())
                {
                    moveLeft = false;
                    moveRight = false;
                    moveBack = false;
                    Debug.Log("can turn Back");
                    canTurnTimer = 0.2f;
                    transform.Rotate(new Vector3(0, 180, 0), Space.World);
                    audioSource.PlayOneShot(turnAround);
                    return;
                }
            }

        }
        navMeshAgent.Move(transform.forward * speed * Time.deltaTime);
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
}
