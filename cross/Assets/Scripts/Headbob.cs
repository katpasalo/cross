using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbob : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Camera playerCamera;
    public Crouch crouch;
    public Sprint sprint;

    private float walkBobSpeed = 14f;
    private float walkBobAmount = 0.05f;
    private float sprintBobSpeed = 18f;
    private float sprintBobAmount = 0.1f;
    private float crouchBobSpeed = 8f;
    private float crouchBobAmount = 0.025f;
    private float defaultYPos = 0;
    private float timer;

    void Start()
    {
        defaultYPos = playerCamera.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHeadbob();
    }

    private void HandleHeadbob()
    {
        if (!playerMovement.isGrounded) return;
        if (playerMovement.isMoving)
        {
            timer += Time.deltaTime * (crouch.isCrouching ? crouchBobSpeed : sprint.isSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (crouch.isCrouching ? crouchBobAmount : sprint.isSprinting ? sprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
        }
    }
}
