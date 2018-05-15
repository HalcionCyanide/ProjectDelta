using UnityEngine;
using System.IO;

public class DisplayStars : MonoBehaviour {

    int numberofLevels;
    string writeToCSV = "0";
    string dataPath;
    public GameObject starPrefab;
    public GameObject[] levelBlocks;

    // Use this for initialization
    void Awake () {
        numberofLevels = levelBlocks.Length;
        #region CROSSPLATFORM
#if UNITY_STANDALONE
        dataPath = "Assets/Resources/PlayerData/Stars.csv";
#else
        dataPath = Application.persistentDataPath + "Stars.csv";
#endif
        #endregion
        //Attempt to check the resource file
        TextAsset starData = Resources.Load("PlayerData/Stars") as TextAsset;

        //no prior starData
        if(!starData)
        {
            Debug.Log("No prior savefile found, creating new file");
            //number of commas = levels - 1
            for (int i = 0; i < numberofLevels - 1; i++)
            {
                writeToCSV += ",0";
            }

            //write this to file
            File.WriteAllText(dataPath, writeToCSV);
        }
        //there is starData
        for (int i = 0; i < numberofLevels; i++)
        {
            string starInfo = File.ReadAllText(dataPath);
            //the position of the level value is 
            int checkingPosition = i * 2;
            //number of stars the level has was
            int starsInLevel = starInfo[checkingPosition] - 48;

            for(int x = 0; x < starsInLevel; x++)
            {
                GameObject star = Instantiate(starPrefab);
                star.transform.SetParent(levelBlocks[i].transform, false);
            }
        }
	}

    public void WriteStarsToLevel(int level, int newStars)
    {
        string starInfo = File.ReadAllText(dataPath);
        int writePos = (level - 1) * 2;

        Debug.Log(newStars + " , " + (starInfo[writePos] - 48));
        if (newStars > (starInfo[writePos] - 48))
        {
            //more stars
            starInfo = starInfo.Remove(writePos, 1);
            starInfo = starInfo.Insert(writePos, newStars.ToString());
            File.WriteAllText(dataPath, starInfo);
        }
        else
            return;
    }
}
