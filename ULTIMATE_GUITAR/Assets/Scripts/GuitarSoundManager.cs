using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarSoundManager : MonoBehaviour
{
    public AudioSource Elow;
    public AudioSource A;
    public AudioSource D;
    public AudioSource G;
    public AudioSource B;
    public AudioSource Ehigh;

    public float[] pitchAdjustments;

    private GuitarSoundManager guitarSoundManager;

    private void Start()
    {
        // Initialize pitch adjustments array
        pitchAdjustments = new float[21];
        for (int i = 0; i < 21; i++)
        {
            pitchAdjustments[i] = Mathf.Pow(2f, i/12f);
        }

        guitarSoundManager = GetComponent<GuitarSoundManager>();
    }

    public void PlayFretSound(int fretNum, int strNum)
    {
        float pitchAdjustment = pitchAdjustments[fretNum];
        AudioSource stringSound = guitarSoundManager.GetStringSound(strNum);
        if (stringSound != null)
        {
            stringSound.pitch = pitchAdjustment;
            stringSound.Play();
        }
    }
    public AudioSource GetStringSound(int stringNumber)
    {
        if (stringNumber == 1)
            return Elow;
        else if (stringNumber == 2)
            return A;
        else if (stringNumber == 3)
            return D;
        else if (stringNumber == 4)
            return G;
        else if (stringNumber == 5)
            return B;
        else if (stringNumber == 6)
            return Ehigh;
        else
            return null;
    }
}