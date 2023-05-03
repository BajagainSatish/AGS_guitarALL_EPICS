using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Rendering;
using System.Linq;
using System.Threading;

public class FundamentalFrequencyDetector : MonoBehaviour
{
    private AudioSource audioSource;
    private float[] _audioData;
    private float[] autocorrelation;
    private int sampleSize;//4096
    private int _sampleRate;//44100

    Dictionary<float, string> n = new Dictionary<float, string>()
{
    {73.42f, "D"},
    {77.78f, "D#"},
    {82.41f, "E"},
    {87.31f, "F"},
    {92.50f, "F#"},
    {98.00f, "G"},
    {103.83f, "G#"},
    {110.00f, "A"},
    {116.54f, "A#"},
    {123.47f, "B"},
    {130.81f, "C"},
    {138.59f, "C#"},
    {146.83f, "D"},
    {155.56f, "D#"},
    {164.81f, "E"},
    {174.61f, "F"},
    {185.00f, "F#"},
    {196.00f, "G"},
    {207.65f, "G#"},
    {220.00f, "A"},
    {233.08f, "A#"},
    {246.94f, "B"},
    {261.63f, "C"},
    {277.18f, "C#"},
    {293.66f, "D"},
    {311.13f, "D#"},
    {329.63f, "E"},
    {349.23f, "F"}
};

    float[] notes = new float[] {73.42f, 77.78f, 82.41f, 87.31f, 92.50f, 98.00f, 103.83f, 110.00f, 116.54f, 123.47f, 130.81f, 138.59f, 146.83f, 155.56f, 164.81f, 174.61f, 185.00f, 196.00f, 207.75f, 220.00f, 233.08f, 246.94f, 261.63f, 277.18f, 293.66f, 311.13f, 329.63f, 349.23f};
    void Start()
    {
        sampleSize = 4096;
        _sampleRate = 44100;
        _audioData = new float[sampleSize];
        autocorrelation = new float[sampleSize];
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Get the audio data from the audio clip and fill the array named audioData with that data
        if (Input.GetKeyDown(KeyCode.P))
        {
            audioSource.Play();
            audioSource.clip.GetData(_audioData, 0);
            Interpret(_audioData, _sampleRate);
        }
    }

    private void Interpret(float[] audioData, int sampleRate)
    {
        float[] ac = AutocorrelationWithShiftingLag(audioData);
        float[] normalized = MaxAbsoluteScaling(ac);
        float freq = GetFreq(normalized,sampleRate);

        float fundamentalFrequency = GetFundamentalFrequency(freq);
        float closestNote= GetClosestNoteFrequency(fundamentalFrequency);
        string note_letter = GetNoteLetter(closestNote);

        Debug.Log("Fundamental Frequency: " + fundamentalFrequency);
        Debug.Log("Closest Note: " + closestNote);
        Debug.Log("Note Letter: " + note_letter);
    }

    //autocorrelation function
    float Rxx(int l,float N,float[] x)
    {
        float sum = 0;
        for (var n = 0; n <= N - l - 1; n++)
        {
            sum += (x[n] * x[n + l]);
        }
        return sum;
    }

    float[] AutocorrelationWithShiftingLag(float[] samples)
    {
        for (int lag = 0; lag < samples.Length; lag++)
        {
            autocorrelation[lag] = Rxx(lag, samples.Length, samples);
        }
        return autocorrelation;
    }

    float[] MaxAbsoluteScaling(float[] data) {//error possible here
        float xMax = Mathf.Abs(data.Max());
        return data.Select(x => x / xMax).ToArray();
    }

    //peak detection
    float GetFreq(float[] autocorrelation, int sampleRate)
    {
        float sum = 0;
        int pd_state = 0;
        int period = 0;
        float thresh = autocorrelation[0] * 0.5f;//error possible here thresh

        for (int i = 0; i < autocorrelation.Length; i++)
        {
            float sum_old = sum;
            sum = autocorrelation[i];
            if (pd_state == 2 && sum - sum_old <= 0)
            {
                period = i;
                pd_state = 3;
            }
            if (pd_state == 1 && sum > thresh && sum - sum_old > 0)
            {
                pd_state = 2;
            }
            if (i == 0)
            {
                pd_state = 1;
            }
        }
        float frequency = sampleRate / (float)period;
        return frequency;
    }

    float GetFundamentalFrequency(float frequency)
    {
        for (int i = 1; i <= 5; i++)
        {
            float currentHarmonic = frequency / i;
            if (currentHarmonic < 350)
            {
                return currentHarmonic;
            }
        }
        return frequency;
    }

    float GetClosestNoteFrequency(float frequency)
    {
        float smallestDifference = float.MaxValue;
        float closestNote = float.MaxValue;

        for (int i = 0; i < notes.Length; i++)
        {
            float difference = Mathf.Abs(frequency - (float)notes[i]);

            if (difference < smallestDifference)
            {
                smallestDifference = difference;
                closestNote = (float)notes[i];
            }
        }

        return closestNote;
    }
    string GetNoteLetter(float frequency)
    {
        if (n.ContainsKey(frequency))
        {
            return n[frequency];
        }
        else
        {
            return null;
        }
    }
}
//This is a test script and not used in actual Guitar Scene
