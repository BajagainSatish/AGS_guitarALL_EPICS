using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using System.IO;

public class StringClickHandler : MonoBehaviour
{
    public GuitarSoundManager guitarSoundManager;
    public ButtonScript buttonScript;
    private SphereController sphereControllerScript;
    public CapoController capoControllerScript;

    private Vector3 initialMousePosition;
    private bool isDraggingVerticalBend = false;
    private bool isDraggingHorizontal = false;
    private bool isDraggingVerticalStrum = false;
    private float verticalDisplacement, horizontalDisplacement;
    private float tempFret;
    private float tempStr;
    private int fretNum;
    private int strNum;

    public TMP_InputField userInputFingerElow;
    public TMP_InputField userInputFingerA;
    public TMP_InputField userInputFingerD;
    public TMP_InputField userInputFingerG;
    public TMP_InputField userInputFingerB;
    public TMP_InputField userInputFingerEhigh;

    public TMP_InputField userInputCapo;

    private int userCapoNum;
    private int[] userFingerNum;//finger press purpose

    [SerializeField] private ParticleSystem glowEffect;

    private float startPitch;
    private float endPitch;
    public float duration = 1f; // Duration of the bend in seconds, arbitrary value
    private float currentPitch; // Current pitch during the bend
    private float timer; // Timer to keep track of the progress of the bend

    private bool audioPlayed = false;

    [SerializeField] private LayerMask layerMask;

    private void Start()
    {
        sphereControllerScript = GetComponent<SphereController>();
        userFingerNum = new int[]{0,0,0,0,0,0};
        userCapoNum = 0;
    }

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
                    //Debug.Log("String " + strNum);

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
                            //Debug.Log("Fret " + fretNum);

                            //None is pressed, with fret num input
                            if (buttonScript.noneIsPressed)
                            {
                                if (RetFretSpecificString(strNum,userFingerNum) == 0 && userCapoNum == 0)//finger at 0, capo at 0
                                {
                                    PlayGlowEffect(hitAgain.point);//particle effect
                                    guitarSoundManager.PlayFretSound(fretNum, strNum);
                                }
                                else if(RetFretSpecificString(strNum, userFingerNum) < userCapoNum)//finger after capo
                                {
                                    if (fretNum == 0)
                                    {
                                        PlayGlowEffect(hitAgain.point);//particle effect
                                        guitarSoundManager.PlayFretSound(userCapoNum, strNum);//when open played, play finger press fret note
                                    }
                                    if (fretNum > userCapoNum)
                                    {
                                        PlayGlowEffect(hitAgain.point);//particle effect
                                        guitarSoundManager.PlayFretSound(fretNum, strNum);
                                    }
                                }
                                else if (RetFretSpecificString(strNum, userFingerNum) > userCapoNum && RetFretSpecificString(strNum, userFingerNum) <= 23)//finger before capo
                                {
                                    if (fretNum == 0)
                                    {
                                        PlayGlowEffect(hitAgain.point);//particle effect
                                        guitarSoundManager.PlayFretSound(RetFretSpecificString(strNum, userFingerNum), strNum);//when open played, play finger press fret note which is ahead of capo
                                    }
                                    if (fretNum > RetFretSpecificString(strNum, userFingerNum))
                                    {
                                        PlayGlowEffect(hitAgain.point);//particle effect
                                        guitarSoundManager.PlayFretSound(fretNum, strNum);
                                    }                              
                                }
                            }
                            //END none is pressed

