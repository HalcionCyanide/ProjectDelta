using System;
using System.IO;
using UnityEngine;

public class GameManagement : SingletonTemplate<GameManagement> {
    
    [HideInInspector]
    public string starPath;
    [HideInInspector]
    public string accessPath;
    [HideInInspector]
    public TextAsset accessData;
    [HideInInspector]
    public TextAsset starData;

    [HideInInspector]
    public string levelToAccess;

    int defaultLevel = 1;
    readonly int numberOfLevels = 15;

    [HideInInspector]
    public int starCount = 0;

    // Use this for initialization
    void Awake () {
        #region CROSSPLATFORM
#if UNITY_STANDALONE
        starPath = "Assets/Resources/PlayerData/Stars.csv";
        accessPath = "Assets/Resources/PlayerData/Access.txt";
#else
        starPath = Application.persistentDataPath + "Stars.csv";
        accessPath = Application.persistentDataPath + "Access.txt";
#endif
        #endregion

        accessData = Resources.Load("PlayerData/Access") as TextAsset;
        if (!accessData)
        {
            Debug.Log("No prior access file found, creating new file");

            File.WriteAllText(accessPath, defaultLevel.ToString());
        }

        starData = Resources.Load("PlayerData/Stars") as TextAsset;
        if (!starData)
        {
            string writeToCSV = "0";
            Debug.Log("No prior star file found, creating new file");
            //number of commas = levels - 1
            for (int i = 0; i < numberOfLevels - 1; i++)
            {
                writeToCSV += ",0";
            }
            //write this to file
            File.WriteAllText(starPath, writeToCSV);
        }
    }

    public void ChangeAccessLevel(int newAccess)
    {
        string accessInfo = File.ReadAllText(accessPath);

        if (newAccess > Int32.Parse(accessInfo))
        {
            File.WriteAllText(accessPath, newAccess.ToString());
        }
    }

    public void WriteStarsToLevel(int level, int newStars)
    {
        string starInfo = File.ReadAllText(starPath);
        int writePos = (level - 1) * 2;

        Debug.Log(newStars + " , " + (starInfo[writePos] - 48));
        if (newStars > (starInfo[writePos] - 48))
        {
            //more stars
            starInfo = starInfo.Remove(writePos, 1);
            starInfo = starInfo.Insert(writePos, newStars.ToString());
            File.WriteAllText(starPath, starInfo);
        }
        else
            return;
    }

    public void CompleteLevel()
    {
        ChangeAccessLevel(Int32.Parse(levelToAccess) + 1);
        WriteStarsToLevel(Int32.Parse(levelToAccess), starCount);
    }
}
