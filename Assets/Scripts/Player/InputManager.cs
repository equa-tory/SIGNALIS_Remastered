using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerInput input;
    PlayerLocomotion playerLocomotion;
    public AnimatorManager animatorManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool sprint_Input;
    public bool jump_Input;
    public bool battleState_Input;


    private void Awake()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void OnEnable()
    {
        if(input == null)
        {
            input = new PlayerInput();

            input.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
            input.PlayerMovement.Camera.performed += ctx => cameraInput = ctx.ReadValue<Vector2>();

            input.PlayerActions.Sprint.performed += ctx => sprint_Input = true;
            input.PlayerActions.Sprint.canceled += ctx => sprint_Input = false;

            input.PlayerActions.Jump.performed += ctx => jump_Input = true;

            input.PlayerActions.BattleState.performed += ctx => battleState_Input = true;
        }

        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void AllInputs()
    {
        MovementInput();
        SprintInput();
        JumpingInput();
        //BattleStateInput();
    }

    private void MovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
    }

    private void SprintInput()
    {
        if (sprint_Input && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void JumpingInput()
    {
        if (jump_Input)
        {
            jump_Input = false;
            playerLocomotion.Jumping();
        }
    }

    private void BattleStateInput()
    {
        if (battleState_Input)
        {
            battleState_Input = false;
            playerLocomotion.BattleState();
        }
    }

}