using System;
using System.Collections.Generic;

[System.Serializable]
public class HighscoreEntry
{
    public string playerName;
    public string time;
    public int place;

    public HighscoreEntry(string name, string survivedTime, int position)
    {
        playerName = name;
        time = survivedTime;
        place = position;
    }
}

[System.Serializable]
public class HighscoreList
{
    public List<HighscoreEntry> entries = new List<HighscoreEntry>();
}
