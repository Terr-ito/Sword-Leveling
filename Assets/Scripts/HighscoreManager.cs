using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager instance;

    private string highscorePath;
    private HighscoreList highscores;
    private const int MAX_HIGHSCORES = 3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        highscorePath = Application.persistentDataPath + "/highscores.json";
        LoadHighscores();
    }

    /// <summary>
    /// Loads highscores from JSON file
    /// </summary>
    void LoadHighscores()
    {
        if (File.Exists(highscorePath))
        {
            try
            {
                string encoded = File.ReadAllText(highscorePath);
                string json = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(encoded));
                highscores = JsonUtility.FromJson<HighscoreList>(json);
            }
            catch
            {
                Debug.LogWarning("Failed to load highscores (corrupted file). Creating new list.");
                highscores = new HighscoreList();
            }
        }
        else
        {
            highscores = new HighscoreList();
        }
    }

    /// <summary>
    /// Saves highscores to JSON file
    /// </summary>
    void SaveHighscores()
    {
        string json = JsonUtility.ToJson(highscores, true);
        string encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
        File.WriteAllText(highscorePath, encoded);
        Debug.Log($"Highscores saved to: {highscorePath}");
    }

    /// <summary>
    /// Checks if a time qualifies for the highscore list
    /// Returns position (1, 2, 3) if it qualifies, or -1 if it doesn't
    /// </summary>
    public int CheckHighscorePosition(string timeString)
    {
        // Convert time string to seconds for comparison
        float newTime = TimeStringToSeconds(timeString);

        // If less than 3 entries, automatically qualify
        if (highscores.entries.Count < MAX_HIGHSCORES)
        {
            return highscores.entries.Count + 1;
        }

        // Get the worst time in top 3
        float worstTime = 0f;
        foreach (var entry in highscores.entries)
        {
            float entryTime = TimeStringToSeconds(entry.time);
            if (entryTime > worstTime)
            {
                worstTime = entryTime;
            }
        }

        // Check if new time is better than worst time
        if (newTime < worstTime)
        {
            // Find the position
            for (int i = 0; i < highscores.entries.Count; i++)
            {
                float entryTime = TimeStringToSeconds(highscores.entries[i].time);
                if (newTime < entryTime)
                {
                    return i + 1;
                }
            }
            return MAX_HIGHSCORES;
        }

        return -1; // Doesn't qualify
    }

    /// <summary>
    /// Adds a new highscore entry
    /// </summary>
    public void AddHighscore(string playerName, string timeString)
    {
        int position = CheckHighscorePosition(timeString);

        if (position == -1)
        {
            Debug.LogWarning("Time doesn't qualify for highscores!");
            return;
        }

        // Create new entry
        HighscoreEntry newEntry = new HighscoreEntry(playerName, timeString, position);

        // Insert at correct position (1-indexed to 0-indexed)
        highscores.entries.Insert(position - 1, newEntry);

        // Remove the worst entry if we exceed max
        if (highscores.entries.Count > MAX_HIGHSCORES)
        {
            highscores.entries.RemoveAt(MAX_HIGHSCORES);
        }

        // Update all positions
        for (int i = 0; i < highscores.entries.Count; i++)
        {
            highscores.entries[i].place = i + 1;
        }

        SaveHighscores();
        Debug.Log($"Added highscore: {playerName} - {timeString} at position {position}");
    }

    /// <summary>
    /// Gets all highscore entries (sorted by position)
    /// </summary>
    public List<HighscoreEntry> GetHighscores()
    {
        return highscores.entries;
    }

    /// <summary>
    /// Gets highscore entries as formatted strings for UI display
    /// </summary>
    public List<string> GetFormattedHighscores()
    {
        List<string> formatted = new List<string>();

        if (highscores == null || highscores.entries == null)
        {
            return formatted;
        }

        foreach (var entry in highscores.entries)
        {
            formatted.Add($"{entry.place}. {entry.playerName} - {entry.time}");
        }

        return formatted;
    }

    /// <summary>
    /// Converts time string (MM:SS format) to total seconds
    /// </summary>
    private float TimeStringToSeconds(string timeStr)
    {
        string[] parts = timeStr.Split(':');
        if (parts.Length == 2 && float.TryParse(parts[0], out float minutes) && float.TryParse(parts[1], out float seconds))
        {
            return (minutes * 60) + seconds;
        }
        return float.MaxValue; // If parsing fails, treat as worst time
    }

    /// <summary>
    /// Clears all highscores (useful for testing)
    /// </summary>
    public void ClearHighscores()
    {
        highscores.entries.Clear();
        SaveHighscores();
        Debug.Log("Highscores cleared!");
    }
}
