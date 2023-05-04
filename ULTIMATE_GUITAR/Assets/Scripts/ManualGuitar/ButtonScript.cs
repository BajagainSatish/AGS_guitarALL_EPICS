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
    public bool isRecordingPressed;
    public string currentMode;

    public GameObject startButton;

    private void Start()
    {
        noneIsPressed = true;
        hammerIsPressed = false;
        pullOffIsPressed = false;
        slideIsPressed = false;
        bendIsPressed = false;
        currentMode = "None";
        isRecordingPressed = false;
    }

    public void OnClickNone()
    {
        noneIsPressed = true;
        hammerIsPressed = false;
        pullOffIsPressed = false;
        slideIsPressed = false;
        bendIsPressed = false;
        currentMode = "None";
    }
    public void OnClickHammer()
    {
        noneIsPressed = false;
        hammerIsPressed = true;
        pullOffIsPressed = false;
        slideIsPressed = false;
        bendIsPressed = false;
        currentMode = "Hammer";
    }
    public void OnClickPullOff()
    {
        noneIsPressed = false;
        hammerIsPressed = false;
        pullOffIsPressed = true;
        slideIsPressed = false;
        bendIsPressed = false;
        currentMode = "PullOff";
    }
    public void OnClickSlide()
    {
        noneIsPressed = false;
        hammerIsPressed = false;
        pullOffIsPressed = false;
        slideIsPressed = true;
        bendIsPressed = false;
        currentMode = "Slide";
    }

    public void OnClickBend()
    {
        noneIsPressed = false;
        hammerIsPressed = false;
        pullOffIsPressed = false;
        slideIsPressed = false;
        bendIsPressed = true;
        currentMode = "Bend";
    }
    public void OnClickStartRecord()
    {
        isRecordingPressed = true;
        startButton.GetComponent<Image>().color = Color.red;
    }
    public void OnClickStopRecord()
    {
        isRecordingPressed = false;
        startButton.GetComponent<Image>().color = Color.white;
    }
}
