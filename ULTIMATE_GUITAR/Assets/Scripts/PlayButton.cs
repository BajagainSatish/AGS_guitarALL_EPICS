using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public AudioSource audioSourceA;
    public AudioSource audioSourceB;

    public PitchDetector pitchDetector;
    public void OnClickButtonA()
    {
        audioSourceA.Play();
    }
    public void OnClickButtonB()
    {
        audioSourceB.Play();
    }
    public void OnClickCompare()
    {
        //
    }
}
