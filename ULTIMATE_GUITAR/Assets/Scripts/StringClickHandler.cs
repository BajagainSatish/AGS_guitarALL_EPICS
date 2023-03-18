using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringClickHandler : MonoBehaviour
{
    public GuitarSoundManager guitarSoundManager;
    public ButtonScript buttonScript;

    private int fretNum;
    private int strNum;

    private float startTime;
    private float endTime;

    [SerializeField] private LayerMask layerMask;

    //There is one main bug,when mouse cursor moves from one string to another without passing through fret, then value at previous fret will be played and not of current fret
    private void Update()
    {
            if (Input.GetMouseButtonDown(0))
            { // Check for left mouse button click
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    //Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
                    if (hit.collider.CompareTag("String"))
                    {
                        // Handle string click
                        GameObject stringClicked = hit.transform.gameObject;
                        GuitarString guitarStringScript = stringClicked.GetComponent<GuitarString>();
                        strNum = guitarStringScript.Return_String_Number();
                        Debug.Log("String " + strNum);

                    //change layer of string to Ignore Raycast so that now fret is detected
                        //Debug.Log("Initial layer of string:" + stringClicked.gameObject.layer);//initial which is string
                        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
                        stringClicked.gameObject.layer = LayerIgnoreRaycast;
                        //Debug.Log("Updated layer of string:" + stringClicked.gameObject.layer);

                        //now we check if the fret has been clicked or not
                        Ray raySecond = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hitAgain;
                        if (Physics.Raycast(raySecond, out hitAgain, layerMask))
                        {
                            //Debug.Log(hitAgain.collider.gameObject.name);//gives fret name
                            //Debug.DrawRay(raySecond.origin, raySecond.direction * hitAgain.distance, Color.yellow);
                            if (hitAgain.collider.CompareTag("Fret"))
                            {
                                //find the fret number
                                GameObject fretClicked = hitAgain.transform.gameObject;
                                FretNum fretNumScript = fretClicked.GetComponent<FretNum>();
                                fretNum = fretNumScript.Return_Fret_Num();
                                Debug.Log("Fret " + fretNum);

                            //Hammer logic = First play open string sound(no finger or kepo press for now)
                            if (buttonScript.hammerIsPressed)
                            {
                                if (fretNum!=0)//no hammer for open string
                                {
                                    startTime = Time.time;
                                    guitarSoundManager.PlayFretSound(0, strNum);//for now nothing like kepo or finger press so 0
                                    //Now the code inside if (Input.GetMouseButtonUp(0)) will also be executed
                                }
                                else if (fretNum == 0)
                                {
                                    guitarSoundManager.PlayFretSound(fretNum, strNum);//just normal
                                }
                            }


                            //Play the sound
                            if (buttonScript.noneIsPressed)
                            {
                                guitarSoundManager.PlayFretSound(fretNum, strNum);
                            }

                            //PUll off logic = play openstring sound for now immediately after sound play
                            if (buttonScript.pullOffIsPressed)
                            {
                                if (fretNum != 0)//no pull off for open string
                                {
                                    startTime = Time.time;
                                    guitarSoundManager.PlayFretSound(fretNum, strNum);
                                    //Now the code inside if (Input.GetMouseButtonUp(0)) will also be executed
                                }
                                else if (fretNum == 0)
                                {
                                    guitarSoundManager.PlayFretSound(fretNum, strNum);//just normal
                                }
                            }
                            }
                        }

                        //Restore layer of string gameobject
                        int stringLayer = LayerMask.NameToLayer("String");
                        stringClicked.gameObject.layer = stringLayer;
                        //Debug.Log("Updated layer of string:" + stringClicked.layer);
                    }
                else
                {
                    strNum = 0;
                    fretNum = 0;
                }
                }
            }
        if (Input.GetMouseButtonUp(0))
        {
            if (buttonScript.hammerIsPressed)
            {
                if (fretNum != 0)//no hammer for open string
                {
                    guitarSoundManager.PlayFretSound(fretNum, strNum);
                }               
            }
            if (buttonScript.pullOffIsPressed)
            {
                if (fretNum != 0)//no pull off for open string
                {
                    guitarSoundManager.PlayFretSound(0, strNum);
                }
            }
        }
    }
}

/*
 public class StringClickHandler : MonoBehaviour
{
    public GuitarSoundManager guitarSoundManager;

    private int fretNum;
    private int strNum;

    [SerializeField] private LayerMask layerMask;

    //There is one main bug,when mouse cursor moves from one string to another without passing through fret, then value at previous fret will be played and not of current fret
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        { // Check for left mouse button click
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.collider.gameObject.name);
                //Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
                if (hit.collider.CompareTag("String"))
                {
                    // Handle string click
                    GameObject stringClicked = hit.transform.gameObject;
                    GuitarString guitarStringScript = stringClicked.GetComponent<GuitarString>();
                    strNum = guitarStringScript.Return_String_Number();
                    Debug.Log("String " + strNum);

                    //change layer of string to Ignore Raycast so that now fret is detected
                    Debug.Log("Initial layer of string:"+ stringClicked.gameObject.layer);//initial which is string
                    int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
                    stringClicked.gameObject.layer = LayerIgnoreRaycast;
                    Debug.Log("Updated layer of string:" + stringClicked.gameObject.layer);

                    //now we check if the fret has been clicked or not
                    Ray raySecond = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitAgain; 
                    if (Physics.Raycast(raySecond, out hitAgain,layerMask))
                    {
                        //Debug.Log(hitAgain.collider.gameObject.name);//gives fret name
                        //Debug.DrawRay(raySecond.origin, raySecond.direction * hitAgain.distance, Color.yellow);
                        if (hitAgain.collider.CompareTag("Fret"))
                        {
                            //find the fret number
                            GameObject fretClicked = hitAgain.transform.gameObject;
                            FretNum fretNumScript = fretClicked.GetComponent<FretNum>();
                            fretNum = fretNumScript.Return_Fret_Num();
                            Debug.Log("Fret " + fretNum);
                            
                            //Play the sound
                            guitarSoundManager.PlayFretSound(fretNum, strNum);
                        }
                    }

                    //Restore layer of string gameobject
                    int stringLayer = LayerMask.NameToLayer("String");
                    stringClicked.gameObject.layer = stringLayer;
                    Debug.Log("Updated layer of string:" + stringClicked.layer);
                }
            }
        }
    }
}
 */