using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //���ǵ� ����
    [SerializeField] float speed;

    private int flickerTimer;
    private int flickerDuration;
    private float alpha = 200.0f;
    private int sign = -1;


    void Update()
    {
        //��ġ �� ���� ���� Ȯ�� �ʿ� , ������
        float v = Input.GetAxisRaw("Vertical");
        Vector2 curPos = transform.position;
        Vector2 nextPos = new Vector2(0, v) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;
        /* if (Input.touchCount > 0)
         {

         }*/

        //�ø�Ŀ
        if (flickerTimer >= 0 && flickerDuration <= 0)
        {
            flickerTimer--;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, alpha / 255);
            alpha += sign * 70;
            sign *= -1;
            flickerDuration = 10;
        }
        else if (flickerTimer < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        flickerDuration--;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Block") == 0)
        {
            Flicker();
        }
    }

    //Flicker = ������� ���
    private void Flicker()
    {
        flickerDuration = 10;
        sign = -1;
        alpha = 200.0f;
        flickerTimer = 6;
    }
}
