using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStars : MonoBehaviour {

    public GameObject uiStarPrefab;

    private void Start()
    {
        for (int i = 0; i < GameManagement.Instance.starCount; i++)
        {
            GameObject uiStar = Instantiate(uiStarPrefab);
            uiStar.transform.SetParent(transform, false);
        }
    }
}
