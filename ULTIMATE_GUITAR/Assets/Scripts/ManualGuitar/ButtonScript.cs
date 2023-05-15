using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonScript : MonoBehaviour
{
    public bool noneIsPressed;
    public bool hammerIsPressed;
    public bool pullOffIsPressed;
    public bool slideIsPressed;
    public bool bendIsPressed;
    public bool strumIsPressed;
    public bool isRecordingPressed;
    public string currentMode;

    public GameObject startButton;
    public GameObject stopButton;
    public GuitarSoundManager guitarSoundManagerScript;

    private void Start()
    {
        noneIsPressed = true;
        hammerIsPressed = false;
        pullOffIsPressed = false;
        slideIsPressed = false;
        bendIsPressed = false;
        strumIsPressed = false;
        currentMode = "None";
        isRecordingPressed = false;
        stopButton.SetActive(false);
    }

    public void OnClickNone()
    {
        noneIsPressed = true;
        hammerIsPressed = false;
        pullOffIsPressed = false;
        slideIsPressed = false;
        bendIsPressed = false;
        strumIsPressed = false;
        currentMode = "None";
    }
    public void OnClickHammer()
    {
        noneIsPressed = false;
        hammerIsPressed = true;
        pullOffIsPressed = false;
        slideIsPressed = false;
        bendIsPressed = false;
        strumIsPressed = false;
        currentMode = "Hammer";
    }
    public void OnClickPullOff()
    {
        noneIsPressed = false;
        hammerIsPressed = false;
        pullOffIsPressed = true;
        slideIsPressed = false;
        bendIsPressed = false;
        strumIsPressed = false;
        currentMode = "PullOff";
    }
    public void OnClickSlide()
    {
        noneIsPressed = false;
        hammerIsPressed = false;
        pullOffIsPressed = false;
        slideIsPressed = true;
        bendIsPressed = false;
        strumIsPressed = false;
        currentMode = "Slide";
    }

    public void OnClickBend()
    {
        noneIsPressed = false;
        hammerIsPressed = false;
        pullOffIsPressed = false;
        slideIsPressed = false;
        bendIsPressed = true;
        strumIsPressed = false;
        currentMode = "Bend";
    }

    public void OnClickStrum()
    {
        noneIsPressed = false;
        hammerIsPressed = false;
        pullOffIsPressed = false;
        slideIsPressed = false;
        bendIsPressed = false;
        strumIsPressed = true;
        currentMode = "Strum";
    }

    public void OnClickStartRecord()
    {
        isRecordingPressed = true;
        startButton.GetComponent<Image>().color = Color.red;
        stopButton.SetActive(true);
        guitarSoundManagerScript.storeData.Clear();
    }
    public void OnClickStopRecord()
    {
        startButton.GetComponent<Image>().color = Color.white;
        isRecordingPressed = false;
        stopButton.SetActive(false);
        JsonCompare.DisplayNotes();
        guitarSoundManagerScript.similarityOutputText.text = "Note Similarity: " + JsonCompare.similarityNote + " %, Mode Similarity: " + JsonCompare.similarityMode + "%.";
        guitarSoundManagerScript.similarityOutputText.gameObject.SetActive(true);
    }
}
