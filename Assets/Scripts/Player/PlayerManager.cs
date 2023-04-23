using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Animator animator;
    InputManager inputManager;
    public CameraManager cameraManager;
    PlayerController pc;

    public bool isInteracting;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        pc = GetComponent<PlayerController>();
    }

    private void Update()
    {
        inputManager.AllInputs();
    }

    private void FixedUpdate()
    {
        pc.AllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.AllCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
        pc.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", pc.isGrounded);
    }
}
