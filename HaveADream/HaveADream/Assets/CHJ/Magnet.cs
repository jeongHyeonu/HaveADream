using UnityEngine;

public class Magnet : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<DreamPiece>(out DreamPiece dp))
        {
            dp.target = gameObject;
        }
        if (collision.gameObject.TryGetComponent<Jewel>(out Jewel jw))
        {
            jw.targetj = gameObject;
        }
    }
}
