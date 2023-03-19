using UnityEngine;



public class DreamPiece : MonoBehaviour
{

    [SerializeField] GameObject target;
    [SerializeField] float speed = 5.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            //꿈조각 카운트
            DataManager.Instance.DreamPieceScore += 1;
            //화면에서 끄기
            gameObject.SetActive(false);
        }
        if (collision.gameObject.tag.CompareTo("MagneticField") == 0)
        {
            //매끄럽게 움직이게 하기
            Vector3 pos = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * speed * 1f);
            transform.position = pos;

        }
    }
}
