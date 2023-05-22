using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using TMPro;
public class InputValidityTests
{
    [Test]
    [TestCase(-5)]
    [TestCase(17)]
    [TestCase(97)]
    public void MoveCapoOverFret_InvalidInput_SetActiveErrorMessageTMP(int invalidNum)
    {
        GameObject testObject = new GameObject();
        GameObject capoObject = new GameObject();
        TextMeshProUGUI errorDisplayText = testObject.AddComponent<TextMeshProUGUI>();
        CapoController testScript = testObject.AddComponent<CapoController>();

        // Set up errorDisplayText
        testScript.errorDisplayText = errorDisplayText;
        testScript.capo = capoObject;

        testScript.MoveCapoOverFret(invalidNum);
        Assert.IsTrue(errorDisplayText.IsActive());//error display text is active if error
    }

    [Test]
    [TestCase(0)]
    [TestCase(7)]
    [TestCase(13)]
    [TestCase(16)]
    public void MoveCapoOverFret_ValidInput_SetActiveErrorMessageTMP(int validNum)
    {
        GameObject testObject = new GameObject();
        GameObject capoObject = new GameObject();
        TextMeshProUGUI errorDisplayText = testObject.AddComponent<TextMeshProUGUI>();
        CapoController testScript = testObject.AddComponent<CapoController>();

        // Set up errorDisplayText
        testScript.errorDisplayText = errorDisplayText;
        testScript.capo = capoObject;

        testScript.MoveCapoOverFret(validNum);
        Assert.IsFalse(errorDisplayText.IsActive());//error display text is inactive if no error
    }
    [Test]
    public void FingerNumInputElow_m66FingerNum_NoneDisplay()
    {
        int[] defaultArray = { 0, 0, 0, 0, 0, 0 };
        GameObject testObject = new GameObject();
        testObject.AddComponent<TextMeshProUGUI>();
        testObject.AddComponent<TMP_InputField>();
        testObject.GetComponent<TMP_InputField>().text = "-66";

        StringClickHandler testScript = testObject.AddComponent<StringClickHandler>();
        testScript.userInputFingerElow = testObject.GetComponent<TMP_InputField>();
        testScript.invalidFingerError = testObject.GetComponent<TextMeshProUGUI>();

        testScript.InputUserFingerNumArray(defaultArray);
        testScript.FingerNumInputElow();
        Assert.AreEqual("",testScript.userInputFingerElow.text);
    }
    [Test]
    public void FingerNumInputElow_66FingerNum_ErrorTextActive()
    {
        int[] defaultArray = { 0, 0, 0, 0, 0, 0 };
        GameObject testObject = new GameObject();
        testObject.AddComponent<TextMeshProUGUI>();
        testObject.AddComponent<TMP_InputField>();
        testObject.GetComponent<TMP_InputField>().text = "66";

        StringClickHandler testScript = testObject.AddComponent<StringClickHandler>();
        testScript.userInputFingerElow = testObject.GetComponent<TMP_InputField>();
        testScript.invalidFingerError = testObject.GetComponent<TextMeshProUGUI>();

        testScript.InputUserFingerNumArray(defaultArray);
        testScript.FingerNumInputElow();
        Assert.IsTrue(testScript.invalidFingerError.IsActive());
    }
    [Test]
    public void FingerNumInputElow_6FinNumCapoValid_ErrorTextInactive()
    {
        int[] defaultArray = { 0, 0, 0, 0, 0, 0 };
        GameObject testObject = new GameObject();
        GameObject capoObject = new GameObject();

        testObject.AddComponent<TextMeshProUGUI>();
        testObject.AddComponent<TMP_InputField>();
        testObject.GetComponent<TMP_InputField>().text = "6";

        StringClickHandler testScript = testObject.AddComponent<StringClickHandler>();
        testScript.userInputFingerElow = testObject.GetComponent<TMP_InputField>();
        testScript.invalidFingerError = testObject.GetComponent<TextMeshProUGUI>();
        testScript.fingerWrtCapoError = testObject.GetComponent<TextMeshProUGUI>();

        testScript.InputUserFingerNumArray(defaultArray);
        testScript.InputUserCapoNum(0);
        testScript.FingerNumInputElow();
        Assert.IsFalse(testScript.fingerWrtCapoError.IsActive());
    }
    [Test]
    public void FingerNumInputElow_6FinNumCapoValid_0()
    {
        int[] defaultArray = { 6, 0, 0, 0, 0, 0 };
        GameObject testObject = new GameObject();
        GameObject capoObject = new GameObject();

        testObject.AddComponent<TextMeshProUGUI>();
        testObject.AddComponent<TMP_InputField>();
        testObject.GetComponent<TMP_InputField>().text = "6";

        StringClickHandler testScript = testObject.AddComponent<StringClickHandler>();
        testScript.userInputFingerElow = testObject.GetComponent<TMP_InputField>();
        testScript.invalidFingerError = testObject.GetComponent<TextMeshProUGUI>();
        testScript.fingerWrtCapoError = testObject.GetComponent<TextMeshProUGUI>();

        testScript.InputUserFingerNumArray(defaultArray);
        testScript.InputUserCapoNum(9);
        testScript.FingerNumInputElow();
        Assert.AreEqual("",testScript.userInputFingerElow.text);
    }
    [Test]
    public void FingerNumInputElow_6FinNumCapoInValid_0()
    {
        int[] defaultArray = { 0, 0, 0, 0, 0, 0 };
        GameObject testObject = new GameObject();
        GameObject capoObject = new GameObject();

        testObject.AddComponent<TextMeshProUGUI>();
        testObject.AddComponent<TMP_InputField>();
        testObject.GetComponent<TMP_InputField>().text = "6";

        StringClickHandler testScript = testObject.AddComponent<StringClickHandler>();
        testScript.userInputFingerElow = testObject.GetComponent<TMP_InputField>();
        testScript.invalidFingerError = testObject.GetComponent<TextMeshProUGUI>();
        testScript.fingerWrtCapoError = testObject.GetComponent<TextMeshProUGUI>();

        testScript.InputUserFingerNumArray(defaultArray);
        testScript.InputUserCapoNum(9);
        testScript.FingerNumInputElow();
        Assert.AreEqual("", testScript.userInputFingerElow.text);
    }
    //Imp note: userFingerNum[0] = 0 inside the loop, incase you are debugging later trying to add more Unit Tests.
}
