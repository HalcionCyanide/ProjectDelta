using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

	public void Pause()
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
    }
}
