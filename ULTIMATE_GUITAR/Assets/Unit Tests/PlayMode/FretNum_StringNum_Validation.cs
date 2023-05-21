using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class FretNum_StringNum_Validation
{
    [UnityTest]
    public IEnumerator ReturnFretNum_5_5()
    {
        GameObject testObject = new GameObject();
        testObject.AddComponent<FretNum>();
        testObject.AddComponent<Rigidbody>();
        testObject.name = "5";

        testObject.GetComponent<FretNum>().Start();
        Assert.AreEqual(5,testObject.GetComponent<FretNum>().Return_Fret_Num());
        yield return null;
    }
    [UnityTest]
    public IEnumerator ReturnFretNum_23_23()
    {
        GameObject testObject = new GameObject();
        testObject.AddComponent<FretNum>();
        testObject.AddComponent<Rigidbody>();
        testObject.name = "14";

        testObject.GetComponent<FretNum>().Start();
        Assert.AreEqual(14, testObject.GetComponent<FretNum>().Return_Fret_Num());
        yield return null;
    }
    [UnityTest]
    public IEnumerator ReturnFretNum_12_12()
    {
        GameObject testObject = new GameObject();
        testObject.AddComponent<FretNum>();
        testObject.AddComponent<Rigidbody>();
        testObject.name = "23";

        testObject.GetComponent<FretNum>().Start();
        Assert.AreEqual(23, testObject.GetComponent<FretNum>().Return_Fret_Num());
        yield return null;
    }

    [UnityTest]
    public IEnumerator ReturnStringNum_Elow_1()
    {
        GameObject testObject = new GameObject();
        testObject.AddComponent<Rigidbody>();
        testObject.AddComponent<GuitarString>();
        testObject.name = "Elow";

        testObject.GetComponent<GuitarString>().Awake();
        Assert.AreEqual(1, testObject.GetComponent<GuitarString>().Return_String_Number());
        yield return null;
    }

    [UnityTest]
    public IEnumerator ReturnStringNum__D_3()
    {
        GameObject testObject = new GameObject();
        testObject.AddComponent<Rigidbody>();
        testObject.AddComponent<GuitarString>();
        testObject.name = "D";

        testObject.GetComponent<GuitarString>().Awake();
        Assert.AreEqual(3, testObject.GetComponent<GuitarString>().Return_String_Number());
        yield return null;
    }
    [UnityTest]
    public IEnumerator ReturnStringNum_G_4()
    {
        GameObject testObject = new GameObject();
        testObject.AddComponent<Rigidbody>();
        testObject.AddComponent<GuitarString>();
        testObject.name = "G";

        testObject.GetComponent<GuitarString>().Awake();
        Assert.AreEqual(4, testObject.GetComponent<GuitarString>().Return_String_Number());
        yield return null;
    }
}
