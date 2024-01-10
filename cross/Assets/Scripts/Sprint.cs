using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprint : MonoBehaviour
{
    public PlayerMovement playerMovement;
    
    public float sprintSpeed = 18f;

    public bool isSprinting = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerMovement.SetSpeed(sprintSpeed);
            isSprinting = true;
        }
        else
        {
            playerMovement.SetSpeed(playerMovement.walkSpeed);
            isSprinting = false;
        }
    }
}
