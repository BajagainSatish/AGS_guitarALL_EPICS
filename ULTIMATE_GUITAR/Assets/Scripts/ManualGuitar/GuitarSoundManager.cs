using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GuitarSoundManager : MonoBehaviour
{
    public AudioSource Elow;
    public AudioSource A;
    public AudioSource D;
    public AudioSource G;
    public AudioSource B;
    public AudioSource Ehigh;

    public AudioSource stringSound;

    public float[] pitchAdjustments;

    public AudioAnalyzer audioAnalyzer;

    private GuitarSoundManager guitarSoundManager;
    private int _sampleRate;//44100
    private int sampleSize;//4096
    private float[] _audioData;

    private void Start()
    {
        // Initialize pitch adjustments array
        pitchAdjustments = new float[21];//open string + 20 frets
        for (int i = 0; i < 21; i++)
        {
            pitchAdjustments[i] = Mathf.Pow(2f, i/12f);
        }

        guitarSoundManager = GetComponent<GuitarSoundManager>();

        _sampleRate = 44100;
        sampleSize = 4096;
        _audioData = new float[sampleSize];
    }

    public void PlayFretSound(int fretNum, int strNum)
    {
        float pitchAdjustment = pitchAdjustments[fretNum];
        stringSound = guitarSoundManager.GetStringSound(strNum);
        if (stringSound != null)
        {
            stringSound.pitch = pitchAdjustment;
            stringSound.Play();

            stringSound.clip.GetData(_audioData, 0);

            float[] newData = new float[sampleSize];
            for (int i = 0; i < _audioData.Length; i++)
            {
                int oldIndex = Mathf.RoundToInt(i * pitchAdjustment);
                if (oldIndex >= _audioData.Length) break;
                newData[i] = _audioData[oldIndex];
            }

            audioAnalyzer.Interpret(newData, _sampleRate);
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