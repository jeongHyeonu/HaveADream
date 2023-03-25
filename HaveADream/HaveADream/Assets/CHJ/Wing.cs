using UnityEngine;

public class Wing : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
