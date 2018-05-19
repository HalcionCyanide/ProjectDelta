using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    GameObject endPanel;

    private void Start()
    {
        endPanel = GameObject.FindGameObjectWithTag("UI_FINISH");
        endPanel.SetActive(false);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Camera.main.GetComponent<CameraHandler>().target = transform;
            GameManagement.Instance.CompleteLevel();
            gameObject.GetComponent<Collider2D>().enabled = false;

            //activate the level complete thing
            endPanel.SetActive(true);
        }
    }
}
