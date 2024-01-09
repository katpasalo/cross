using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprint : MonoBehaviour
{
    public PlayerMovement playerMovement; // reference to the movement script
    
    public float sprintSpeed = 18f;

    void Update()
    {
        // Check if the player is pressing the sprint key (change it to whatever key you want)
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            playerMovement.SetSpeed(sprintSpeed);
        }
        else
        {
            playerMovement.SetSpeed(playerMovement.walkSpeed);
        }
    }
}
