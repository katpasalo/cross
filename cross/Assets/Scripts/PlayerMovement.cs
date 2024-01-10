using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Camera playerCamera;

    [Header("Functional Options")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canHeadbob = true;

    [Header("Movement Parameters")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float walkSpeed = 12f;
    [SerializeField] private float sprintSpeed = 18f;
    [SerializeField] private float crouchSpeed = 6f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravity = -30f;
    [SerializeField] private float groundDistance = 0.4f;
    public Transform groundCheck;
    public LayerMask groundMask;
    Vector3 velocity;

    [Header("Crouch Parameters")]
    [SerializeField] private float normalHeight = 4f;
    [SerializeField] private float crouchHeight = 1f;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.1f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;
    [SerializeField] private float defaultYPos = 0;
    [SerializeField] private float timer;

    private bool isGrounded;
    private bool isMoving = false;
    private bool isSprinting = false;
    private bool isCrouching = false;

    void Start()
    {
        defaultYPos = playerCamera.transform.localPosition.y;
    }

    void Update()
    {
        GroundCheck();

        if (canSprint) {HandleSprint();}
        if (canCrouch) {HandleCrouch();}
        if (canHeadbob) {HandleHeadbob();}
        if (canJump) {HandleJump();}

        HandleMovement();
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
        if(Input.GetButtonDown("Jump") && isGrounded)
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            SetSpeed(sprintSpeed);
            isSprinting = true;
        }
        else
        {
            SetSpeed(walkSpeed);
            isSprinting = false;
        }
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
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
}