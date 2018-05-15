using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelAccess : MonoBehaviour {

    int bestLevel = 1;
    string dataPath;

    // Use this for initialization
    private void Awake()
    {
        #region CROSSPLATFORM
#if UNITY_STANDALONE
        dataPath = "Assets/Resources/PlayerData/Access.txt";
#else
        dataPath = Application.persistentDataPath + "Access.txt";
#endif
        #endregion
        TextAsset accessData = Resources.Load("PlayerData/Access") as TextAsset;

        if(!accessData)
        {
            Debug.Log("No prior access file found, creating new file");

            File.WriteAllText(dataPath, bestLevel.ToString());
        }

        //determine unlocked levels.
        bestLevel = Int32.Parse(File.ReadAllText(dataPath));

        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Level");

        foreach (GameObject x in buttons)
        {
            string levelNumber = x.transform.GetChild(0).GetComponent<Text>().text;
            if (Int32.Parse(levelNumber) > bestLevel)
            {
                x.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void ChangeAccessLevel(int newAccess)
    {
        string accessInfo = File.ReadAllText(dataPath);

        if(newAccess > Int32.Parse(accessInfo))
        {
            File.WriteAllText(dataPath, newAccess.ToString());
        }
    }
}
