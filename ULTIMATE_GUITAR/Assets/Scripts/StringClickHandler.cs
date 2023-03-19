using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class StringClickHandler : MonoBehaviour
{
    public GuitarSoundManager guitarSoundManager;
    public ButtonScript buttonScript;

    private Vector3 initialMousePosition;
    private bool isDraggingVertical = false;
    private bool isDraggingHorizontal = false;
    private float verticalDisplacement, horizontalDisplacement;
    private float tempFret;
    private int fretNum;
    private int strNum;

    private float startPitch;
    private float endPitch;
    public float duration = 1f; // Duration of the bend in seconds, arbitrary value
    private float currentPitch; // Current pitch during the bend
    private float timer; // Timer to keep track of the progress of the bend

    private bool audioPlayed = false;

    [SerializeField] private LayerMask layerMask;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // Check for left mouse button click
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit; //inlined

            if (Physics.Raycast(ray, out RaycastHit hit))
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
                        Debug.DrawRay(raySecond.origin, raySecond.direction * hitAgain.distance, Color.yellow);
                        if (hitAgain.collider.CompareTag("Fret"))
                        {
                            //find the fret number
                            GameObject fretClicked = hitAgain.transform.gameObject;
                            FretNum fretNumScript = fretClicked.GetComponent<FretNum>();
                            fretNum = fretNumScript.Return_Fret_Num();
                            Debug.Log("Fret " + fretNum);

                            //Bend String Code Implementation
                            if (buttonScript.bendIsPressed)
                            {
                                //better approach, actual bend
                                isDraggingVertical = true;
                                initialMousePosition = Input.mousePosition;
                                guitarSoundManager.PlayFretSound(fretNum, strNum);//played just once here
                                //now code inside if(INput.getmousebutton(0) && isDraggingVertical) is executed
                            }
                            //code verticalDisplacement = Input.mousePosition.y - inputPosition.y inside another if block
                            //Bend String Code Implementation


                            //Hammer and Pull Off Code Implementation
                            //Hammer logic = First play open string sound(no finger or kepo press for now)
                            if (buttonScript.hammerIsPressed)
                            {
                                if (fretNum != 0)//no hammer for open string
                                {
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
                                    guitarSoundManager.PlayFretSound(fretNum, strNum);
                                    //Now the code inside if (Input.GetMouseButtonUp(0)) will also be executed
                                }
                                else if (fretNum == 0)
                                {
                                    guitarSoundManager.PlayFretSound(fretNum, strNum);//just normal
                                }
                            }
                        }
                        //Hammer and Pull Off Code Implementation END

                        //Sliding Code Implementation

                        if (buttonScript.slideIsPressed)
                        {
                            tempFret = fretNum;
                            isDraggingHorizontal = true;
                            initialMousePosition = Input.mousePosition;
                            //code inside Input.GetMouseButton(0) && isDraggingHorizontal is implemented
                        }
                        //Sliding Code Implementation End

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
        if (Input.GetMouseButton(0) && isDraggingVertical)
        {
            verticalDisplacement = Input.mousePosition.y - initialMousePosition.y;
            /* BEND APPROACH 1, SIMILAR TO SLIDE EFFECT
            if ((verticalDisplacement >= 5f && verticalDisplacement < 25f) || (verticalDisplacement <= -5f && verticalDisplacement >= -25f))//increase to next note
            {
                if (!audioPlayed)
                {
                    guitarSoundManager.PlayFretSound(fretNum + 1, strNum);
                    audioPlayed = true;
                }
            }
            if ((verticalDisplacement >= 25f) || (verticalDisplacement <= -25f))//increase to next note
            {
                if (!audioPlayed)
                {
                    guitarSoundManager.PlayFretSound(fretNum + 2, strNum);
                    audioPlayed = true;
                }
            }
            */
            //APPROACH 2
            if (verticalDisplacement >= 5f || verticalDisplacement <= -5f)//increase to next note
            {
                /////OMGGGGGGGGGGG GENIUSSSSS
                startPitch = Mathf.Pow(2f, fretNum / 12f);
                endPitch = Mathf.Pow(2f, (fretNum+3) / 12f);
                if (timer < duration)
                {
                    // Calculate the new pitch based on the progress of the bend
                    float t = timer / duration;
                    currentPitch = Mathf.Lerp(startPitch, endPitch, t);

                    // Set the pitch of the Audio Source to the new pitch
                    guitarSoundManager.stringSound.pitch = currentPitch;

                    // Increment the timer
                    timer += Time.deltaTime;
                }
            }

        }
        if (Input.GetMouseButton(0) && isDraggingHorizontal)
        {
            horizontalDisplacement = initialMousePosition.x - Input.mousePosition.x;
            if (horizontalDisplacement >= 10f || horizontalDisplacement <= -10f)
            {
                if (!audioPlayed)
                {
                    guitarSoundManager.PlayFretSound(fretNum, strNum);
                    audioPlayed = true;
                }

                Ray multipleRays = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(multipleRays, out RaycastHit hitMultipleRays, layerMask))
                {
                    Debug.Log(hitMultipleRays.collider.gameObject.name);//gives fret name
                    Debug.DrawRay(multipleRays.origin, multipleRays.direction * hitMultipleRays.distance, Color.red);

                    if (hitMultipleRays.collider.CompareTag("String"))
                    {
                        // Handle string click
                        GameObject stringClicked = hitMultipleRays.transform.gameObject;
                        GuitarString guitarStringScript = stringClicked.GetComponent<GuitarString>();
                        strNum = guitarStringScript.Return_String_Number();
                        Debug.Log("String " + strNum);

                        //change layer of string to Ignore Raycast so that now fret is detected
                        //Debug.Log("Initial layer of string:" + stringClicked.gameObject.layer);//initial which is string
                        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
                        stringClicked.layer = LayerIgnoreRaycast;
                        //Debug.Log("Updated layer of string:" + stringClicked.gameObject.layer);

                        //now we check if the fret has been clicked or not
                        Ray multipleRaysAgain = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(multipleRaysAgain, out RaycastHit multipleRayshitAgain, layerMask))
                        {
                            //Debug.Log(hitAgain.collider.gameObject.name);//gives fret name
                            Debug.DrawRay(multipleRaysAgain.origin, multipleRaysAgain.direction * multipleRayshitAgain.distance, Color.yellow);
                            if (multipleRayshitAgain.collider.CompareTag("Fret"))
                            {
                                //find the fret number
                                GameObject fretClicked = multipleRayshitAgain.transform.gameObject;
                                FretNum fretNumScript = fretClicked.GetComponent<FretNum>();
                                fretNum = fretNumScript.Return_Fret_Num();
                                Debug.Log("Fret " + fretNum);
                            }
                        }

                        //Restore layer of string gameobject
                        int stringLayer = LayerMask.NameToLayer("String");
                        stringClicked.layer = stringLayer;
                        //Debug.Log("Updated layer of string:" + stringClicked.layer);
                    }

                    //check if fretNum has changed(increased or decreased)
                    if (tempFret != fretNum)
                    {
                        Ray multipleRays2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(multipleRays2, out RaycastHit hitMultipleRays2, layerMask))
                        {
                            Debug.Log(hitMultipleRays2.collider.gameObject.name);//gives fret name
                            Debug.DrawRay(multipleRays2.origin, multipleRays2.direction * hitMultipleRays2.distance, Color.red);

                            if (hitMultipleRays2.collider.CompareTag("String"))
                            {
                                // Handle string click
                                GameObject stringClicked = hitMultipleRays2.transform.gameObject;
                                GuitarString guitarStringScript = stringClicked.GetComponent<GuitarString>();
                                strNum = guitarStringScript.Return_String_Number();
                                Debug.Log("String " + strNum);

                                //change layer of string to Ignore Raycast so that now fret is detected
                                //Debug.Log("Initial layer of string:" + stringClicked.gameObject.layer);//initial which is string
                                int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
                                stringClicked.layer = LayerIgnoreRaycast;
                                //Debug.Log("Updated layer of string:" + stringClicked.gameObject.layer);

                                //now we check if the fret has been clicked or not
                                Ray multipleRaysAgain2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                if (Physics.Raycast(multipleRaysAgain2, out RaycastHit multipleRayshitAgain2, layerMask))
                                {
                                    //Debug.Log(hitAgain.collider.gameObject.name);//gives fret name
                                    Debug.DrawRay(multipleRaysAgain2.origin, multipleRaysAgain2.direction * multipleRayshitAgain2.distance, Color.yellow);
                                    if (multipleRayshitAgain2.collider.CompareTag("Fret"))
                                    {
                                        //find the fret number
                                        GameObject fretClicked = multipleRayshitAgain2.transform.gameObject;
                                        FretNum fretNumScript = fretClicked.GetComponent<FretNum>();
                                        fretNum = fretNumScript.Return_Fret_Num();
                                        Debug.Log("Fret " + fretNum);
                                    }
                                }
                                //Restore layer of string gameobject
                                int stringLayer = LayerMask.NameToLayer("String");
                                stringClicked.layer = stringLayer;
                                //Debug.Log("Updated layer of string:" + stringClicked.layer);
                            }
                        }

                        audioPlayed = false;
                        if (!audioPlayed)
                        {
                            guitarSoundManager.PlayFretSound(fretNum, strNum);
                            audioPlayed = true;
                        }
                        tempFret = fretNum;
                    }
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
            if (isDraggingVertical)
            {
                Debug.Log("Vertical displacement: " + verticalDisplacement);
            }

            if (isDraggingHorizontal)
            {
                Debug.Log("Horizontal displacement: " + horizontalDisplacement);
            }
            isDraggingVertical = false;
            isDraggingHorizontal = false;
            audioPlayed = false;

            //Restore to play sound again(due to bend)
            timer = 0;
            currentPitch = fretNum;
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