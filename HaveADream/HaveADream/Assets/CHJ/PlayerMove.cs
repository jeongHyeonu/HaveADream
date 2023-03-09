using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //스피드 조절
    [SerializeField] float speed;

    private int flickerTimer;
    private int flickerDuration;
    private float alpha = 200.0f;
    private int sign = -1;


    void Update()
    {
        //터치 시 상하 구현 확인 필요 , 움직임
        float v = Input.GetAxisRaw("Vertical");
        Vector2 curPos = transform.position;
        Vector2 nextPos = new Vector2(0, v) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;
        /* if (Input.touchCount > 0)
         {

         }*/

        //플리커
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

    //Flicker = 깜빡대는 기능
    private void Flicker()
    {
        flickerDuration = 10;
        sign = -1;
        alpha = 200.0f;
        flickerTimer = 6;
    }
}
