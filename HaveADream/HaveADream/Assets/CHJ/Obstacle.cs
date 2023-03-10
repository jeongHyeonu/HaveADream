using UnityEngine;


public class Obstacle : MonoBehaviour
{



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            DataManager.Instance.HealthCurrent -= 2.0f;


        }
    }

}
