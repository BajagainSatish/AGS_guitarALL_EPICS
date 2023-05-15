using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;

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

    private static readonly Dictionary<string, string> chromaticNoteConverter = new Dictionary<string, string>() {
{"A","H"},
{"A#","I"},
{"B","J"},
{"C","K"},
{"C#","L"},
{"D","M"},
{"D#","N"},
{"E","O"},
{"F","P"},
{"F#","Q"},
{"G","R"},
{"G#","S"}
    };
    private static readonly Dictionary<string, string> chromaticModeConverter = new Dictionary<string, string>() {
{"None","T"},
{"Hammer","U"},
{"PullOff","V"},
{"Slide","W"},
{"Bend","X"},
{"Strum","Y"}
    };
    private static readonly string[] chromaticNoteList = new string[] {"A","A#","B","C","C#","D","D#","E","F","F#","G","G#"};
    private static readonly string[] chromaticModeList = new string[] { "None", "Hammer", "PullOff", "Slide", "Bend", "Strum" };

    public static double similarityNote, similarityMode;
    public static void DisplayNotes()
    {
        // Read the contents of the JSON file into a string
        jsonStringOriginal = File.ReadAllText(filePathOriginal);
        jsonStringUser = File.ReadAllText(filePathUser);

        //Debug.Log("jsonStringOriginal: " + jsonStringOriginal);//works
        Debug.Log("jsonStringUser: " + jsonStringUser);

        // Parse the JSON strings into arrays of StorePlayedNote objects
        notes1 = JsonHelper.FromJson<StorePlayedNote>(jsonStringOriginal);
        notes2 = JsonHelper.FromJson<StorePlayedNote>(jsonStringUser);

        // Compare the arrays for similarity
        bool areSimilar = CompareNotes(notes1, notes2);
        Debug.Log("The JSON files are " + (areSimilar ? "similar" : "different"));

        JsonConverterToCharacter(notes1,notes2);
    }
    private static bool CompareNotes(StorePlayedNote[] notes1, StorePlayedNote[] notes2)
    {
        // If either array is null, they are not similar
        if (notes1 == null || notes2 == null)
        {
            Debug.Log("Null value error!!!");
            return false;
        }
        //Debug.Log("Notes length: " + notes2.Length);
        for (int i = 0; i < notes2.Length; i++)
        {
            //Debug.Log("notes2[" + i + "].note: " + notes2[i].note);
            //Debug.Log("notes2[" + i + "].mode: " + notes2[i].mode);
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

    private static void JsonConverterToCharacter(StorePlayedNote[] jsonDataOriginal, StorePlayedNote[] jsonDataUser)
    {
        int noteMatch, modeMatch;
        string[] encodedArrayNotesOriginal = new string[jsonDataOriginal.Length];
        string[] encodedArrayModesOriginal = new string[jsonDataOriginal.Length];

        string[] encodedArrayNotesUser = new string[jsonDataUser.Length];
        string[] encodedArrayModesUser = new string[jsonDataUser.Length];

        for (int i = 0; i < jsonDataOriginal.Length; i++)
        {
            for (int j = 0; j < 12; j++)
            {
            if (jsonDataOriginal[i].note == chromaticNoteList[j])
            {
                    encodedArrayNotesOriginal[i] = chromaticNoteConverter[jsonDataOriginal[i].note];
                    //Debug.Log(encodedArrayNotes[i] + jsonData[i].note);
                    break;
            }
            }
        }
        for (int i = 0; i < jsonDataUser.Length; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                if (jsonDataUser[i].note == chromaticNoteList[j])
                {
                    encodedArrayNotesUser[i] = chromaticNoteConverter[jsonDataUser[i].note];
                    //Debug.Log(encodedArrayNotes[i] + jsonData[i].note);
                    break;
                }
            }
        }
        for (int i = 0; i < jsonDataOriginal.Length; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (jsonDataOriginal[i].mode == chromaticModeList[j])
                {
                    encodedArrayModesOriginal[i] = chromaticModeConverter[jsonDataOriginal[i].mode];
                    //Debug.Log(encodedArrayModes[i] + jsonData[i].mode);
                    break;
                }
            }
        }
        for (int i = 0; i < jsonDataUser.Length; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (jsonDataUser[i].mode == chromaticModeList[j])
                {
                    encodedArrayModesUser[i] = chromaticModeConverter[jsonDataUser[i].mode];
                    //Debug.Log(encodedArrayModes[i] + jsonData[i].mode);
                    break;
                }
            }
        }
        string encodedNotesOriginal = string.Join("", encodedArrayNotesOriginal);
        string encodedModesOriginal = string.Join("",encodedArrayModesOriginal);
        string encodedNotesUser = string.Join("", encodedArrayNotesUser);
        string encodedModesUser = string.Join("", encodedArrayModesUser);

        noteMatch = NeedlemanWunsch(encodedNotesOriginal,encodedNotesUser);
        modeMatch = NeedlemanWunsch(encodedModesOriginal,encodedModesUser);

        Debug.Log("Total Matches: " + noteMatch);
        Debug.Log("Original Length: " + jsonDataOriginal.Length);

        similarityNote = ((double)noteMatch / (double)jsonDataOriginal.Length) * 100;//We're taking string 1 as reference.
        similarityMode = ((double)modeMatch / (double)jsonDataOriginal.Length) * 100;
        Debug.Log("Note Similarity: " + similarityNote + " Mode Similarity: " + similarityMode);
    }

    private static int NeedlemanWunsch(string s1, string s2)
    {
        int n = s1.Length;
        int m = s2.Length;
        int i, j;
        // Initialize the dp matrix with gap penalties
        int[,] dp = new int[n + 1, m + 1];
        for (i = 1; i <= n; i++)
        {
            dp[i, 0] = -i;
        }
        for (j = 1; j <= m; j++)
        {
            dp[0, j] = -j;
        }

        // Fill in the dp matrix
        for (i = 1; i <= n; i++)
        {
            for (j = 1; j <= m; j++)
            {
                int matchScore = (s1[i - 1] == s2[j - 1]) ? 1 : -1;
                int diagonal = dp[i - 1, j - 1] + matchScore;
                int left = dp[i, j - 1] - 1;
                int up = dp[i - 1, j] - 1;
                dp[i, j] = Mathf.Max(Mathf.Max(diagonal, left), up);
            }
        }

        // Trace back through the dp matrix to find the optimal alignment
        i = n;
        j = m;
        int matches = 0;
            while (i > 0 && j > 0)
            {
            int matchScore = (s1[i - 1] == s2[j - 1]) ? 1 : -1;
            if (dp[i,j] == dp[i - 1,j - 1] + matchScore)
            {
                matches += (matchScore == 1) ? 1 : 0;
                i--;
                j--;
            }
            else if (dp[i,j] == dp[i,j - 1] - 1)
            {
                j--;
            }
            else
            {
                i--;
            }
            }

        return matches;
    }
}