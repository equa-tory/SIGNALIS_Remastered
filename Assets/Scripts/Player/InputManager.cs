using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    PlayerInput input;
    PlayerController pc;
    public AnimatorManager animatorManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool sprint_Input;

    public bool shoot_Input;
    public bool scope_Input;
    public bool reload_Input;


    private void Awake()
    {
        Instance = this;
        pc = GetComponent<PlayerController>();
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

            input.PlayerActions.Shoot.performed += ctx => shoot_Input = true;

            input.PlayerActions.Scope.performed += ctx => scope_Input = true;
            input.PlayerActions.Scope.canceled += ctx => scope_Input = false;
            
            input.PlayerActions.Reload.performed += ctx => reload_Input = true;
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
        ShootInput();
        ReloadInput();
    }

    private void MovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, pc.isSprinting);
    }

    private void ShootInput()
    {
        if (shoot_Input)
        {
            shoot_Input = false;
        }
    }

    private void ReloadInput()
    {
        if (reload_Input)
        {
            reload_Input = false;
        }
    }

}