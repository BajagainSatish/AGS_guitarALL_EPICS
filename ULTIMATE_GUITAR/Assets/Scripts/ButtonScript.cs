using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public bool noneIsPressed;
    public bool hammerIsPressed;
    public bool pullOffIsPressed;

    private void Start()
    {
        noneIsPressed = true;
        hammerIsPressed = false;
        pullOffIsPressed = false;
    }

    public void OnClickNone()
    {
        noneIsPressed = true;
        hammerIsPressed = false;
        pullOffIsPressed = false;
    }
    public void OnClickHammer()
    {
        noneIsPressed = false;
        hammerIsPressed = true;
        pullOffIsPressed = false;
    }
    public void OnClickPullOff()
    {
        noneIsPressed = false;
        hammerIsPressed = false;
        pullOffIsPressed = true;
    }
}
