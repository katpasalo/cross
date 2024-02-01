using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("Functional Options")]
    [SerializeField] private bool canFlashlight = true;

    [Header("Controls")]
    [SerializeField] private KeyCode flashlightKey = KeyCode.F;

    public GameObject flashlight;

    // public AudioSource turnOn;
    // public AudioSource turnOff;

    public bool flashlightOn;

    void Start()
    {
        flashlightOn = false;
        flashlight.SetActive(false);    
    }

    void Update()
    {
        if (canFlashlight) {HandleFlashlight();}
    }

    void HandleFlashlight()
    {
        if (flashlightOn == false && Input.GetKeyDown(flashlightKey))
        {
            flashlight.SetActive(true);
            // turnOn.Play();
            flashlightOn = true;
        }
        else if (flashlightOn && Input.GetKeyDown(flashlightKey))
        {
            flashlight.SetActive(false);
            // turnOff.Play();
            flashlightOn = false;
        }
    }
}
