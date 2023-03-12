using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FretNum : MonoBehaviour
{
    private int FRET_NUMBER;
    private Rigidbody actualFret;

    private void Start()
    {
        actualFret = GetComponent<Rigidbody>();
        FRET_NUMBER = Convert.ToInt32(actualFret.name);
    }
    public int Return_Fret_Num()
    {
        return FRET_NUMBER;
    }
}
