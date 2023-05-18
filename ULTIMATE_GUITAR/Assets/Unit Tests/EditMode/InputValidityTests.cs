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
}
