using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class LevelLoader : MonoBehaviour {

    private string[,] LevelLayoutArray;
    private string[] textRow;

    //Object ID -1
    public GameObject playerPrefab;
    //Object ID 0
    public GameObject wallPrefab;
    //Object ID 99
    public GameObject StarOBJ;
    //Object ID 100
    public GameObject endFlagOBJ;

    public string levelOverride;
    TextAsset levelData;

    // Use this for initialization
    void Awake () {
        if (!String.IsNullOrEmpty(GameManagement.Instance.levelToAccess))
        {
            //load the level file to a storage space in memory
            levelData = Resources.Load("Levels/level" + GameManagement.Instance.levelToAccess) as TextAsset;
            //unload the value from the loader, we copied the data already.
            GameManagement.Instance.levelToAccess = null;
        }
        else
        {
            levelData = Resources.Load("Levels/level" + levelOverride) as TextAsset;
        }
        BuildLevel();
	}

    void BuildLevel()
    {
        //read the level file and make it
        GameObject lvlHolder = new GameObject("levelHolder");
        LevelLayoutArray = SplitCsvGrid(levelData.text);
        textRow = new string[LevelLayoutArray.GetUpperBound(0)];

        int height = ((LevelLayoutArray.Length / textRow.Length) - 1);
        int width = textRow.Length - 1;

        for (int y = 0; y < height; y++) // height
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 objPos = new Vector3(x, y, 0);

                switch (LevelLayoutArray[x, y])
                {
                    case "-1": //player
                        GameObject player = Instantiate(playerPrefab, objPos, Quaternion.identity);
                        Camera.main.GetComponent<SmoothCamera2D>().target = player.transform;
                        player.GetComponent<DragShotMover>().powerArrow = GameObject.FindGameObjectWithTag("Arrow");
                        break;
                    case "0": //wall
                        GameObject wall = Instantiate(wallPrefab, objPos, Quaternion.identity);
                        wall.transform.SetParent(lvlHolder.transform);
                        break;
                    case "99": //star
                        break;
                    case "100": //endFlag
                        break;
                    default:
                        //nothing here.
                        break;
                }
            }
        }
    }
    // splits a CSV file into a 2D string array
    static public string[,] SplitCsvGrid(string csvText)
    {
        string[] lines = csvText.Split("\n"[0]);

        // finds the max width of row
        int width = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = SplitCsvLine(lines[i]);
            width = Mathf.Max(width, row.Length);
        }

        // creates new 2D string grid to output to
        string[,] outputGrid = new string[width + 1, lines.Length + 1];
        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = SplitCsvLine(lines[y]);
            for (int x = 0; x < row.Length; x++)
            {
                outputGrid[x, y] = row[x];

                // This line was to replace "" with " in my output. 
                // Include or edit it as you wish.
                outputGrid[x, y] = outputGrid[x, y].Replace("\"\"", "\"");
            }
        }

        return outputGrid;
    }
    // splits a CSV row 
    static public string[] SplitCsvLine(string line)
    {
        return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
        @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
        System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                select m.Groups[1].Value).ToArray();
    }
}
