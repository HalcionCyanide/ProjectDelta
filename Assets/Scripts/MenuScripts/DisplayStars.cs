using UnityEngine;
using System.IO;

public class DisplayStars : MonoBehaviour {

    int numberofLevels;
    public GameObject starPrefab;
    public GameObject[] levelBlocks;

    // Use this for initialization
    void Awake () {
        numberofLevels = levelBlocks.Length;
        //there is starData
        for (int i = 0; i < numberofLevels; i++)
        {
            string starInfo = File.ReadAllText(GameManagement.Instance.starPath);
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
}
