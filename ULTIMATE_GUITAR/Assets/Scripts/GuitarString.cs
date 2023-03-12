using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarString : MonoBehaviour
{
    private int stringNumber;
    private string string_name;
    private Rigidbody actualString;
    private bool isPressed = false;

    private void Start()
    {
        actualString = GetComponent<Rigidbody>();
        string_name = actualString.name;
        if (string_name == "Elow")
        {
            stringNumber = 1;
        }
        if (string_name == "A")
        {
            stringNumber = 2;
        }
        if (string_name == "D")
        {
            stringNumber = 3;
        }
        if (string_name == "G")
        {
            stringNumber = 4;
        }
        if (string_name == "B")
        {
            stringNumber = 5;
        }
        if (string_name == "Ehigh")
        {
            stringNumber = 6;
        }
        Debug.Log(stringNumber);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Fret"))
        {
            // Set the isPressed variable to true if a fret is being pressed
            isPressed = true;
            // Get the fret number from the other collider's script
            int fretNumber = other.GetComponent<FretNum>().Return_Fret_Num();
            // Play the appropriate sound
            //GuitarSoundManager.instance.PlaySound(stringNumber, fretNumber);
            Debug.Log(fretNumber + stringNumber);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fret"))
        {
            // Set the isPressed variable to false when the fret is released
            isPressed = false;
        }
    }

}
