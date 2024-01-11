using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public Animator doorAnimator;
    public GameObject openText;
    public bool doorOpen = false;

    // public AudioSource doorSound;

    public override void OnFocus()
    {
        openText.SetActive(true);
    }
    public override void OnInteract()
    {
        if (doorOpen) {CloseDoor();}
        else {OpenDoor();}
    }
    public override void OnLoseFocus()
    {
        openText.SetActive(false);
    }

    private void OpenDoor()
    {
        doorAnimator.SetBool("open",true);
        doorOpen = true;
    }

    private void CloseDoor()
    {
        doorAnimator.SetBool("open",false);
        doorOpen = false;
    }
}