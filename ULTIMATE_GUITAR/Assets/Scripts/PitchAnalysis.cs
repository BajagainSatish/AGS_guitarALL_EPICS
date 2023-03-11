using UnityEngine;
using UnityEngine.UI;

public class GuitarNoteComparator : MonoBehaviour
{
    public Text resultText;
    public float desiredPitch;

    private bool isRecording;
    private float[] recordingBuffer;
    private const int recordingDuration = 1; // Recording duration in seconds
    private const int sampleRate = 44100; // Sample rate of the recording

    private void Start()
    {
        // Start recording
        recordingBuffer = new float[recordingDuration * sampleRate];
        Microphone.GetDeviceCaps(null, out var minFreq, out var maxFreq);
        Microphone.Start(null, true, recordingDuration, sampleRate);
        isRecording = true;
    }

    private void Update()
    {
        // Stop recording and detect pitch when recording is finished
        if (isRecording && Microphone.IsRecording(null) == false)
        {
            isRecording = false;
            Microphone.End(null);
            DetectPitch();
        }
    }

    private void DetectPitch()
    {
        // Perform autocorrelation to detect pitch
        var correlationBuffer = new float[recordingBuffer.Length];
        for (int i = 0; i < recordingBuffer.Length; i++)
        {
            for (int j = 0; j < recordingBuffer.Length - i; j++)
            {
                correlationBuffer[i] += recordingBuffer[j] * recordingBuffer[j + i];
            }
        }

        // Find the maximum value in the correlation buffer
        var maxCorrelation = Mathf.Max(correlationBuffer);
        var maxIndex = System.Array.IndexOf(correlationBuffer, maxCorrelation);

        // Calculate the detected pitch
        var detectedPitch = sampleRate / maxIndex;

        // Compare the detected pitch to the desired pitch
        var percentageDifference = Mathf.Abs(detectedPitch - desiredPitch) / desiredPitch * 100;

        // Display the results
        resultText.text = $"Detected pitch: {detectedPitch} Hz\nPercentage difference: {percentageDifference}%";
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        // Copy the recorded audio to the recording buffer
        if (isRecording)
        {
            System.Array.Copy(data, recordingBuffer, recordingBuffer.Length);
        }
    }
}
