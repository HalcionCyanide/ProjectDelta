using UnityEngine;

public class AddStar : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManagement.Instance.starCount++;
            Destroy(gameObject);
        }
    }
}
