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
    {349.23f, "F"},
    {369.99f, "F#"},
    {392.00f, "G"},
    {415.30f, "G#"},
    {440.00f, "A"},
    {466.16f, "A#"},
    {493.88f, "B"},
    {523.25f, "C"},
    {554.37f, "C#"},
    {587.33f, "D"},
    {622.25f, "D#"},
    {659.25f, "E"},
    {698.46f, "F"},
    {739.99f, "F#"},
    {783.99f, "G"},
    {830.61f, "G#"},
    {880.00f, "A"},
    {932.33f, "A#"},
    {987.77f, "B"},//here, 958.69
    {1046.50f,"C"},
    {1108.73f, "C#"},//here,1075.61f
    {1174.66f, "D"},//here,1130.769f
    {1244.51f, "D#"}
    };

    readonly float[] notes = new float[] { 73.42f, 77.78f, 82.41f, 87.31f, 92.50f, 98.00f, 103.83f, 110.00f, 116.54f, 123.47f, 130.81f, 138.59f, 146.83f, 155.56f, 164.81f, 174.61f, 185.00f, 196.00f, 207.65f, 220.00f, 233.08f, 246.94f, 261.63f, 277.18f, 293.66f, 311.13f, 329.63f, 349.23f, 369.99f, 392.00f, 415.30f, 440.00f, 466.16f, 493.88f, 523.25f, 554.37f, 587.33f, 622.25f, 659.25f, 698.46f, 739.99f, 783.99f, 830.61f, 880.00f, 932.33f, 987.77f, 1046.50f, 1108.73f, 1174.66f, 1244.51f};
    void Start()
    {
        autocorrelation = new float[4096];
    }
    public string Interpret(float[] audioData, int sampleRate)
    {
        float[] ac = AutocorrelationWithShiftingLag(audioData);
        float[] normalized = MaxAbsoluteScaling(ac);
        float freq = GetFreq(normalized, sampleRate);

        float fundamentalFrequency = GetFundamentalFrequency(freq);
        float closestNote = GetClosestNoteFrequency(fundamentalFrequency);
        string note_letter = GetNoteLetter(closestNote);

        //Debug.Log("Fundamental Frequency: " + fundamentalFrequency);
        //Debug.Log("Closest Note: " + closestNote);
        Debug.Log("Note Letter: " + note_letter);

        return note_letter;
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
    {
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
            if (currentHarmonic < 1250)
            {
                return currentHarmonic;
            }
        }
        return frequency;
    }

    float GetClosestNoteFrequency(float frequency)//work on this later, maybe frequency just greater than this frequency rather than closest note
    {//Out of 24 * 6 notes, these 3 notes at extremely high frequency are exception as per our work logic of finding closest note. Here, for fret 21 and 22, unlike 19 here we chopped off all values after decimal to compare
        if (frequency == 958.6957f)
        {
            return 987.77f;//B fret 19
        }
        else if ((int)frequency == 1075)
        {
            return 1108.73f;//C# fret 21
        }
        else if ((int)frequency == 1130)
        {
            return 1174.66f;//D fret 22
        }
        else {
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
