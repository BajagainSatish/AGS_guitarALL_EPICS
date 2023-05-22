/*
using System.IO;
using UnityEngine;

public class AudioTesting : MonoBehaviour
{
    public float duration = 1f; // Duration of the bend in seconds
    public float startPitch = 1f; // Starting pitch of the bend
    public float endPitch = 2.5f; // Ending pitch of the bend
    public AudioSource audioSource; // Reference to the Audio Source component

    private float currentPitch; // Current pitch during the bend
    private float timer; // Timer to keep track of the progress of the bend
    private bool verticalDrag;
    private float verticalDisplacement;

    private Vector3 initialMousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.pitch = startPitch;
            audioSource.Play();
            verticalDrag = true;
            initialMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0) && verticalDrag)
        {
            verticalDisplacement = Input.mousePosition.y - initialMousePosition.y;

            if (verticalDisplacement > 5f)
            {
                // If the timer is less than the duration, update the pitch and the timer
                if (timer < duration)
                {
                    // Calculate the new pitch based on the progress of the bend
                    float t = timer / duration;
                    currentPitch = Mathf.Lerp(startPitch, endPitch, t);

                    // Set the pitch of the Audio Source to the new pitch
                    audioSource.pitch = currentPitch;

                    // Increment the timer
                    timer += Time.deltaTime;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (verticalDrag)
            {
                Debug.Log("Vertical displacement: " + verticalDisplacement);
            }
            //Restore to play again
            verticalDrag = false;
            timer = 0;
            currentPitch = startPitch;
        }
    }
}
//This code is practice code before developing guitar and was used in another scene, not used by other gameobjects
*/