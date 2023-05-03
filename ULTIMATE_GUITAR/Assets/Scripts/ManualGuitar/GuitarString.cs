using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarString : MonoBehaviour
{
    private int stringNumber;
    private string string_name;
    private Rigidbody actualString;
    //private bool isPressed = false;

    private void Awake()
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
    }
    public int Return_String_Number()
    {
        return stringNumber;
    }
}
