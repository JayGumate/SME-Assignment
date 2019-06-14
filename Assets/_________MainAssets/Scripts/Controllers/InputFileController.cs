using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFileController : MonoBehaviour
{
    private string currentDifficulty = Keys.Strings.NORMAL;

    public string CurrentDifficulty { get { return currentDifficulty; } }

    private Dictionary<int, string> stats = new Dictionary<int, string>();
    private List<string> availableDifficulties = new List<string>();
    private void Start()
    {
        LoadDifficulty();
    }

    public void LoadDifficulty()
    {
        TextAsset statsFile = Resources.Load<TextAsset>(Keys.InputFiles.ENEMY_STATS_FILE_PATH);

        var lines = statsFile.text.Split('\n');
        int difficultyIndex = -1;

        if (stats.Count > 0)
        {
            stats.Clear();
        }
        if (availableDifficulties.Count > 0)
        {
            availableDifficulties.Clear();
        }

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            var part = line.Split(',');
            if (i == 0)
            {
                for (int j = 1; j < part.Length; j++)
                {
                    availableDifficulties.Add(part[j]);

                    if (part[j] == currentDifficulty)
                    {
                        difficultyIndex = j;
                    }
                }

                if (difficultyIndex == -1)
                {
                    difficultyIndex = 1;
                    currentDifficulty = part[difficultyIndex - 1];
                }
            }
            else
            {
                string key = part[0];
                stats[i] = part[difficultyIndex];
            }
        }
    }

    public int GetStats(Keys.InputFiles.StatsKeys key, int prefabNumber)
    {
        if (!stats.ContainsKey((int)key + prefabNumber))
        {
            Debug.LogErrorFormat("stat in Difficulty: {0} is missing key: {1}", currentDifficulty, key);
            return (int)key;
        }
        return int.Parse(stats[(int)key + prefabNumber]);
    }

    public List<string> GetAvalaibleDifficulties()
    {
        return availableDifficulties;
    }

    public void ChangeDifficulty(string difficulty)
    {
        currentDifficulty = difficulty;
        LoadDifficulty();
    }
}
