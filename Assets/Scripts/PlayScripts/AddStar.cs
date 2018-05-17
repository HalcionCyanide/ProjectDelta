using UnityEngine;

public class AddStar : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManagement.Instance.starCount++;
            Destroy(gameObject);
        }
    }
}
