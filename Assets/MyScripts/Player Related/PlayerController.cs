using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;
using UnityEngine.EventSystems;
public class PlayerController : MonoSingleton<PlayerController>
{
    public static PlayerController _Instance { get; private set; }

    public bool isReloading;
    [SerializeField] private float reloadTime = 2f;
    [SerializeField] private float slideTime = 1.5f;
    [SerializeField] private float jumpTime = 1.5f;
    [SerializeField] private float runTime = 2f;
    [SerializeField] private float jumpCameraRotationValue = 0.1f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float jumpStartTime;
    [SerializeField] private float jumpEndTime;




    [SerializeField] private bool canMove = false;
    public bool canMoveLeft = true;
    public bool canMoveRight = true;
    public bool canMoveBack = true;
    public bool canRun;
    public bool isInRunState;
    public bool isInSlideState;
    public bool isInJumpState;
    public bool isInShootState;
    [SerializeField] private AnimationCurve jumpStartAnimationCurve;
    [SerializeField] private AnimationCurve jumpEndAnimationCurve;
    [SerializeField] public float defaultMoveSpeed = 3;
    [SerializeField] private float defaultRunSpeed = 6;
    [SerializeField] public float moveSpeed;
    [SerializeField] private float turnBackSpeed;
    [SerializeField] private float beforeTurnSideAngle;
    [SerializeField] private float beforeTurnSideSpeed;
    [SerializeField] private AnimationCurve beforeTurnSideCurve;
    [SerializeField] private float beforeBackDefaultSpeed;
    [SerializeField] private AnimationCurve turnDefaultSideCurve;

    [SerializeField] private AnimationCurve turnBackCurve;
    [SerializeField] private float turnSideSpeed;
    [SerializeField] private AnimationCurve turnSideCurve;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float gunRayDistance;

    [SerializeField] private float canTurnTimer;
    [SerializeField] private float canTurnMaxTime;

    [SerializeField] private float footStepInterval;

    [SerializeField] private AudioClip footstep1;
    [SerializeField] private AudioClip footstep2;
    [SerializeField] private AudioClip turnAround;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip noAmmoSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip slideSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip killSound;



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
    [SerializeField] public CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineComposer cinemachineComposer;
    [SerializeField] private CinemachineFreeLook freeLook;
    [SerializeField] private GameObject magnesParticles;
    public Camera mainCamera;
    public Transform weaponPivot;
    public Transform cameraPivot;
    public int ammunition;
    [HideInInspector] public bool moveLeft;
    [HideInInspector] public bool moveRight;
    [HideInInspector] public bool moveBack;

    public bool turnEnd;
    private string inst = null;

