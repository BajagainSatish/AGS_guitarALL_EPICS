using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class JsonCompare
{
    // Specify the path to your JSON file
    private static readonly string filePathOriginal = @"C:\Users\Administrator\AppData\LocalLow\DefaultCompany\ULTIMATE_GUITAR\TestReference.json";
    private static readonly string filePathUser = @"C:\Users\Administrator\AppData\LocalLow\DefaultCompany\ULTIMATE_GUITAR\UserPlay.json";

    // Read the contents of the JSON file into a string
    private static string jsonStringOriginal;
    private static string jsonStringUser;

    // Parse the JSON strings into arrays of StorePlayedNote objects
    private static StorePlayedNote[] notes1;
    private static StorePlayedNote[] notes2;

    private static int countHalf;
    public static void DisplayNotes()
    {
        // Read the contents of the JSON file into a string
        jsonStringOriginal = File.ReadAllText(filePathOriginal);
        jsonStringUser = File.ReadAllText(filePathUser);

        //Debug.Log("jsonStringOriginal: " + jsonStringOriginal);//works
        //Debug.Log("jsonStringUser: " + jsonStringUser);

        // Parse the JSON strings into arrays of StorePlayedNote objects
        notes1 = JsonHelper.FromJson<StorePlayedNote>(jsonStringOriginal);
        notes2 = JsonHelper.FromJson<StorePlayedNote>(jsonStringUser);

        // Compare the arrays for similarity
        bool areSimilar = CompareNotes(notes1, notes2);
        Debug.Log("The JSON files are " + (areSimilar ? "similar" : "different"));
    }
    private static bool CompareNotes(StorePlayedNote[] notes1, StorePlayedNote[] notes2)
    {
        // If either array is null, they are not similar
        if (notes1 == null || notes2 == null)
        {
            Debug.Log("Null value error!!!");
            return false;
        }
        Debug.Log("Notes length: " + notes2.Length);
        for (int i = 0; i < notes2.Length; i++)
        {
            Debug.Log("notes2[" + i + "].note: " + notes2[i].note);
            Debug.Log("notes2[" + i + "].mode: " + notes2[i].mode);
        }

        // Check each note in the arrays for similarity
        for (int i = 0; i < notes1.Length; i++)
        {
            if (notes1[i].note != notes2[i].note)
            {
                return false;
            }
        }
        return true;
    }
}