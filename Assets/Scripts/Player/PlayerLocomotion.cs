using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    public AnimatorManager animatorManager;
    InputManager inputManager;

    Vector3 moveDir;
    public Camera cam;
    Rigidbody rb;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffSet = 0.5f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 7f;
    public float sprintingSpeed = 5f;
    public float rotationSpeed = 15f;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15f;

    [Header("Battle")]
    public ObjTransform camState;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
    }

    public void AllMovement()
    {
        FallingAndLanding();

        if (playerManager.isInteracting) return;
        if (isJumping) return;

        Movement();
        Rotation();
    }

    private void Movement()
    {
        if (isJumping) return;
        moveDir = cam.transform.forward * inputManager.verticalInput;
        moveDir = moveDir + cam.transform.right * inputManager.horizontalInput;
        moveDir.Normalize();
        moveDir.y = 0;

        if (isSprinting)
        {
            moveDir = moveDir * sprintingSpeed;
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDir = moveDir * runningSpeed;
            }
            else
            {
                moveDir = moveDir * walkingSpeed;

            }

            moveDir = moveDir * walkingSpeed;
        }
        

        Vector3 movementVelocity = moveDir;
        rb.velocity = movementVelocity;
    }

    private void Rotation()
    {
        if (isJumping) return;
        Vector3 targetDir = Vector3.zero;

        targetDir = cam.transform.forward * inputManager.verticalInput;
        targetDir = targetDir + cam.transform.right * inputManager.horizontalInput;
        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero) targetDir = transform.forward;

        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRot;
    }

    private void FallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffSet;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out hit, 1f, groundLayer))
        {
            if (!isGrounded && playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }

            Vector3 rayCastHitPoint = hit.point;
            inAirTimer = 0;
            isGrounded = true;
            playerManager.isInteracting = false;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void Jumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDir;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;
        }
    }
    bool state;
    public void BattleState()
    {
        if (state)
        {
            state = false;
            camState.enabled = false;
        }
        else
        {
            state = true;
            camState.enabled = true;
        }
    }
}