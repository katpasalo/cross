using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    public CharacterController controller;

    public float crouchSpeed = 6;
    public float normalHeight = 4;
    public float crouchHeight = 1;

    public Vector3 offset; // prevent falling through ground
    public Transform player;
    bool isCrouching;

    void Update()
    {
        HandleCrouchInput();
        HandleCrouching();
    }

    void HandleCrouchInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
        }
    }

    void HandleCrouching()
    {
        float targetHeight = isCrouching ? crouchHeight : normalHeight;
        float smoothHeight = Mathf.Lerp(controller.height, targetHeight, crouchSpeed * Time.fixedDeltaTime);

        // Smoothly adjust the controller height
        controller.height = smoothHeight;

        float yOffset = (controller.height - normalHeight) / 2;
        Vector3 targetPosition = new Vector3(player.position.x, normalHeight + yOffset, player.position.z);

        // Smoothly adjust the player position
        player.position = Vector3.Lerp(player.position, targetPosition, crouchSpeed * Time.smoothDeltaTime);
    }
}