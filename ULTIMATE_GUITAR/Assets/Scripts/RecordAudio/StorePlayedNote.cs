using System;

[Serializable]
public class StorePlayedNote
{
    public string note;
    public string mode;

    public StorePlayedNote(string Note, string Mode)
    {
        note = Note;
        mode = Mode;
    }
}
