using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelAccess : MonoBehaviour {

    // Use this for initialization
    private void Awake()
    {
        //determine unlocked levels.
        int bestLevel = Int32.Parse(File.ReadAllText(GameManagement.Instance.accessPath));

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
}
