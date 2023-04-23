using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerManager playerManager;
    public AnimatorManager animatorManager;
    InputManager input;

    Vector3 moveDir;
    public Camera cam;
    Rigidbody rb;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffSet = 0.5f;
    public LayerMask groundLayer;

    [Header("Statements")]

    public MovementState state;
    public enum MovementState
    {
        jogging,
        walking,
        sprinting
    }

    public bool isSprinting;
    public bool isWalking;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    public float currentSpeed;
    public float walkSpeed = 0.5f;
    public float jogSpeed = 1.5f;
    public float sprintSpeed = 4.5f;
    public float rotationSpeed = 15f;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15f;

    [Header("Battle")]
    public ObjTransform camState;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        input = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
    }

    public void AllMovement()
    {
        FallingAndLanding();

        if (playerManager.isInteracting) return;
        if (isJumping) return;

        isWalking = WeaponHolder.Instance.scoping;

        Movement();
        Rotation();
        StateHandler();
    }

    private void StateHandler()
    {

        if (isWalking)
        {
            state = MovementState.walking;
            currentSpeed = walkSpeed;
        }

        else if (input.sprint_Input)
        {
            state = MovementState.sprinting;
            currentSpeed = sprintSpeed;
        }

        else
        {
            state = MovementState.jogging;
            currentSpeed = jogSpeed;
        }
    }


    private void Movement()
    {
        if (isJumping) return;
        moveDir = cam.transform.forward * input.verticalInput;
        moveDir = moveDir + cam.transform.right * input.horizontalInput;
        moveDir.Normalize();
        moveDir.y = 0;

        moveDir = moveDir * currentSpeed;

        Vector3 movementVelocity = moveDir;
        rb.velocity = movementVelocity;
    }

    Quaternion targetRot;
    Quaternion playerRot;

    private void Rotation()
    {
        Vector3 targetDir = Vector3.zero;
        if (WeaponHolder.Instance.scoping)
        {
            targetDir = cam.transform.forward * 1;
            targetDir.Normalize();
            targetDir.y = 0;

            targetRot = Quaternion.LookRotation(targetDir);
            playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            transform.rotation = playerRot;

        }
        else
        {
            targetDir = cam.transform.forward * input.verticalInput;
            targetDir = targetDir + cam.transform.right * input.horizontalInput;
            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero) targetDir = transform.forward;

            targetRot = Quaternion.LookRotation(targetDir);
            playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            transform.rotation = playerRot;
        }
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
}