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

    public bool inventory_Input;

    public bool use_Input;
    public bool cancel_Input;


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

            input.PlayerActions.Inventory.performed += ctx => inventory_Input = true;

            input.PlayerActions.Use.performed += ctx => use_Input = true;
            input.PlayerActions.Cancel.performed += ctx => cancel_Input = true;
            

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
        InventoryInput();
        UseInput();
        CancelInput();
    }

    private void MovementInput()
    {
        if(GameManager.Instance.gameIsPaused) return;

        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, pc.isSprinting);
    }

    private void InventoryInput(){
        if(inventory_Input) {
            inventory_Input = false;
        }
    }
    
    private void UseInput(){
        if(use_Input){
            use_Input = false;
        }
    }

    private void CancelInput(){
        if(cancel_Input){
            cancel_Input = false;
        }
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