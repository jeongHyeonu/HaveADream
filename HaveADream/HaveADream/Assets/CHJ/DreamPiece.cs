using UnityEngine;



public class DreamPiece : MonoBehaviour
{

    [SerializeField] GameObject target;
    [SerializeField] float speed = 5.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            //������ ī��Ʈ
            DataManager.Instance.DreamPieceScore += 1;
            //ȭ�鿡�� ����
            gameObject.SetActive(false);
        }
        if (collision.gameObject.tag.CompareTo("MagneticField") == 0)
        {
            //�Ų����� �����̰� �ϱ�
            Vector3 pos = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * speed * 1f);
            transform.position = pos;

        }
    }
}
