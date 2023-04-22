//by TheSuspect
//06.04.2023

using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public Transform orientation;
    private Rigidbody rb;
    
    [Space]


    [Header("Heath")]

    public float currentHealth;
    public float maxHealth = 100f;
    public Image healthBarImage;
    public TMP_Text healthText;


    [Header("Movement")]

    public float currentSpeed;
    public float walkSpeed = 7f;
    public float sprintSpeed = 12f;
    public float crouchSpeed = 4f;

    [Space]

    Vector3 moveDirection;

    [Space]

    public float maxSlopeAngle = 40f;
    private RaycastHit slopeHit;
    private bool extitingSlope;


    [Header("Jumping")]

    public float airMultiplayer = 1f;
    public float groundDrag = .5f;
    public float jumpForce = 20f;
    public float jumpCooldwn = 1f;
    bool readyToJump = true;

    [Space]

    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    public bool grounded;


    [Header("Statements")]

    public MovementState state;
    public enum MovementState
    {
        walking,
        crouching,
        sprinting,
        air
    }

    public bool crouching;


    [Header("Input")]

    //InputManager input;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.X;
    public KeyCode sprintKey = KeyCode.LeftShift;


    private void Awake()
    {
        SetUp();
    }

    private void Update()
    {
        StateHandler();
        MyInput();
        SpeedControl();
    }

    private void FixedUpdate(){
        Movement();
    }

    //--------------------------------------------------------------------------------------------------------------------------------

    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, whatIsGround);

        #region Ground Drag
        if (grounded) rb.drag = groundDrag;
        else rb.drag = 0;
        #endregion
    }

    private void MyInput()
    {
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {

            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldwn);
        }
    }

    private void StateHandler()
    {

        if (crouching)
        {
            currentSpeed = crouchSpeed;
            state = MovementState.crouching;
        }

        else if (Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            currentSpeed = sprintSpeed;
        }

        else if (!Input.GetKey(sprintKey))
        {
            state = MovementState.walking;
            currentSpeed = walkSpeed;
        }

        else
        {
            state = MovementState.air;
            currentSpeed = walkSpeed;
        }
    }

    private void Movement()
    {

        moveDirection = orientation.forward * Input.GetAxisRaw("Vertical") 
            + orientation.right * Input.GetAxisRaw("Horizontal");


        if (OnSlope() && !extitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * currentSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        //Realistic Air Speed
        //airMultiplayer = 1 / currentSpeed;

        if (grounded)
            rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);

        else if (!grounded)
            rb.AddForce(moveDirection.normalized * currentSpeed * 10f * airMultiplayer, ForceMode.Force);

        rb.useGravity = !OnSlope();


    }

    private void SpeedControl()
    {
        if (currentSpeed < 0) currentSpeed = 0;

        if (OnSlope() && !extitingSlope)
        {
            if (rb.velocity.magnitude > currentSpeed)
                rb.velocity = rb.velocity.normalized * currentSpeed;
        }

        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > currentSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * currentSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }

        }
    }

    private void Jump()
    {
        extitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        extitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    //--------------------------------------------------------------------------------------------------------------------------------

    private void SetUp()
    {

        //input = InputManager.Instance;

        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        sprintSpeed = walkSpeed;
    }

    public void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        //healthText.text = currentHealth + "/" + maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
