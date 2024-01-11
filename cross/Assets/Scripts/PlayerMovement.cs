using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Camera playerCamera;

    [Header("Functional Options")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canHeadbob = true;
    [SerializeField] private bool canClimb = true;
    [SerializeField] private bool canInteract = true;

    [Header("Controls")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode interactKey = KeyCode.Mouse0;

    [Header("Movement Parameters")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float walkSpeed = 12f;
    [SerializeField] private float sprintSpeed = 18f;
    [SerializeField] private float crouchMoveSpeed = 5f;
    [SerializeField] private float climbSpeed = 5f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravity = -50f;
    [SerializeField] private float groundDistance = 0.4f;
    public Transform groundCheck;
    public LayerMask groundMask;
    Vector3 velocity;

    [Header("Crouch Parameters")]
    [SerializeField] private float normalHeight = 4f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchSpeed = 6f;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.1f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;
    [SerializeField] private float defaultYPos = 0;
    [SerializeField] private float timer;

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    private Interactable currentInteractable;

    private bool isGrounded;
    private bool isMoving = false;
    private bool isSprinting = false;
    private bool isCrouching = false;
    private bool isClimbing = false;

    void Start()
    {
        defaultYPos = playerCamera.transform.localPosition.y;
    }

    void Update()
    {
        if (canMove) 
        {
            GroundCheck();

            if (canSprint) {HandleSprint();}
            if (canCrouch) {HandleCrouch();}
            if (canHeadbob) {HandleHeadbob();}
            if (canJump) {HandleJump();}
            if (canClimb) {HandleClimb();}
            if (canInteract) 
            {
                HandleInteractionCheck();
                HandleInteractionInput();
            }

            HandleMovement();
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
        }        
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        isMoving = (x != 0f || z != 0f);

        controller.Move(move * speed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if(Input.GetKeyDown(jumpKey) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);        
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void HandleSprint()
    {
        if (Input.GetKey(sprintKey))
        {
            SetSpeed(sprintSpeed);
            isSprinting = true;
        }
        else
        {
            if (!isCrouching) 
            {
                SetSpeed(walkSpeed);
            }
            isSprinting = false;
        }
    }

    private void HandleCrouch()
    {
        if (Input.GetKey(crouchKey))
        {
            SetSpeed(crouchMoveSpeed);
            isCrouching = true;

        }
        else
        {
            if (!isSprinting)
            {
                SetSpeed(walkSpeed);
            }
            isCrouching = false;
        }

        float targetHeight = isCrouching ? crouchHeight : normalHeight;
        float smoothHeight = Mathf.Lerp(controller.height, targetHeight, crouchSpeed * Time.fixedDeltaTime);

        controller.height = smoothHeight;
    }    

    private void HandleHeadbob()
    {
        if (!isGrounded) return;
        if (isMoving)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : isSprinting ? sprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
        }
    }

    private void HandleClimb()
    {
        RaycastHit ladderHit;
        if (Physics.Raycast(transform.position, transform.forward, out ladderHit, 2f) && ladderHit.collider.CompareTag("Ladder"))
        {
            isClimbing = true;
            gravity = 0f;
            velocity.y = 0f;

            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 climbDirection = new Vector3(horizontalInput, verticalInput, 0f);
            climbDirection = transform.TransformDirection(climbDirection); // Convert to world space
            controller.Move(climbDirection * climbSpeed * Time.deltaTime);
        }
        else
        {
            isClimbing = false;
            gravity = -50f; // Reset gravity to its original value
        }
    }

    private void HandleInteractionCheck()
    {
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject.layer == 9 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.gameObject.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);

                if (currentInteractable)
                {
                    currentInteractable.OnFocus();
                }
            }        
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }

    private void HandleInteractionInput()
    {
        if(Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInteractable.OnInteract();
        }
    }
}