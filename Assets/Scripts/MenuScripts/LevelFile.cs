using UnityEngine;
using UnityEngine.UI;

public class LevelFile : MonoBehaviour {

	public void LoadLevel()
    {
        GameManagement.Instance.levelToAccess = transform.GetChild(0).GetComponent<Text>().text;
    }
}
