using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.IO;

public class AudioSimilarityTest
{
    //Test Working of our NeedleMan-Wunsch Algorithm, Take multiple scenarios and compare actual values with expected percentage outputs.
    [Test]
    public void JsonConverterToCharacter_ExactlySameNotes_100PercentSimilarity()//we compare completely similar pieces of music
    {
        StorePlayedNote note1index0 = new StorePlayedNote("A","None");
        StorePlayedNote note1index1 = new StorePlayedNote("B", "None");
        StorePlayedNote note1index2 = new StorePlayedNote("C", "None");
        StorePlayedNote note1index3 = new StorePlayedNote("D", "None");

        StorePlayedNote[] note1TestValue = { note1index0, note1index1, note1index2, note1index3};
        JsonCompare.JsonConverterToCharacter(note1TestValue,note1TestValue);
        Assert.AreEqual(100,JsonCompare.similarityNote);
    }

    [Test]
    public void JsonConverterToCharacter_CompletelyDifferentNotes_0PercentSimilarity()//we compare music completely different in terms of length, notes and modes played 
    {
        StorePlayedNote note1index0 = new StorePlayedNote("A", "None");
        StorePlayedNote note1index1 = new StorePlayedNote("B", "None");
        StorePlayedNote note1index2 = new StorePlayedNote("C", "None");
        StorePlayedNote note1index3 = new StorePlayedNote("D", "None");
        StorePlayedNote note2index0 = new StorePlayedNote("E","Bend");
        StorePlayedNote note2index1 = new StorePlayedNote("F", "Hammer");
        StorePlayedNote note2index2 = new StorePlayedNote("G", "Hammer");

        StorePlayedNote[] note1TestValue = { note1index0, note1index1, note1index2, note1index3};
        StorePlayedNote[] note2TestValue = { note2index0, note2index1, note2index2};
        JsonCompare.JsonConverterToCharacter(note1TestValue, note2TestValue);
        Assert.AreEqual(0, JsonCompare.similarityNote);
    }

    [Test]
    public void JsonConverterToCharacter_HalfEqualNotes_50PercentSimilarity()//we compare music completely different in terms of notes, but same length 
    {
        StorePlayedNote note1index0 = new StorePlayedNote("A", "None");
        StorePlayedNote note1index1 = new StorePlayedNote("B", "None");
        StorePlayedNote note1index2 = new StorePlayedNote("C", "None");
        StorePlayedNote note1index3 = new StorePlayedNote("D", "None");

        StorePlayedNote note2index0 = new StorePlayedNote("B", "Bend");
        StorePlayedNote note2index1 = new StorePlayedNote("F", "Hammer");
        StorePlayedNote note2index2 = new StorePlayedNote("D", "Hammer");
        StorePlayedNote note2index3 = new StorePlayedNote("G", "Hammer");

        StorePlayedNote[] note1TestValue = { note1index0, note1index1, note1index2, note1index3 };
        StorePlayedNote[] note2TestValue = { note2index0, note2index1, note2index2, note2index3 };
        JsonCompare.JsonConverterToCharacter(note1TestValue, note2TestValue);
        Assert.AreEqual(50, JsonCompare.similarityNote);
    }

    [Test]
    public void JsonConverterToCharacter_ManySimilarNotes_60orMoreandLessthan95PercentSimilarity()//we compare music slightly different in terms of notes as well as length, there should be no 100 % similarity here 
    {
        StorePlayedNote note1index0 = new StorePlayedNote("A", "None");
        StorePlayedNote note1index1 = new StorePlayedNote("B", "None");
        StorePlayedNote note1index2 = new StorePlayedNote("C", "None");
        StorePlayedNote note1index3 = new StorePlayedNote("D", "None");
        StorePlayedNote note1index4 = new StorePlayedNote("E", "None");
        StorePlayedNote note1index5 = new StorePlayedNote("F", "None");

        StorePlayedNote note2index0 = new StorePlayedNote("A", "None");
        StorePlayedNote note2index1 = new StorePlayedNote("C", "Bend");
        StorePlayedNote note2index2 = new StorePlayedNote("C", "Hammer");
        StorePlayedNote note2index3 = new StorePlayedNote("D", "Hammer");
        StorePlayedNote note2index4 = new StorePlayedNote("F", "None");

        StorePlayedNote[] note1TestValue = { note1index0, note1index1, note1index2, note1index3, note1index4, note1index5 };
        StorePlayedNote[] note2TestValue = { note2index0, note2index1, note2index2, note2index3, note2index4 };
        JsonCompare.JsonConverterToCharacter(note1TestValue, note2TestValue);
        Assert.IsTrue(JsonCompare.similarityNote > 60 && JsonCompare.similarityNote < 95);//Value must lie in this range for such little variation
    }

    [Test]
    public void JsonConverterToCharacter_DoubleLengthOfNotes_0PercentSimilarity()
    {
        StorePlayedNote note1index0 = new StorePlayedNote("A", "None");
        StorePlayedNote note1index1 = new StorePlayedNote("B", "None");
        StorePlayedNote note1index2 = new StorePlayedNote("C", "None");

        StorePlayedNote note2index0 = new StorePlayedNote("A", "None");
        StorePlayedNote note2index1 = new StorePlayedNote("C", "Bend");
        StorePlayedNote note2index2 = new StorePlayedNote("C", "Hammer");
        StorePlayedNote note2index3 = new StorePlayedNote("D", "Hammer");
        StorePlayedNote note2index4 = new StorePlayedNote("F", "None");

        StorePlayedNote[] note1TestValue = { note1index0, note1index1, note1index2 };
        StorePlayedNote[] note2TestValue = { note2index0, note2index1, note2index2,note2index3,note2index4 };
        JsonCompare.JsonConverterToCharacter(note1TestValue, note2TestValue);
        Assert.AreEqual(0, JsonCompare.similarityNote);
    }

    [Test]
    public void JsonConverterToCharacter_SameLengthDiffNotes_0PercentSimilarity()
    {
        StorePlayedNote note1index0 = new StorePlayedNote("A", "None");
        StorePlayedNote note1index1 = new StorePlayedNote("B", "None");
        StorePlayedNote note1index2 = new StorePlayedNote("C", "None");

        StorePlayedNote note2index0 = new StorePlayedNote("E", "None");
        StorePlayedNote note2index1 = new StorePlayedNote("F", "None");
        StorePlayedNote note2index2 = new StorePlayedNote("G", "None");

        StorePlayedNote[] note1TestValue = { note1index0, note1index1, note1index2};
        StorePlayedNote[] note2TestValue = { note2index0, note2index1, note2index2};
        JsonCompare.JsonConverterToCharacter(note1TestValue, note2TestValue);
        Assert.AreEqual(0, JsonCompare.similarityNote);
    }

    [Test]
    public void JsonConverterToCharacter_NoNotesPlayed_0PercentSimilarity()
    {
        StorePlayedNote note1index0 = new StorePlayedNote("A", "None");
        StorePlayedNote note1index1 = new StorePlayedNote("B", "None");
        StorePlayedNote note1index2 = new StorePlayedNote("C", "None");

        StorePlayedNote[] note1TestValue = { note1index0, note1index1, note1index2 };
        StorePlayedNote[] note2TestValue = {};
        JsonCompare.JsonConverterToCharacter(note1TestValue, note2TestValue);
        Assert.AreEqual(0, JsonCompare.similarityNote);
    }
}
