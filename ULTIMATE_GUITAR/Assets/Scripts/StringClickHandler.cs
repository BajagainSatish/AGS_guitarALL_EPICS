using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringClickHandler : MonoBehaviour
{
    public GuitarSoundManager guitarSoundManager;
    public int fretNum;

    private int strNum;

    private int layerNumber = 6;
    private LayerMask layerMask;
    private RaycastHit raycastHit;

    //There is one main bug,when mouse cursor moves from one string to another without passing through fret, then value at previous fret will be played and not of current fret

    private void Start()
    {
        layerMask = 1 << layerNumber;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        { // Check for left mouse button click
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("String"))
                {
                    // Handle string click
                    GameObject stringClicked = hit.transform.gameObject;
                    GuitarString guitarStringScript = stringClicked.GetComponent<GuitarString>();
                    strNum = guitarStringScript.Return_String_Number();
                    Debug.Log("String " + strNum);


                    //now we check if the fret has been clicked or not
                    Ray raySecond = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Debug.Log("hit fret dattebayo");
                    if (Physics.Raycast(raySecond, out raycastHit,Mathf.Infinity, layerMask))
                    {
                        if (raycastHit.collider.CompareTag("Fret"))
                        {
                            //find the fret number
                            if (fretNum != 0)
                            {
                                Debug.Log("Hovering over fret " + fretNum);
                            }
                            if (fretNum == 0)
                            {
                                Debug.Log("Open String Sounds");
                            }

                            //Play the sound
                            guitarSoundManager.PlayFretSound(fretNum, strNum);
                        }
                    }
                }
            }
        }
    }
    
}