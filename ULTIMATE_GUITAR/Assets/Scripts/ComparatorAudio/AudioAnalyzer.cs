using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Rendering;
using System.Linq;
using System.Threading;

public class AudioAnalyzer : MonoBehaviour
{
    private float[] autocorrelation;

    Dictionary<float, string> n = new Dictionary<float, string>()
    {
    {73.42f, "D2"},
    {77.78f, "D#2"},
    {82.41f, "E2"},
    {87.31f, "F2"},
    {92.50f, "F#2"},
    {98.00f, "G2"},
    {103.83f, "G#2"},
    {110.00f, "A2"},
    {116.54f, "A#2"},
    {123.47f, "B2"},
    {130.81f, "C3"},
    {138.59f, "C#3"},
    {146.83f, "D3"},
    {155.56f, "D#3"},
    {164.81f, "E3"},
    {174.61f, "F3"},
    {185.00f, "F#3"},
    {196.00f, "G3"},
    {207.65f, "G#3"},
    {220.00f, "A3"},
    {233.08f, "A#3"},
    {246.94f, "B3"},
    {261.63f, "C4"},
    {277.18f, "C#4"},
    {293.66f, "D4"},
    {311.13f, "D#4"},
    {329.63f, "E4"},
    {349.23f, "F4"},
    {369.99f, "F#4"},
    {392.00f, "G4"},
    {415.30f, "G#4"},
    {440.00f, "A4"},
    {466.16f, "A#4"},
    {493.88f, "B4"},
    {523.25f, "C5"},
    {554.37f, "C#5"},
    {587.33f, "D5"},
    {622.25f, "D#5"},
    {659.25f, "E5"},
    {698.46f, "F5"},
    {739.99f, "F#5"},
    {783.99f, "G5"},
    {830.61f, "G#5"},
    {880.00f, "A5"},
    {932.33f, "A#5"},
    {987.77f, "B5"},//here, 958.69
    {1046.50f,"C6"}
    };

    float[] notes = new float[] { 73.42f, 77.78f, 82.41f, 87.31f, 92.50f, 98.00f, 103.83f, 110.00f, 116.54f, 123.47f, 130.81f, 138.59f, 146.83f, 155.56f, 164.81f, 174.61f, 185.00f, 196.00f, 207.65f, 220.00f, 233.08f, 246.94f, 261.63f, 277.18f, 293.66f, 311.13f, 329.63f, 349.23f, 369.99f, 392.00f, 415.30f, 440.00f, 466.16f, 493.88f, 523.25f, 554.37f, 587.33f, 622.25f, 659.25f, 698.46f, 739.99f, 783.99f, 830.61f, 880.00f, 932.33f, 987.77f, 1046.50f};
    void Start()
    {
        autocorrelation = new float[4096];
    }
    public void Interpret(float[] audioData, int sampleRate)
    {
        float[] ac = AutocorrelationWithShiftingLag(audioData);
        float[] normalized = MaxAbsoluteScaling(ac);
        float freq = GetFreq(normalized, sampleRate);

        float fundamentalFrequency = GetFundamentalFrequency(freq);
        float closestNote = GetClosestNoteFrequency(fundamentalFrequency);
        string note_letter = GetNoteLetter(closestNote);

        Debug.Log("Fundamental Frequency: " + fundamentalFrequency);
        Debug.Log("Closest Note: " + closestNote);
        Debug.Log("Note Letter: " + note_letter);
    }

    //autocorrelation function
    float Rxx(int l, float N, float[] x)
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

    float[] MaxAbsoluteScaling(float[] data)
    {//error possible here
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
            if (currentHarmonic < 1050)
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

        if (frequency == 958.6957f)
        {
            return 987.77f;
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
