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

    //level completion info:
    [HideInInspector]
    public int starCount;

    // Use this for initialization
    void Awake () {
        if (!String.IsNullOrEmpty(GameManagement.Instance.levelToAccess))
        {
            //load the level file to a storage space in memory
            levelData = Resources.Load("Levels/level" + GameManagement.Instance.levelToAccess) as TextAsset;
        }
        else
        {
            levelData = Resources.Load("Levels/level" + levelOverride) as TextAsset;
            GameManagement.Instance.levelToAccess = levelOverride;
        }
        starCount = 0;
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

        float cameraHalfWidth = Camera.main.OrthographicBounds().size.x / 2;
        float cameraHalfHeight = Camera.main.OrthographicBounds().size.y / 2;
        Camera.main.GetComponent<CameraHandler>().SetBounds(new Vector2(cameraHalfWidth - 0.5f, width - cameraHalfWidth - 0.5f), new Vector2(cameraHalfHeight - 0.5f, height - cameraHalfHeight - 0.5f));
        Camera.main.GetComponent<CameraHandler>().BoundsX = new Vector2(cameraHalfWidth - 0.5f, width - cameraHalfWidth - 0.5f);
        Camera.main.GetComponent<CameraHandler>().BoundsY = new Vector2(cameraHalfHeight - 0.5f, height - cameraHalfHeight - 0.5f);
        for (int y = 0; y < height; y++) // height
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 objPos = new Vector3(x, y, 0);

                switch (LevelLayoutArray[x, height - y - 1])
                {
                    case "-1": //player
                        GameObject player = Instantiate(playerPrefab, objPos, Quaternion.identity);
                        Camera.main.GetComponent<CameraHandler>().target = player.transform;
                        break;
                    case "0": //wall
                        GameObject wall = Instantiate(wallPrefab, objPos, Quaternion.identity);
                        wall.transform.SetParent(lvlHolder.transform);
                        break;
                    case "99": //star
                        GameObject star = Instantiate(StarOBJ, objPos, Quaternion.identity);
                        star.transform.SetParent(lvlHolder.transform);
                        break;
                    case "100": //endFlag
                        GameObject endFlag = Instantiate(endFlagOBJ, objPos, Quaternion.identity);
                        endFlag.transform.SetParent(lvlHolder.transform);
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
