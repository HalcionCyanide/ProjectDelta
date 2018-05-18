using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnStart : MonoBehaviour {

    public GameObject obj;

	// Use this for initialization
	void Awake () {
        if (!obj.activeSelf)
            obj.SetActive(true);
	}
}