                            //Hammer and Pull Off Code Implementation
                            //Hammer logic = play open/finger press sound, then pressed sound on fret
                            if (buttonScript.hammerIsPressed)
                            {
                                if (RetFretSpecificString(strNum, userFingerNum) == 0 && userCapoNum == 0)//finger at 0, capo at 0
                                {
                                    if (fretNum != 0)
                                    {
                                        PlayGlowEffect(hitAgain.point);//particle effect
                                        guitarSoundManager.PlayFretSound(0, strNum);//for now nothing like kepo or finger press so 0
                                                                                    //Now the code inside if (Input.GetMouseButtonUp(0)) will also be executed
                                    }
                                }
                                else if (RetFretSpecificString(strNum, userFingerNum) > userCapoNum && RetFretSpecificString(strNum, userFingerNum) <= 23)//finger ahead capo
                                {
                                    if (fretNum > RetFretSpecificString(strNum, userFingerNum))
                                    {
                                        if (fretNum != 0)
                                        {
                                            PlayGlowEffect(hitAgain.point);//particle effect
                                            guitarSoundManager.PlayFretSound(RetFretSpecificString(strNum, userFingerNum), strNum);//Now the code inside if (Input.GetMouseButtonUp(0)) will also be executed
                                        }
                                    }                                   
                                }
                                else if (userCapoNum > RetFretSpecificString(strNum,userFingerNum))
                                {
                                    if (fretNum > userCapoNum)
                                    {
                                    PlayGlowEffect(hitAgain.point);//particle effect
                                    guitarSoundManager.PlayFretSound(userCapoNum, strNum);//Now the code inside if (Input.GetMouseButtonUp(0)) will also be executed
                                    }
                                }
                            }
                            //PUll off logic = play pressed fret sound, then open/finger press
                            if (buttonScript.pullOffIsPressed)
                            {
                                if (RetFretSpecificString(strNum, userFingerNum) == 0 && userCapoNum == 0)//finger at 0, capo at 0
                                {
                                    if (fretNum != 0)
                                    {
                                        PlayGlowEffect(hitAgain.point);//particle effect
                                        guitarSoundManager.PlayFretSound(fretNum, strNum);//for now nothing like kepo or finger press so 0
                                                                                    //Now the code inside if (Input.GetMouseButtonUp(0)) will also be executed
                                    }
                                }
                                else if (RetFretSpecificString(strNum, userFingerNum) > userCapoNum && RetFretSpecificString(strNum, userFingerNum) <= 23)//finger ahead capo
                                {
                                    if (fretNum > RetFretSpecificString(strNum, userFingerNum))
                                    {
                                        if (fretNum != 0)
                                        {
                                            PlayGlowEffect(hitAgain.point);//particle effect
                                            guitarSoundManager.PlayFretSound(fretNum, strNum);
                                        }
                                    }
                                }
                                else if (userCapoNum > RetFretSpecificString(strNum, userFingerNum))
                                {
                                    if (fretNum > userCapoNum)
                                    {
                                        PlayGlowEffect(hitAgain.point);//particle effect
                                        guitarSoundManager.PlayFretSound(fretNum, strNum);
                                    }
                                }
                                //Now the code inside if (Input.GetMouseButtonUp(0)) will also be executed
                            }
                        }
                        // END Hammer and Pull Off Code Implementation

                        //Sliding Code Implementation
                        if (buttonScript.slideIsPressed)
                        {
                            if (RetFretSpecificString(strNum, userFingerNum) == 0 && userCapoNum == 0)//finger at 0, capo at 0
                            {
                                if (fretNum != 0)
                                {
                                    tempFret = fretNum;
                                    isDraggingHorizontal = true;
                                    initialMousePosition = Input.mousePosition;
                                }
                            }
                            else if (RetFretSpecificString(strNum, userFingerNum) > userCapoNum && RetFretSpecificString(strNum, userFingerNum) <= 23)//finger ahead capo
                            {
                                if (fretNum > RetFretSpecificString(strNum, userFingerNum))
                                {
                                    if (fretNum != 0)
                                    {
                                        tempFret = fretNum;
                                        isDraggingHorizontal = true;
                                        initialMousePosition = Input.mousePosition;
                                    }
                                }
                            }
                            else if (userCapoNum > RetFretSpecificString(strNum, userFingerNum))
                            {
                                if (fretNum > userCapoNum)
                                {
                                    tempFret = fretNum;
                                    isDraggingHorizontal = true;
                                    initialMousePosition = Input.mousePosition;
                                }
                            }
                                //code inside Input.GetMouseButton(0) && isDraggingHorizontal is implemented
                        }
                        //Sliding Code Implementation End

                        //Bend String Code Implementation
                        if (buttonScript.bendIsPressed)
                        {
                            if (RetFretSpecificString(strNum, userFingerNum) == 0 && userCapoNum == 0)//finger at 0, capo at 0
                            {
                                if (fretNum != 0)
                                {
                                    isDraggingVerticalBend = true;
                                    initialMousePosition = Input.mousePosition;
                                    PlayGlowEffect(hitAgain.point);//particle effect
                                    guitarSoundManager.PlayFretSound(fretNum, strNum);
                                }
                            }
                            else if (RetFretSpecificString(strNum, userFingerNum) > userCapoNum && RetFretSpecificString(strNum, userFingerNum) <= 23)//finger ahead capo
                            {
                                if (fretNum > RetFretSpecificString(strNum, userFingerNum))
                                {
                                    if (fretNum != 0)
                                    {
                                        isDraggingVerticalBend = true;
                                        initialMousePosition = Input.mousePosition;
                                        PlayGlowEffect(hitAgain.point);//particle effect
                                        guitarSoundManager.PlayFretSound(fretNum, strNum);
                                    }
                                }
                            }
                            else if (userCapoNum > RetFretSpecificString(strNum, userFingerNum))
                            {
                                if (fretNum > userCapoNum)
                                {
                                    isDraggingVerticalBend = true;
                                    initialMousePosition = Input.mousePosition;
                                    PlayGlowEffect(hitAgain.point);//particle effect
                                    guitarSoundManager.PlayFretSound(fretNum, strNum);
                                }
                            }
                        }
                        //code verticalDisplacement = Input.mousePosition.y - inputPosition.y inside another if block
                        //Bend String Code Implementation
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
        if (buttonScript.strumIsPressed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("String"))
                    {
                        // Handle string click
                        GameObject stringClicked = hit.transform.gameObject;
                        GuitarString guitarStringScript = stringClicked.GetComponent<GuitarString>();
                        strNum = guitarStringScript.Return_String_Number();

                        //change layer of string to Ignore Raycast so that now fret is detected
                        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
                        stringClicked.gameObject.layer = LayerIgnoreRaycast;

                        //now we check strictly if the fret "0" has been clicked or not
                        Ray raySecond = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hitAgain;
                        if (Physics.Raycast(raySecond, out hitAgain, layerMask))
                        {
                            if (hitAgain.collider.CompareTag("Fret"))
                            {
                                //find the fret number
                                GameObject fretClicked = hitAgain.transform.gameObject;
                                FretNum fretNumScript = fretClicked.GetComponent<FretNum>();
                                fretNum = fretNumScript.Return_Fret_Num();

                                if (fretNum == 0)
                                {
                                    isDraggingVerticalStrum = true;
                                    PlayGlowEffect(hitAgain.point);
                                    if (userCapoNum > RetFretSpecificString(strNum,userFingerNum))
                                    {
                                        guitarSoundManager.PlayFretSound(userCapoNum,strNum);
                                    }
                                    else if (userCapoNum < RetFretSpecificString(strNum,userFingerNum))
                                    {
                                        guitarSoundManager.PlayFretSound(RetFretSpecificString(strNum, userFingerNum),strNum);
                                    }
                                    else
                                    {
                                        guitarSoundManager.PlayFretSound(0,strNum);
                                    }
                                }
                            }
                            //now code inside Input.GetMouseButton(0) && isDraggingVerticalStrum is executed
                        }
                    //Restore layer of string gameobject
                    int stringLayer = LayerMask.NameToLayer("String");
                    stringClicked.gameObject.layer = stringLayer;
                    //Debug.Log("Updated layer of string:" + stringClicked.layer);
                    }
                    else if (hit.collider.CompareTag("Fret"))
                    {
                        //find the fret number
                        GameObject fretClicked = hit.transform.gameObject;
                        FretNum fretNumScript = fretClicked.GetComponent<FretNum>();
                        fretNum = fretNumScript.Return_Fret_Num();

                        if (fretNum == 0)
                        {
                            isDraggingVerticalStrum = true;
                            PlayGlowEffect(hit.point);
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButton(0) && isDraggingHorizontal)
        {
            if (fretNum > RetFretSpecificString(strNum, userFingerNum))//kinda sus but still works
            {
            horizontalDisplacement = initialMousePosition.x - Input.mousePosition.x;
            if (horizontalDisplacement >= 10f || horizontalDisplacement <= -10f)
            {
                if (!audioPlayed)
                {
                    Ray raySlide = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit rayCastHitSlide;
                        if (Physics.Raycast(raySlide,out rayCastHitSlide))
                        {
                            PlayGlowEffect(rayCastHitSlide.point);//particle effect
                        }
                    guitarSoundManager.PlayFretSound(fretNum, strNum);
                    audioPlayed = true;
                }
                Ray multipleRays = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(multipleRays, out RaycastHit hitMultipleRays, layerMask))
                {
                    //Debug.Log(hitMultipleRays.collider.gameObject.name);//gives fret name
                    //Debug.DrawRay(multipleRays.origin, multipleRays.direction * hitMultipleRays.distance, Color.red);

                    if (hitMultipleRays.collider.CompareTag("String"))
                    {
                        // Handle string click
                        GameObject stringClicked = hitMultipleRays.transform.gameObject;
                        GuitarString guitarStringScript = stringClicked.GetComponent<GuitarString>();
                        strNum = guitarStringScript.Return_String_Number();
                        //Debug.Log("String " + strNum);

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
                            //Debug.DrawRay(multipleRaysAgain.origin, multipleRaysAgain.direction * multipleRayshitAgain.distance, Color.yellow);
                            if (multipleRayshitAgain.collider.CompareTag("Fret"))
                            {
                                //find the fret number
                                GameObject fretClicked = multipleRayshitAgain.transform.gameObject;
                                FretNum fretNumScript = fretClicked.GetComponent<FretNum>();
                                fretNum = fretNumScript.Return_Fret_Num();
                                //Debug.Log("Fret " + fretNum);
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
                            //Debug.Log(hitMultipleRays2.collider.gameObject.name);//gives fret name
                            //Debug.DrawRay(multipleRays2.origin, multipleRays2.direction * hitMultipleRays2.distance, Color.red);

                            if (hitMultipleRays2.collider.CompareTag("String"))
                            {
                                // Handle string click
                                GameObject stringClicked = hitMultipleRays2.transform.gameObject;
                                GuitarString guitarStringScript = stringClicked.GetComponent<GuitarString>();
                                strNum = guitarStringScript.Return_String_Number();
                                //Debug.Log("String " + strNum);

                                //change layer of string to Ignore Raycast so that now fret is detected
                                //Debug.Log("Initial layer of string:" + stringClicked.gameObject.layer);//initial which is string
                                int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
                                stringClicked.layer = LayerIgnoreRaycast;
                                //Debug.Log("Updated layer of string:" + stringClicked.gameObject.layer);

                                //now we check if the fret has been clicked or not
                                Ray multipleRaysAgain2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                    RaycastHit hitAgain2;
                                if (Physics.Raycast(multipleRaysAgain2, out hitAgain2, layerMask))
                                {
                                    //Debug.Log(hitAgain.collider.gameObject.name);//gives fret name
                                    //Debug.DrawRay(multipleRaysAgain2.origin, multipleRaysAgain2.direction * hitAgain2.distance, Color.yellow);
                                    if (hitAgain2.collider.CompareTag("Fret"))
                                    {
                                        //find the fret number
                                        GameObject fretClicked = hitAgain2.transform.gameObject;
                                        FretNum fretNumScript = fretClicked.GetComponent<FretNum>();
                                        fretNum = fretNumScript.Return_Fret_Num();
                                        //Debug.Log("Fret " + fretNum);
                                        PlayGlowEffect(hitAgain2.point);
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
                                if (RetFretSpecificString(strNum, userFingerNum) == 0 && userCapoNum == 0)//finger at 0, capo at 0
                                {
                                    if (fretNum != 0)
                                    {
                                        guitarSoundManager.PlayFretSound(fretNum, strNum);
                                        audioPlayed = true;
                                    }
                                }
                                else if (RetFretSpecificString(strNum, userFingerNum) > userCapoNum && RetFretSpecificString(strNum, userFingerNum) <= 23)//finger ahead capo
                                {
                                    if (fretNum > RetFretSpecificString(strNum, userFingerNum))
                                    {
                                        if (fretNum != 0)
                                        {
                                            guitarSoundManager.PlayFretSound(fretNum, strNum);
                                            audioPlayed = true;
                                        }
                                    }
                                }
                                else if (userCapoNum > RetFretSpecificString(strNum, userFingerNum))
                                {
                                    if (fretNum > userCapoNum)
                                    {
                                        guitarSoundManager.PlayFretSound(fretNum, strNum);
                                        audioPlayed = true;
                                    }
                                }
                        }
                        tempFret = fretNum;
                    }
                }
            }
            }
        }
        if (Input.GetMouseButton(0) && isDraggingVerticalBend)
        {
            if (RetFretSpecificString(strNum, userFingerNum) == 0 && userCapoNum == 0)//finger at 0, capo at 0
            {
                if (fretNum != 0)
                {
                    verticalDisplacement = Input.mousePosition.y - initialMousePosition.y;
                    if (verticalDisplacement >= 5f || verticalDisplacement <= -5f)//increase to next note
                    {
                        /////OMGGGGGGGGGGG GENIUSSSSS
                        startPitch = Mathf.Pow(2f, fretNum / 12f);
                        endPitch = Mathf.Pow(2f, (fretNum + 2) / 12f);
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
            }
            else if (RetFretSpecificString(strNum, userFingerNum) > userCapoNum && RetFretSpecificString(strNum, userFingerNum) <= 23)//finger ahead capo
            {
                if (fretNum > RetFretSpecificString(strNum, userFingerNum))
                {
                    if (fretNum != 0)
                    {
                        verticalDisplacement = Input.mousePosition.y - initialMousePosition.y;
                        if (verticalDisplacement >= 5f || verticalDisplacement <= -5f)//increase to next note
                        {
                            /////OMGGGGGGGGGGG GENIUSSSSS
                            startPitch = Mathf.Pow(2f, fretNum / 12f);
                            endPitch = Mathf.Pow(2f, (fretNum + 2) / 12f);
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
                }
            }
            else if (userCapoNum > RetFretSpecificString(strNum, userFingerNum))
            {
                if (fretNum > userCapoNum)
                {
                    verticalDisplacement = Input.mousePosition.y - initialMousePosition.y;
                    if (verticalDisplacement >= 5f || verticalDisplacement <= -5f)//increase to next note
                    {
                        /////OMGGGGGGGGGGG GENIUSSSSS
                        startPitch = Mathf.Pow(2f, fretNum / 12f);
                        endPitch = Mathf.Pow(2f, (fretNum + 2) / 12f);
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
            }
        }
        if (Input.GetMouseButton(0) && isDraggingVerticalStrum)
        {
                if (fretNum == 0)
                {
                    Ray multipleRaysStrum = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(multipleRaysStrum, out RaycastHit hitMultipleRays, layerMask))
                    {
                        Debug.DrawRay(multipleRaysStrum.origin, multipleRaysStrum.direction * hitMultipleRays.distance, Color.blue);
                        if (hitMultipleRays.collider.CompareTag("String"))
                        {
                            Debug.DrawRay(multipleRaysStrum.origin, multipleRaysStrum.direction * hitMultipleRays.distance, Color.red);
                            Debug.Log("Hit string during strum");
                            GameObject stringClicked = hitMultipleRays.transform.gameObject;
                            GuitarString guitarStringScript = stringClicked.GetComponent<GuitarString>();
                            strNum = guitarStringScript.Return_String_Number();
                            if (tempStr != strNum)
                            {
                                audioPlayed = false;
                                if (!audioPlayed)
                                {
                                if (userCapoNum > RetFretSpecificString(strNum,userFingerNum))
                                {
                                    guitarSoundManager.PlayFretSound(userCapoNum, strNum);
                                    audioPlayed = true;
                                }
                                else if (userCapoNum < RetFretSpecificString(strNum, userFingerNum))
                                {
                                    guitarSoundManager.PlayFretSound(RetFretSpecificString(strNum, userFingerNum), strNum);
                                    audioPlayed = true;
                                }
                                else
                                {
                                    guitarSoundManager.PlayFretSound(fretNum, strNum);//play open sound
                                    audioPlayed = true;
                                }
                                }
                                tempStr = strNum;
                            }
                        }
                    }
                }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (buttonScript.hammerIsPressed)
            {
                if (RetFretSpecificString(strNum, userFingerNum) == 0 && userCapoNum == 0)//finger at 0, capo at 0
                {
                    if (fretNum != 0)
                    {
                        guitarSoundManager.PlayFretSound(fretNum, strNum);//for now nothing like kepo or finger press so 0                                                                   
                    }
                }
                else if (RetFretSpecificString(strNum, userFingerNum) > userCapoNum && RetFretSpecificString(strNum, userFingerNum) <= 23)//finger ahead capo
                {
                    if (fretNum > RetFretSpecificString(strNum, userFingerNum))
                    {
                        if (fretNum != 0)
                        {
                        guitarSoundManager.PlayFretSound(fretNum, strNum);
                        }
                    }
                }
                else if (userCapoNum > RetFretSpecificString(strNum, userFingerNum))
                {
                    if (fretNum > userCapoNum)
                    {
                        guitarSoundManager.PlayFretSound(fretNum, strNum);
                    }
                }
            }
            if (buttonScript.pullOffIsPressed)
            {
                if (RetFretSpecificString(strNum, userFingerNum) == 0 && userCapoNum == 0)//finger at 0, capo at 0
                {
                    if (fretNum != 0)
                    {
                        guitarSoundManager.PlayFretSound(0, strNum);//for now nothing like kepo or finger press so 0                                                                   
                    }
                }
                else if (RetFretSpecificString(strNum, userFingerNum) > userCapoNum && RetFretSpecificString(strNum, userFingerNum) <= 23)//finger ahead capo
                {
                    if (fretNum > RetFretSpecificString(strNum, userFingerNum))
                    {
                        if (fretNum != 0)
                        {
                            guitarSoundManager.PlayFretSound(RetFretSpecificString(strNum, userFingerNum), strNum);//Now the code inside if (Input.GetMouseButtonUp(0)) will also be executed
                        }
                    }
                }
                else if (userCapoNum > RetFretSpecificString(strNum, userFingerNum))
                {
                    if (fretNum > userCapoNum)
                    {
                        guitarSoundManager.PlayFretSound(userCapoNum, strNum);//Now the code inside if (Input.GetMouseButtonUp(0)) will also be executed
                    }
                }
            }
            if (isDraggingVerticalBend)
            {
                //Debug.Log("Vertical displacement: " + verticalDisplacement);
            }
            if (isDraggingHorizontal)
            {
                //Debug.Log("Horizontal displacement: " + horizontalDisplacement);
            }
            isDraggingVerticalBend = false;
            isDraggingVerticalStrum = false;
            isDraggingHorizontal = false;
            audioPlayed = false;
            tempStr = 0;//0 is arbitrary, just to enable play once mouse click Up (0)
            //Restore to play sound again(due to bend)
            timer = 0;
            currentPitch = fretNum;
        }

        //Code to ensure that at every frame, fingers and kepo are at correct place
        capoControllerScript.MoveCapoOverFret(userCapoNum);
        for (int i = 0; i < 6; i++)
        {
            if (userCapoNum >= userFingerNum[i])//ensure that finger is not at capo position or behind capo
            {
                userFingerNum[i] = 0;
            }
            sphereControllerScript.MoveSphereOverString(i + 1, userFingerNum);
        }

        //Shortcut Keys
        //S = Strum
        if (Input.GetKeyDown(KeyCode.S))
        {
            for (int i = 1; i <= 6; i++)//play all 6 strings sounds, open or capo or finger press
            {
            if (userCapoNum > RetFretSpecificString(i, userFingerNum))
            {
                guitarSoundManager.PlayFretSound(userCapoNum, i);
            }
            else if (userCapoNum < RetFretSpecificString(i, userFingerNum))
            {
                guitarSoundManager.PlayFretSound(RetFretSpecificString(i, userFingerNum), i);
            }
            else
            {
                guitarSoundManager.PlayFretSound(0, i);//play open sound
            }
            }
        }
    }
    private void PlayGlowEffect(Vector3 givenMousePosition)
    {
        glowEffect.transform.position = new Vector3(givenMousePosition.x,givenMousePosition.y,givenMousePosition.z);
        glowEffect.Play();
    }
    private int RetFretSpecificString(int strNum,int[] userFingerNum)
    {
        if (strNum == 1)
        {
            return userFingerNum[0];
        }
        else if (strNum == 2)
        {
            return userFingerNum[1];
        }
        else if (strNum == 3)
        {
            return userFingerNum[2];
        }
        else if (strNum == 4)
        {
            return userFingerNum[3];
        }
        else if (strNum == 5)
        {
            return userFingerNum[4];
        }
        else if (strNum == 6)
        {
            return userFingerNum[5];
        }
        else
        {
            return 0;//unnecessary
        }
    }
    public void FingerNumInputElow()
    {
        userFingerNum[0] = int.Parse(userInputFingerElow.text);
    }
    public void FingerNumInputA()
    {
        userFingerNum[1] = int.Parse(userInputFingerA.text);
    }
    public void FingerNumInputD()
    {
        userFingerNum[2] = int.Parse(userInputFingerD.text);
    }
    public void FingerNumInputG()
    {
        userFingerNum[3] = int.Parse(userInputFingerG.text);
    }
    public void FingerNumInputB()
    {
        userFingerNum[4] = int.Parse(userInputFingerB.text);
    }
    public void FingerNumInputEhigh()
    {
        userFingerNum[5] = int.Parse(userInputFingerEhigh.text);
    }
    public void CapoNumInput()
    {
        userCapoNum = int.Parse(userInputCapo.text);
    }

}

/*
 public class StringClickHandler : MonoBehaviour
{
    public GuitarSoundManager guitarSoundManager;

    private int fretNum;
    private int strNum;

    [SerializeField] private LayerMask layerMask;

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