using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FretNum : MonoBehaviour
{
    private int FRET_NUMBER;
    private Rigidbody actualFret;

    public StringClickHandler stringClickHandlerScript;

    private void Start()
    {
        actualFret = GetComponent<Rigidbody>();
        FRET_NUMBER = Convert.ToInt32(actualFret.name);
    }
    public int Return_Fret_Num()
    {
        return FRET_NUMBER;
    }
    private void OnMouseExit()
    {
        //same fret but over the string and not over fret
        stringClickHandlerScript.fretNum = FRET_NUMBER;
    }
}