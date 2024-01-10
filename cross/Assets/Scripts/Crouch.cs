using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    public CharacterController controller;

    public float crouchSpeed = 6;
    public float normalHeight = 4;
    public float crouchHeight = 1;

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

        controller.height = smoothHeight;
    }
}