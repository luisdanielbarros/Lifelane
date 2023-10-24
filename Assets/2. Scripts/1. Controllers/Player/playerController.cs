using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
public class playerController : MonoBehaviour
{
    //Camera, Controller
    [SerializeField]
    private CharacterController Cntrl;
    [SerializeField]
    private Transform firstPersonCamera;
    [SerializeField]
    private Transform thirdPersonCamera;
    private CinemachineFreeLook thirdPersonCameraCine;
    //Movement
    private float walkingSpeed = 6f;
    private float runningSpeed = 12f;
    //Movement -> Smooth
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    //Falling
    Vector3 verticalVelocity;
    private float Gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;
    private bool isGrounded;
    //Jumping
    private float jumpHeight = 2f;
    //Animator
    private List<Animator> animCntrl;
    private int animHorizontalSpeedHash = Animator.StringToHash("horizontalSpeed");
    private int animVerticalSpeedHash = Animator.StringToHash("verticalSpeed");
    private int animIsGroundedHash = Animator.StringToHash("isGrounded");
    private int animIsRunningHash = Animator.StringToHash("isRunning");
    //Micro-State
    private bool isWalking = false;
    //Movement Profile
    private playerMovement movementprofile = playerMovement.Normal;
    private float quietMovementMultiplier = 0.5f;
    public playerMovement movementProfile { get { return movementprofile; } set { movementprofile = value; } }
    void Start()
    {
        thirdPersonCameraCine = thirdPersonCamera.GetComponent<CinemachineFreeLook>();
        //Animator
        animCntrl = new List<Animator>();
    }
    void FixedUpdate()
    {
        if (gameState.Instance.currentState != gameStates.OverWorld) return;
        //Input
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 movementDirection = new Vector3(0f, 0f, 0f);
        ////Is Running
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        //Perspective
        playerPerspective playerPers = cameraManager.Instance.playerPers;
        //Movement
        ////Movement Direction
        Vector3 Direction = new Vector3(-x, 0f, -z).normalized;
        if (Direction.magnitude >= 0.1f)
        {
            //////Third Person Controls
            if (playerPers == playerPerspective.Controlled)
            {
                float targetAngle = Mathf.Atan2(-Direction.x, -Direction.z) * Mathf.Rad2Deg + thirdPersonCameraCine.m_XAxis.Value;
                float rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
                movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }
            //////First Person Controls
            else if (playerPers == playerPerspective.POV)
            {
                float targetAngle = Mathf.Atan2(-Direction.x, -Direction.z) * Mathf.Rad2Deg + firstPersonCamera.eulerAngles.y;
                movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }
            //////Mapped Controls
            else if (playerPers == playerPerspective.Mapped)
            {
                float targetAngle = Mathf.Atan2(-Direction.x, -Direction.z) * Mathf.Rad2Deg;
                float rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
                movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }
            ////Movement Profile
            if (movementProfile == playerMovement.Normal)
            {
                if (isRunning) Cntrl.Move(movementDirection.normalized * runningSpeed * Time.deltaTime);
                else Cntrl.Move(movementDirection.normalized * walkingSpeed * Time.deltaTime);
            }
            else
            {
                if (isRunning) Cntrl.Move(movementDirection.normalized * runningSpeed * Time.deltaTime * quietMovementMultiplier);
                else Cntrl.Move(movementDirection.normalized * walkingSpeed * Time.deltaTime * quietMovementMultiplier);
            }
        }
        //Falling & Jumping
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.75f + groundDistance, groundMask);
        if (Input.GetButtonDown("Jump") && isGrounded) verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * Gravity);
        else if (isGrounded && verticalVelocity.y < 0f) verticalVelocity.y = 0;
        else if (!isGrounded) verticalVelocity.y += Gravity * Time.deltaTime;
        Cntrl.Move(verticalVelocity * Time.deltaTime);
        //Animation Controller
        if (animCntrl != null)
        {
            for (int i = 0; i < animCntrl.Count; i++)
            {
                animCntrl[i].SetFloat(animHorizontalSpeedHash, Math.Abs(x) + Math.Abs(z));
                animCntrl[i].SetFloat(animVerticalSpeedHash, verticalVelocity.y);
                animCntrl[i].SetBool(animIsGroundedHash, isGrounded);
                if ((Math.Abs(x) + Math.Abs(z) > 0) && movementProfile == playerMovement.Normal && isRunning) animCntrl[i].SetBool(animIsRunningHash, true);
                else animCntrl[i].SetBool(animIsRunningHash, false);
            }
        }
        //Audio
        if ((Math.Abs(x) + Math.Abs(z) > 0) && isGrounded)
        {
            if (!isWalking) AudioManager.Instance.playPlayerWalking();
            isWalking = true;
        }
        else
        {
            if (isWalking) AudioManager.Instance.stopPlayerWalking();
            isWalking = false;
        }
    }
    //Animation
    public void stopAnimationWalking()
    {
        //Animation Controller
        if (animCntrl != null)
        {
            for (int i = 0; i < animCntrl.Count; i++)
            {
                animCntrl[i].SetFloat(animHorizontalSpeedHash, 0f);
                animCntrl[i].SetFloat(animVerticalSpeedHash, 0f);
                animCntrl[i].SetBool(animIsGroundedHash, true);
            }
        }
        //Audio
        AudioManager.Instance.stopPlayerWalking();
    }
    //Animators
    public void addAnimator(Animator _animCntrl)
    {
        animCntrl.Add(_animCntrl);
    }
    public void resetAnimators()
    {
        animCntrl.Clear();
    }
}
