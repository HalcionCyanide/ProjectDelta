using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeStars : MonoBehaviour {

    int currentStars = 0;
    public GameObject uiStarPrefab;

	// Update is called once per frame
	void Update () {
		if(currentStars != GameManagement.Instance.starCount)
        {
            currentStars = GameManagement.Instance.starCount;

            if(transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }

            for (int i = 0; i < currentStars; i++)
            {
                GameObject uiStar = Instantiate(uiStarPrefab);
                uiStar.transform.SetParent(transform, false);
            }
        }
	}
}
