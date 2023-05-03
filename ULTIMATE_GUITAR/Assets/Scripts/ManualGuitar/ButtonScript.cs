using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public bool noneIsPressed;
    public bool hammerIsPressed;
    public bool pullOffIsPressed;
    public bool slideIsPressed;
    public bool bendIsPressed;
    private void Start()
    {
        noneIsPressed = true;
        hammerIsPressed = false;
        pullOffIsPressed = false;
        slideIsPressed = false;
        bendIsPressed = false;
    }

    public void OnClickNone()
    {
        noneIsPressed = true;
        hammerIsPressed = false;
        pullOffIsPressed = false;
        slideIsPressed = false;
        bendIsPressed = false;
    }
    public void OnClickHammer()
    {
        noneIsPressed = false;
        hammerIsPressed = true;
        pullOffIsPressed = false;
        slideIsPressed = false;
        bendIsPressed = false;
    }
    public void OnClickPullOff()
    {
        noneIsPressed = false;
        hammerIsPressed = false;
        pullOffIsPressed = true;
        slideIsPressed = false;
        bendIsPressed = false;
    }
    public void OnClickSlide()
    {
        noneIsPressed = false;
        hammerIsPressed = false;
        pullOffIsPressed = false;
        slideIsPressed = true;
        bendIsPressed = false;
    }

    public void OnClickBend()
    {
        noneIsPressed = false;
        hammerIsPressed = false;
        pullOffIsPressed = false;
        slideIsPressed = false;
        bendIsPressed = true;
    }
}