    public Animation reloadAnimation;
    private void Start()
    {



        cinemachineComposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineComposer>();
        // SnapToGround();
        StartCoroutine(Footstep());
        StartCoroutine(StartDelay());
    }
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(3f);
        moveSpeed = defaultMoveSpeed;
        cinemachineBrain.enabled = true;
    }



    public void Update()
    {
        if (closestEnemy == null)
            cinemachineVirtualCamera.LookAt = cameraPivot;
        else
        {
            if (closestEnemy.GetComponent<EnemyController>().head)
            {
                cinemachineVirtualCamera.LookAt = closestEnemy.GetComponent<EnemyController>().head;
            }
        }



        SwipeControll();
        if (canMove)
            MoveController();


        LookAtClosestEnemy();
    }
    public void LookAtClosestEnemy()
    {
        RaycastHit hit;

        if (LevelManager.Instance.dataManager.AmmunitionInMagazine <= 0)
        {
            float xLerp = Mathf.LerpAngle(weaponPivot.localEulerAngles.x, 0, 3 * Time.deltaTime);
            float yLerp = Mathf.LerpAngle(weaponPivot.localEulerAngles.y, 0, 3 * Time.deltaTime);
            float zLerp = Mathf.LerpAngle(weaponPivot.localEulerAngles.z, 0, 3 * Time.deltaTime);
            Vector3 Lerped = new Vector3(xLerp, yLerp, zLerp);
            weaponPivot.localEulerAngles = Lerped;
            return;
        }




        if (LevelManager.Instance.enemySpawner.spawnedEnemies.Count > 0)
        {

            if (IsEnemyVisible())
            {
                Vector3 dir = closestEnemy.transform.position - weaponPivot.position;
                Quaternion lookRot = Quaternion.LookRotation(dir);
                lookRot.x = 0; lookRot.z = 0;
                weaponPivot.rotation = Quaternion.Slerp(weaponPivot.rotation, lookRot, 6 * Time.deltaTime);
            }
            else
            {
                float xLerp = Mathf.LerpAngle(weaponPivot.localEulerAngles.x, 40, 3 * Time.deltaTime);
                float yLerp = Mathf.LerpAngle(weaponPivot.localEulerAngles.y, 0, 3 * Time.deltaTime);
                float zLerp = Mathf.LerpAngle(weaponPivot.localEulerAngles.z, 0, 3 * Time.deltaTime);
                Vector3 Lerped = new Vector3(xLerp, yLerp, zLerp);
                weaponPivot.localEulerAngles = Lerped;
            }
        }
        else
        {
            float xLerp = Mathf.LerpAngle(weaponPivot.localEulerAngles.x, 40, 3 * Time.deltaTime);
            float yLerp = Mathf.LerpAngle(weaponPivot.localEulerAngles.y, 0, 3 * Time.deltaTime);
            float zLerp = Mathf.LerpAngle(weaponPivot.localEulerAngles.z, 0, 3 * Time.deltaTime);
            Vector3 Lerped = new Vector3(xLerp, yLerp, zLerp);
            weaponPivot.localEulerAngles = Lerped;
        }
    }

    public Transform closestEnemy;
    public bool IsEnemyVisible()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection((Vector3.forward) + new Vector3(0.5f, 0, 0)) * gunRayDistance);
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection((Vector3.forward) - new Vector3(0.5f, 0, 0)) * gunRayDistance);
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection((Vector3.forward) + new Vector3(0.3f, 0, 0)) * gunRayDistance);
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection((Vector3.forward) - new Vector3(0.3f, 0, 0)) * gunRayDistance);
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection((Vector3.forward) + new Vector3(0.7f, 0, 0)) * gunRayDistance);
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection((Vector3.forward) - new Vector3(0.7f, 0, 0)) * gunRayDistance);
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection(Vector3.forward) * gunRayDistance);

        RaycastHit hitFrontClose;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection(Vector3.forward), out hitFrontClose, 4, enemyLayermask))
        {

            closestEnemy = hitFrontClose.transform;
            return true;
        }

        RaycastHit hitRight;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection(Vector3.forward) + new Vector3(0.3f, 0, 0), out hitRight, gunRayDistance, enemyLayermask))
        {

            closestEnemy = hitRight.transform;
            return true;
        }

        RaycastHit hitRight2;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection(Vector3.forward) + new Vector3(0.5f, 0, 0), out hitRight2, gunRayDistance, enemyLayermask))
        {

            closestEnemy = hitRight2.transform;
            return true;
        }

        RaycastHit hitRight3;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection(Vector3.forward) + new Vector3(0.8f, 0, 0), out hitRight3, gunRayDistance * 0.7f, enemyLayermask))
        {

            closestEnemy = hitRight3.transform;
            return true;
        }

        RaycastHit hitLeft;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection(Vector3.forward) - new Vector3(0.3f, 0, 0), out hitLeft, gunRayDistance, enemyLayermask))
        {

            closestEnemy = hitLeft.transform;
            return true;
        }

        RaycastHit hitLeft2;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection(Vector3.forward) - new Vector3(0.5f, 0, 0), out hitLeft2, gunRayDistance, enemyLayermask))
        {

            closestEnemy = hitLeft2.transform;
            return true;
        }
        RaycastHit hitLeft3;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection(Vector3.forward) - new Vector3(0.8f, 0, 0), out hitLeft3, gunRayDistance * 0.7f, enemyLayermask))
        {

            closestEnemy = hitLeft3.transform;
            return true;
        }

        RaycastHit hitFrontFar;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), cameraPivot.TransformDirection(Vector3.forward), out hitFrontFar, 10, enemyLayermask))
        {

            closestEnemy = hitFrontFar.transform;
            return true;
        }



        closestEnemy = null;
        return false;
    }

    //public Transform GetClosestEnemy()
    //{
    //    Transform bestTarget = null;
    //    float closestDistanceSqr = Mathf.Infinity;
    //    Vector3 currentPosition = transform.position;
    //    for (int i = 0; i < LevelManager.Instance.enemySpawner.spawnedEnemies.Count; i++)
    //    {
    //        Vector3 directionToTarget = LevelManager.Instance.enemySpawner.spawnedEnemies[i].transform.position - currentPosition;
    //        float dSqrToTarget = directionToTarget.sqrMagnitude;
    //        if (dSqrToTarget < closestDistanceSqr)
    //        {
    //            closestDistanceSqr = dSqrToTarget;
    //            bestTarget = LevelManager.Instance.enemySpawner.spawnedEnemies[i].transform;
    //        }
    //    }

    //    return bestTarget;
    //}

    IEnumerator ShootTrigger()
    {
        isInShootState = true;
        yield return new WaitForSeconds(0.25f);
        isInShootState = false;
    }
    public void Shoot()
    {
        StartCoroutine(ShootTrigger());
        if (!isReloading)
            if (LevelManager.Instance.dataManager.CheckCanShoot())
            {
                gunAnimator.SetTrigger("Shoot" + Random.Range(1, 4));

                cinemachineImpulseSource.GenerateImpulse();
                gunParticleSystem.Play();
                audioSource.PlayOneShot(shootSound);

                if (IsEnemyVisible())
                {
                    if (closestEnemy.GetComponent<EnemyController>())
                    {
                        audioSource.PlayOneShot(killSound);
                        closestEnemy.GetComponent<EnemyController>().OnHit(LevelManager.Instance.dataManager.GetWeaponDamage());
                        closestEnemy = null;
                    }

                }

                if (LevelManager.Instance.dataManager.AmmunitionInMagazine <= 0)
                {
                    if (LevelManager.Instance.dataManager.AmmunitionLeft > 0)
                    {
                        if (!isReloading)
                        {
                            Reload();
                        }
                    }
                    else
                    {
                        audioSource.PlayOneShot(noAmmoSound);
                    }
                }
            }
            else
            {
                if (LevelManager.Instance.dataManager.AmmunitionLeft > 0)
                {
                    if (!isReloading)
                    {
                        Reload();
                    }
                }
                else
                {
                    if (!isReloading)
                    {
                        audioSource.PlayOneShot(noAmmoSound);
                    }
                }
            }
    }

    public void Reload()
    {
        if (!isReloading)
            StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        audioSource.PlayOneShot(reloadSound, 1.5f);
        isReloading = true;
        gunAnimator.SetTrigger("Reload");

        yield return new WaitUntil(() => gunAnimator.GetCurrentAnimatorStateInfo(0).IsName("Reload"));
        LevelManager.Instance.uIManager.ButtonTimer(LevelManager.Instance.uIManager.imageReloadTimer, LevelManager.Instance.dataManager.GetReloadTime());
        float reloadAnimationTime = gunAnimator.GetCurrentAnimatorStateInfo(0).length;
        float targetReloadAnimationSpeed = 1 * LevelManager.Instance.dataManager.GetReloadTime() / reloadAnimationTime;
        gunAnimator.speed = 1 / targetReloadAnimationSpeed;
        yield return new WaitForSeconds(LevelManager.Instance.dataManager.GetReloadTime());
        gunAnimator.speed = 1;
        isReloading = false;
        LevelManager.Instance.dataManager.SetAmmunition();
    }

    public void SwipeControll()
    {

        if (swipeController.SwipeLeft && turnEnd && canMoveLeft)
        {
            StopCoroutine(inst);
            inst = "Left";
            StartCoroutine(RememberLastSwipe("Left"));
        }

        if (swipeController.SwipeRight && turnEnd && canMoveRight)
        {
            StopCoroutine(inst);
            inst = "Right";
            StartCoroutine(RememberLastSwipe("Right"));
        }

        if (swipeController.SwipeUp && turnEnd)
        {
            StopCoroutine(inst);
            inst = "Up";
            StartCoroutine(RememberLastSwipe("Up"));
        }



        if (swipeController.SwipeDown && turnEnd && canMoveBack)
        {
            StopCoroutine(inst);
            inst = "Down";
            StartCoroutine(RememberLastSwipe("Down"));
        }

        if (swipeController.longTap)
        {
            SlideDown();
        }

        if (swipeController.Tap)
        {
            if (Input.GetMouseButtonDown(0) == true && !EventSystem.current.IsPointerOverGameObject())
            {
                StopCoroutine(inst);
                swipeController.tap = false;
                Shoot();
            }
            else
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                {
                    Debug.Log(EventSystem.current.currentSelectedGameObject.gameObject.name);
                }
                else
                {
                    StopCoroutine(inst);
                    swipeController.tap = false;
                    Shoot();
                }
            }
        }
    }

    public void StopPlaySound()
    {
        StopAllCoroutines();
    }
    IEnumerator Footstep()
    {

        yield return new WaitForSeconds(footStepInterval);
        if (!wallRaycast() && !isInJumpState && !isInSlideState && moveSpeed > 0)
            audioSource.PlayOneShot(footstep1, 0.2f);
        yield return new WaitForSeconds(footStepInterval);
        if (!wallRaycast() && !isInJumpState && !isInSlideState && moveSpeed > 0)
            audioSource.PlayOneShot(footstep2, 0.2f);

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
            transform.position = groundHit.transform.position;


            navMeshAgent.enabled = true;
            canMove = true;
        }
    }

    public void SwitchMovement(bool state)
    {
        canMove = state;
        navMeshAgent.enabled = state;
    }

    public void SlideDown()
    {
        if (!isInSlideState)
        {
            audioSource.PlayOneShot(slideSound, 1.5f);
            isInSlideState = true;
            LevelManager.Instance.uIManager.ButtonTimer(LevelManager.Instance.uIManager.imageSlideTimer, 0.2f + 0.2f + slideTime);
            Sequence slideSequence = DOTween.Sequence();
            slideSequence.Append(cameraPivot.DOLocalMoveY(0.15f, 0.2f));
            slideSequence.AppendInterval(slideTime);
            slideSequence.Append(cameraPivot.DOLocalMoveY(0.88f, 0.2f));
            slideSequence.AppendCallback(() => isInSlideState = false);
        }
    }



    public void Run()
    {
        if (canRun)
            if (!isInRunState)
            {
                isInRunState = true;
                Sequence runSequnece = DOTween.Sequence();
                LevelManager.Instance.uIManager.ButtonTimer(LevelManager.Instance.uIManager.imageRunTimer, runTime);
                runSequnece.AppendCallback(() => moveSpeed = defaultRunSpeed);
                runSequnece.AppendInterval(runTime);
                runSequnece.AppendCallback(() => moveSpeed = defaultMoveSpeed);
                runSequnece.AppendCallback(() => isInRunState = false);
            }
    }

    public void Jump()
    {
        if (!isInJumpState)
        {
            LevelManager.Instance.uIManager.ButtonTimer(LevelManager.Instance.uIManager.imageJumpTimer, jumpStartTime + jumpTime + jumpEndTime);
            audioSource.PlayOneShot(jumpSound);
            isInJumpState = true;
            Sequence slideSequence = DOTween.Sequence();
            slideSequence.Append(cameraPivot.DOLocalMoveY(jumpHeight, jumpStartTime).SetEase(jumpStartAnimationCurve));
            slideSequence.Join(DOTween.To(() => cinemachineComposer.m_TrackedObjectOffset, x => cinemachineComposer.m_TrackedObjectOffset = x, new Vector3(0, jumpCameraRotationValue, 0), jumpEndTime).SetEase(jumpStartAnimationCurve));

            slideSequence.AppendInterval(jumpTime);
            slideSequence.Join(DOTween.To(() => cinemachineComposer.m_TrackedObjectOffset, x => cinemachineComposer.m_TrackedObjectOffset = x, new Vector3(0, 0, 0), jumpEndTime).SetEase(jumpStartAnimationCurve));
            slideSequence.Append(cameraPivot.DOLocalMoveY(0.88f, jumpEndTime).SetEase(jumpEndAnimationCurve));
            slideSequence.Join(DOTween.To(() => cinemachineComposer.m_TrackedObjectOffset, x => cinemachineComposer.m_TrackedObjectOffset = x, new Vector3(0, -jumpCameraRotationValue, 0), jumpEndTime).SetEase(jumpStartAnimationCurve));
            slideSequence.Append(DOTween.To(() => cinemachineComposer.m_TrackedObjectOffset, x => cinemachineComposer.m_TrackedObjectOffset = x, new Vector3(0, 0, 0), jumpEndTime).SetEase(jumpStartAnimationCurve));
            slideSequence.AppendCallback(() => isInJumpState = false);
        }
    }

    private bool turnBackLeft;
    public void TurnBack(bool isLeft)
    {
        turnBackLeft = isLeft;
        moveBack = true;
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
    public Sequence turnSequence;
    public Sequence firstTurn;
    IEnumerator RememberLastSwipe(string direction)
    {

        if (direction == "Left" && turnEnd && !moveLeft && !moveRight)
        {


            if (!wallRaycast())
            {
                firstTurn = DOTween.Sequence();
                firstTurn.Append(cameraPivot.DOLocalRotate(new Vector3(0, -beforeTurnSideAngle, 0), beforeTurnSideSpeed).SetEase(beforeTurnSideCurve));
            }
            yield return new WaitForSeconds(0.01f);
            moveLeft = true;
        }
        else if (direction == "Left" && turnEnd && moveLeft)
        {
            moveLeft = false;
            TurnBack(true);
        }
        else if (direction == "Left" && turnEnd && moveRight)
        {
            moveRight = false;
            moveLeft = false;
            yield return new WaitForSeconds(0.01f);
            cameraPivot.DOLocalRotate(new Vector3(0, 0, 0), beforeTurnSideSpeed).SetEase(beforeTurnSideCurve);
        }




        if (direction == "Right" && turnEnd && !moveRight && !moveLeft)
        {


            if (!wallRaycast())
            {
                firstTurn = DOTween.Sequence();
                firstTurn.Append(cameraPivot.DOLocalRotate(new Vector3(0, beforeTurnSideAngle, 0), beforeTurnSideSpeed).SetEase(beforeTurnSideCurve));
            }
            yield return new WaitForSeconds(0.01f);
            moveRight = true;
        }
        else if (direction == "Right" && turnEnd && moveRight)
        {
            moveRight = false;
            TurnBack(false);
        }
        else if (direction == "Right" && turnEnd && moveLeft)
        {
            moveRight = false;
            moveLeft = false;
            yield return new WaitForSeconds(0.01f);
            cameraPivot.DOLocalRotate(new Vector3(0, 0, 0), beforeTurnSideSpeed).SetEase(beforeTurnSideCurve);
        }




        if (direction == "Up")
        {
            Jump();
        }
        if (direction == "Down" && turnEnd)
        {
            moveBack = true;
        }
        yield return new WaitForSeconds(0.1f);
    }




    public void MoveController()
    {
        if (CanTurn())
        {
            if (moveLeft && CheckCanTurnLeft() && turnEnd)
            {
                if (firstTurn != null)
                {
                    firstTurn.Kill();
                }
                moveLeft = false;
                moveRight = false;
                moveBack = false;
                if (turnSequence != null)
                {
                    turnSequence.Kill();
                }
                turnEnd = false;
                ; canTurnTimer = 0.2f;
                turnSequence = DOTween.Sequence();
                turnSequence.AppendCallback(() => canMove = false);
                turnSequence.Append(transform.DOLocalRotate(new Vector3(0, -90, 0), turnSideSpeed, RotateMode.LocalAxisAdd).SetEase(turnSideCurve));
                turnSequence.AppendCallback(() => canMove = true);
                turnSequence.Append(cameraPivot.DOLocalRotate(new Vector3(0, 0, 0), turnSideSpeed).SetEase(turnSideCurve));
                turnSequence.AppendCallback(() => turnEnd = true);
                if (!isInRunState)
                    turnSequence.AppendCallback(() => moveSpeed = defaultMoveSpeed);
                return;
            }
            if (moveRight && CheckCanTurnRight() && turnEnd)
            {
                if (firstTurn != null)
                {
                    firstTurn.Kill();
                }
                moveLeft = false;
                moveRight = false;
                moveBack = false;
                turnEnd = false;
                if (turnSequence != null)
                {
                    turnSequence.Kill();
                }
                canTurnTimer = 0.2f;
                turnSequence = DOTween.Sequence();
                turnSequence.AppendCallback(() => canMove = false);
                turnSequence.Append(transform.DOLocalRotate(new Vector3(0, 90, 0), turnSideSpeed, RotateMode.LocalAxisAdd).SetEase(turnSideCurve));
                turnSequence.AppendCallback(() => canMove = true);
                turnSequence.Append(cameraPivot.DOLocalRotate(new Vector3(0, 0, 0), turnSideSpeed).SetEase(turnSideCurve));
                turnSequence.AppendCallback(() => turnEnd = true);
                if (!isInRunState)
                    turnSequence.AppendCallback(() => moveSpeed = defaultMoveSpeed);
                return;
            }

            if (moveBack)
            {
                //  if (!CheckCanTurnLeft() && !CheckCanTurnRight())
                {
                    moveLeft = false;
                    moveRight = false;
                    moveBack = false;
                    turnEnd = false;
                    if (turnSequence != null)
                    {
                        turnSequence.Kill();
                    }
                    moveSpeed = defaultMoveSpeed;
                    canTurnTimer = 0.2f;
                    turnSequence = DOTween.Sequence();
                    turnSequence.AppendCallback(() => canMove = false);

                    turnSequence.Append(transform.DOLocalRotate(new Vector3(0, 180, 0), turnBackSpeed, RotateMode.LocalAxisAdd).SetEase(turnBackCurve));




                    if (turnBackLeft)
                        turnSequence.Join(cameraPivot.DOLocalRotate(new Vector3(0, 1, 0), turnBackSpeed, RotateMode.FastBeyond360).SetEase(turnBackCurve));

                    else
                    {
                        turnSequence.Join(cameraPivot.DOLocalRotate(new Vector3(0, 359, 0), turnBackSpeed, RotateMode.Fast).SetEase(turnBackCurve));
                    }
                    turnSequence.AppendCallback(() => canMove = true);
                    turnSequence.AppendCallback(() => turnEnd = true);

                    return;
                }
            }

        }
        if (!wallRaycast())
        {
            navMeshAgent.Move(transform.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            if (turnSequence != null)
            {
                turnSequence.Kill();
            }
            //  cameraPivot.localEulerAngles = Vector3.zero;
        }

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

    public void OnSlideObstacleHit(NavMeshObstacle navMeshObstacle)
    {
        moveSpeed = 0;
        StartCoroutine(SlideObstacleHitCoroutine(navMeshObstacle));
    }
    IEnumerator SlideObstacleHitCoroutine(NavMeshObstacle navMeshObstacle)
    {
        yield return new WaitUntil(() => isInSlideState || moveBack);
        moveSpeed = defaultMoveSpeed;
    }


    public void OnJumpObstacleHit(NavMeshObstacle navMeshObstacle)
    {
        moveSpeed = 0;
        StartCoroutine(JumpObstacleHitCoroutine(navMeshObstacle));
    }
    IEnumerator JumpObstacleHitCoroutine(NavMeshObstacle navMeshObstacle)
    {
        yield return new WaitUntil(() => isInJumpState || moveBack);
        moveSpeed = defaultMoveSpeed;
    }

    public void SwitchMagnesParticles(bool state)
    {
        magnesParticles.SetActive(state);
    }
}
