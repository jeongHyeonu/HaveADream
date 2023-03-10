using UnityEngine;



public class DreamPiece : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            //꿈조각 카운트
            DataManager.Instance.DreamPieceScore += 1;
            //화면에서 끄기
            gameObject.SetActive(false);
        }
    }
}
