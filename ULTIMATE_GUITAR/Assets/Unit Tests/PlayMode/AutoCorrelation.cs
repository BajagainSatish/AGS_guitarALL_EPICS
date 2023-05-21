using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using TMPro;

public class AutoCorrelation
{
    //This is in play mode instead of edit mode, because we have to initialize array pitchAdjustments, which is done in Start() method of GuitarSoundManager Script.
    private GameObject audioSourceObject;
    private AudioSource audioSourceElow;
    private AudioSource audioSourceA;
    private AudioSource audioSourceD;
    private AudioSource audioSourceG;
    private AudioSource audioSourceB;
    private AudioSource audioSourceEhigh;
    private AudioClip soundClipElow;
    private AudioClip soundClipA;
    private AudioClip soundClipD;
    private AudioClip soundClipG;
    private AudioClip soundClipB;
    private AudioClip soundClipEhigh;

    [SetUp]
    public void SetUp()
    {
        // Create a temporary GameObject for the AudioSource
        audioSourceObject = new GameObject("TemporaryAudioSource");
        audioSourceElow = audioSourceObject.AddComponent<AudioSource>();
        audioSourceA = audioSourceObject.AddComponent<AudioSource>();
        audioSourceD = audioSourceObject.AddComponent<AudioSource>();
        audioSourceG = audioSourceObject.AddComponent<AudioSource>();
        audioSourceB = audioSourceObject.AddComponent<AudioSource>();
        audioSourceEhigh = audioSourceObject.AddComponent<AudioSource>();
        audioSourceObject.AddComponent<AudioListener>();

        // Load the sound clip from the Resources folder
        soundClipElow = Resources.Load<AudioClip>("Elow");
        soundClipA = Resources.Load<AudioClip>("A");
        soundClipD = Resources.Load<AudioClip>("D");
        soundClipG = Resources.Load<AudioClip>("G");
        soundClipB = Resources.Load<AudioClip>("B");
        soundClipEhigh = Resources.Load<AudioClip>("Ehigh");

        // Assign the sound clip to the AudioSource
        audioSourceElow.clip = soundClipElow;
        audioSourceA.clip = soundClipA;
        audioSourceD.clip = soundClipD;
        audioSourceG.clip = soundClipG;
        audioSourceB.clip = soundClipB;
        audioSourceEhigh.clip = soundClipEhigh;
    }

    [UnityTest]
    public IEnumerator AutocorrelationVerification_0Fret1String_E()
    {
        AutocorrelationVerification(0,1,"E");//0 fret of 1st string(Elow) is E
        TearDown();
        yield return null;
    }

    [UnityTest]
    public IEnumerator AutocorrelationVerification_0Fret3String_D()
    {
        AutocorrelationVerification(0, 3, "D");//0 fret of 3rd string(D) is D
        TearDown();
        yield return null;
    }

    [UnityTest]
    public IEnumerator AutocorrelationVerification_0Fret5String_B()
    {
        AutocorrelationVerification(0, 5, "B");//0 fret of 5th string(B) is B
        TearDown();
        yield return null;
    }

    [UnityTest]
    public IEnumerator AutocorrelationVerification_5Fret2String_D()
    {
        AutocorrelationVerification(5, 2, "D");//5th fret of 2nd string(A) is D
        TearDown();
        yield return null;
    }

    [UnityTest]
    public IEnumerator AutocorrelationVerification_9Fret4String_E()
    {
        AutocorrelationVerification(9, 4, "E");//9th fret of 4th string(G) is E
        TearDown();
        yield return null;
    }

    [UnityTest]
    public IEnumerator AutocorrelationVerification_14Fret6String_FSharp()
    {
        AutocorrelationVerification(14, 6, "F#");//14 fret of 6th string(Ehigh) is F#
        TearDown();
        yield return null;
    }

    [UnityTest]
    public IEnumerator AutocorrelationVerification_16Fret4String_B()
    {
        AutocorrelationVerification(16, 4, "B");//16 fret of 4th string(G) is B
        TearDown();
        yield return null;
    }

    [UnityTest]
    public IEnumerator AutocorrelationVerification_6Fret3String_GSharp()
    {
        AutocorrelationVerification(6, 3, "G#");//6 fret of 3rd string(D) is G#
        TearDown();
        yield return null;
    }

    [UnityTest]
    public IEnumerator AutocorrelationVerification_7Fret4String_D()
    {
        AutocorrelationVerification(7, 4, "D");//7 fret of 4th string(G) is D#
        TearDown();
        yield return null;
    }

    [UnityTest]
    public IEnumerator AutocorrelationVerification_9Fret1String_CSharp()
    {
        AutocorrelationVerification(9, 1, "C#");//9 fret of 1ST string(B) is C#
        TearDown();
        yield return null;
    }

    [UnityTest]
    public IEnumerator AutocorrelationVerification_2Fret5String_CSharp()
    {
        AutocorrelationVerification(2, 5, "C#");//14 fret of 6th string(Ehigh) is F#
        TearDown();
        yield return null;
    }
    private void AutocorrelationVerification(int fretNumber, int stringNumber, string note)
    {
        GameObject testObject = new GameObject();
        GameObject textMeshProObject = new GameObject();
        GameObject buttonScriptObject = new GameObject();
        GameObject stopButton = new GameObject();

        TextMeshProUGUI textMeshProUGUI = textMeshProObject.AddComponent<TextMeshProUGUI>();
        ButtonScript buttonScript = buttonScriptObject.AddComponent<ButtonScript>();
        GuitarSoundManager testScript = testObject.AddComponent<GuitarSoundManager>();

        testScript.similarityOutputText = textMeshProUGUI;
        buttonScript.isRecordingPressed = false;
        testScript.buttonScript = buttonScript;
        testScript.buttonScript.stopButton = stopButton;

        testScript.Elow = audioSourceElow;
        testScript.A = audioSourceA;
        testScript.D = audioSourceD;
        testScript.G = audioSourceG;
        testScript.B = audioSourceB;
        testScript.Ehigh = audioSourceEhigh;

        testScript.Start(); // Initialize pitchAdjustments array
        testScript.isUnitTesting = true;
        testScript.PlayFretSound(fretNumber, stringNumber);
        Assert.AreEqual(note, testScript.note);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up the temporary GameObject
        Object.DestroyImmediate(audioSourceObject);
    }
}
