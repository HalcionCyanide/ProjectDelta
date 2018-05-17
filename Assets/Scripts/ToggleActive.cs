using UnityEngine;

public class ToggleActive : MonoBehaviour {

    public GameObject itemToToggle;

	public void Toggle()
    {
        itemToToggle.SetActive(itemToToggle.activeSelf ? false : true); 
    }
}
