using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManagement.Instance.CompleteLevel();
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
