using UnityEngine;



public class DreamPiece : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            //������ ī��Ʈ
            DataManager.Instance.DreamPieceScore += 1;
            //ȭ�鿡�� ����
            gameObject.SetActive(false);
        }
    }
}
